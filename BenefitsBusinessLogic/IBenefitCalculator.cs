using Benefits.BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benefits.BusinessLogic
{
    public interface IBenefitCalculator
    {
        Task<bool> ValidateRequest(List<BeneficiaryRequestModel> request);
        Task<List<BeneficiaryResponseModel>> CalculateBenefits(List<BeneficiaryRequestModel> request);
    }
}