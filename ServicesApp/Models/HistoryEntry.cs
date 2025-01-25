namespace ServicesApp.Models
{
    public class HistoryEntry
    {
        public DateTime Date { get; set; }
        public double Field1 { get; set; }
        public double Field2 { get; set; }
        public byte Operation { get; set; }
        public double Result { get; set; }
    }
}
