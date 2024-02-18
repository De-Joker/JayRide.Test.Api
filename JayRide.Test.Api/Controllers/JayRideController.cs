using JayRide.Test.Api.Core.Models;
using Microsoft.AspNetCore.Mvc;
using JayRide.Test.Api.Core.Services;
using System.Text.RegularExpressions;

namespace JayRide.Test.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JayRideController : ControllerBase
    {
        private readonly ILogger<JayRideController> _logger;
        private readonly IJayRideService _jayrideService;
        private readonly IConfiguration _configuration;
        public JayRideController(ILogger<JayRideController> logger, IJayRideService jayrideService, IConfiguration configuration)
        {
            _logger = logger;
            _jayrideService = jayrideService;
            _configuration = configuration;
        }

        [ProducesResponseType(typeof(CandidateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("candidate")]
        public ActionResult GetCandidate()
        {
            _logger.LogInformation("JayRideController > GetCandidate > Start");
            var result = _jayrideService.GetCandidate();
            _logger.LogInformation("JayRideController > GetCandidate > End > {@result}", Request);
            return Ok(result);
        }

        [ProducesResponseType(typeof(LocationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("Location/{ipAddress}")]
        public async Task<ActionResult> GetLocationAsync([FromRoute] string ipAddress)
        {
            _logger.LogInformation("JayRideController > GetLocationAsync > Start");
            if (Regex.IsMatch(ipAddress, _configuration.GetValue<string>("ipRegex")) == false)
            {
                _logger.LogError("JayRideController > GetLocationAsync > Error >  Invalid ip address entered > {@ipAddress}", ipAddress);
                return BadRequest("Invalid ip address entered");
            }

            var result = await _jayrideService.GetLocationAsync(ipAddress);
            _logger.LogInformation("JayRideController > GetLocationAsync > End > {@result}", result);
            return Ok(result);
        }

        [ProducesResponseType(typeof(List<ListingTotalsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("Listings/{numPassengers}")]
        public async Task<ActionResult> GetListingsAsync([FromRoute] int numPassengers)
        {
            _logger.LogInformation("JayRideController > GetListingsAsync > Start > {@numPassengers}", numPassengers);
            var result = await _jayrideService.GetListingsAsync(numPassengers);
            _logger.LogInformation("JayRideController > GetListingsAsync > End > {@result}", result);
            return Ok(result);
        }
    }
}