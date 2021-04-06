using System.Threading.Tasks;

namespace Benefits.DataAccess
{
    public interface IDataAccessor
    {
        Task<bool> BeneficiaryTypeExists(string beneficiaryType);
        Task<decimal> GetBenefitCostsAsync(string beneficiaryType);
        Task<decimal> GetDiscountAsync();
        Task<int> GetPayPeriodsAsync();
    }
}