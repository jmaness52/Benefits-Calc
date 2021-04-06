using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Benefits.BusinessLogic;
using Benefits.BusinessLogic.Models;
using Microsoft.AspNetCore.Cors;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Benefits.API.Controllers
{
    [ApiController]
    public class BenefitsController : ControllerBase
    {
        IBenefitCalculator _benefitCalculator;
        public BenefitsController(IBenefitCalculator benefitCalculator)
        {
            _benefitCalculator = benefitCalculator;
        }
        
        [Route("api/benefits/calculate")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CalculateBenefits(List<BeneficiaryRequestModel> request)
        {
            bool valid = await _benefitCalculator.ValidateRequest(request);

            if (!valid)
                return Problem("Bad Input Data!");

            List<BeneficiaryResponseModel> response = await _benefitCalculator.CalculateBenefits(request);
            return new JsonResult(response);
        }
    }
}
