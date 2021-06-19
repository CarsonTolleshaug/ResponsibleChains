using ChainLinks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FizzbuzzController : ControllerBase
    {
        private readonly IFizzBuzzChain _fizzBuzzChain;
        private readonly ILogger<FizzbuzzController> _logger;

        public FizzbuzzController(IFizzBuzzChain fizzBuzzChain, ILogger<FizzbuzzController> logger)
        {
            _fizzBuzzChain = fizzBuzzChain;
            _logger = logger;
        }

        [HttpGet]
        [Route("{input}")]
        public IActionResult Get([FromRoute] int input)
        {
            if (input < 0)
            {
                _logger.LogError($"Invalid input provided: {input}");
                return BadRequest("Please provide a positive integer as an argument.");
            }

            return Ok(new FizzbuzzResponse
            {
                Output = _fizzBuzzChain.Execute(input)
            });
        }
    }
}
