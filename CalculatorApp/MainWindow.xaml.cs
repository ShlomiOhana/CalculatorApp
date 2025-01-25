using CalculatorApp.Enums;
using CalculatorApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace CalculatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<HistoryEntry> HistoryEntries;
        private List<HistoryStats> HistoryStats;

        public MainWindow()
        {
            InitializeComponent();
            HistoryEntries = new();
            HistoryStats = new();
        }

        private async void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double value1, value2;

            if (ValidateData())
            {
                double.TryParse(Number1TextBox.Text, out value1);
                double.TryParse(Number2TextBox.Text, out value2);
                OperationType operation = GetEnumValue(OperationComboBox.Text);

                GetHistoryStats((byte)operation);
                GetHistoryLastThreeEntries((byte)operation);
                double result = CalculateResult(value1, value2, operation);

                // Update Result
                ResultTextBlock.Text = $"Result: {result.ToString()}";
                var historyEntry = new HistoryEntry
                {
                    Date = DateTime.Now,
                    Field1 = (double)value1,
                    Field2 = (double)value2,
                    Operation = (byte)operation,
                    Result = result
                };
                AddToHistoryDb(historyEntry);
            }
        }

        private double CalculateResult(double number1, double number2, OperationType operation)
        {
            double result = 0;
            switch (operation)
            {
                case OperationType.Add:
                    result = ((double)number1 + (double)number2);
                    break;
                case OperationType.Subtract:
                    result = ((double)number1 - (double)number2);
                    break;
                case OperationType.Multiply:
                    result = ((double)number1 * (double)number2);
                    break;
                case OperationType.Divide:
                    result = ((double)number1 / (double)number2);
                    break;
            }
            return Math.Round(result, 2);
        }

        private bool ValidateData()
        {
            double value1, value2;

            if (double.TryParse(Number1TextBox.Text, out value1))
            {
                if (double.TryParse(Number2TextBox.Text, out value2))
                {
                    OperationType operation = GetEnumValue(OperationComboBox.Text);
                    if (operation == OperationType.Divide && value2 == 0)
                    {
                        MessageBox.Show("Cannot divide by zero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Number2 field is not a valid number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Number1 field is not a valid number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private OperationType GetEnumValue(string value)
        {
            switch (value)
            {
                case "Add":
                    return OperationType.Add;
                case "Subtract":
                    return OperationType.Subtract;
                case "Multiply":
                    return OperationType.Multiply;
                case "Divide":
                    return OperationType.Divide;
                default:
                    return OperationType.None;
            }
        }

        private async void AddToHistoryDb(HistoryEntry entry)
        {
            var jsonEntry = JsonConvert.SerializeObject(entry);
            var content = new StringContent(jsonEntry, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            await httpClient.PostAsync("https://localhost:7102/api/calculatorHistory", content);
        }

        private async void GetHistoryLastThreeEntries(byte operationType)
        {
            using var httpClient = new HttpClient();
            var typeString = operationType.ToString();
            var response = await httpClient.GetStringAsync($"https://localhost:7102/api/calculatorHistory/{operationType}");

            HistoryEntries.Clear();
            HistoryEntries = JsonConvert.DeserializeObject<List<HistoryEntry>>(response);

            HistoryDataGrid.ItemsSource = HistoryEntries ?? new List<HistoryEntry>();
            HistoryDataGrid.Items.Refresh();
        }

        private async void GetHistoryStats(byte operationType)
        {
            using var httpClient = new HttpClient();
            var typeString = operationType.ToString();
            var response = await httpClient.GetStringAsync($"https://localhost:7102/api/calculatorHistory/stats/{operationType}");

            HistoryStats.Clear();
            var result = JsonConvert.DeserializeObject<HistoryStats>(response) ?? new HistoryStats();
            HistoryStats.Add(result);

            HistoryStatsDataGrid.ItemsSource = HistoryStats ?? new List<HistoryStats>();
            HistoryStatsDataGrid.Items.Refresh();
        }
    }
}