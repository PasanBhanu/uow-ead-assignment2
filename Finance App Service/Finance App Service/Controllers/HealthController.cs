using Finance_App_Service.Data;
using Finance_App_Service.REST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Finance_App_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly ApplicationDbContext _applicationDbCotext;

        public HealthController(ILogger<HealthController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbCotext = applicationDbContext;
        }

        // GET: health
        [HttpGet]
        public BaseResponse Index()
        {
            BaseResponse response = new BaseResponse
            {
                Status = "success",
                Message = "Service available",
                Data = null
            };
            return response;
        }
    }
}
