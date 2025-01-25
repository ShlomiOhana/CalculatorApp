using Microsoft.AspNetCore.Mvc;
using ServicesApp.Enums;
using ServicesApp.Models;
using ServicesApp.Services.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServicesApp.Controllers
{
    [Route("api/calculatorHistory")]
    [ApiController]
    public class CalculatorHistoryController : ControllerBase
    {

        private ICalculatorHistoryService _calculatorHistoryService;
        public CalculatorHistoryController(ICalculatorHistoryService calculatorHistoryService)
        {
            _calculatorHistoryService = calculatorHistoryService;
        }

        [HttpGet]
        [Route("{type}")]
        public async Task<IActionResult> Get(int type)
        {
            var result = await _calculatorHistoryService.GetHistory((OperationType)type);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("stats/{type}")]
        public async Task<IActionResult> GetStats(int type)
        {
            var result = await _calculatorHistoryService.GetHistoryStats((OperationType)type);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<bool> Post([FromBody] HistoryEntry entry)
        {
            return await _calculatorHistoryService.Insert(entry) > 0;
        }
    }
}
