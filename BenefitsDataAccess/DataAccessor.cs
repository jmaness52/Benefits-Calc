using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benefits.DataAccess
{
    /// <summary>
    /// Retrieves data needed for benefits calculations. Normally this would call out to a Database.
    /// </summary>
    public class DataAccessor : IDataAccessor
    {
        /// <summary>
        /// Retrieves benefit cost for a given Beneficiary Type.
        /// Ensure type exists with <see cref="BeneficiaryTypeExists(string)"/> before calling this method
        /// </summary>
        public async Task<decimal> GetBenefitCostsAsync(string beneficiaryType)
        {

            BenefitsData.Instance.BenefitsLookup.TryGetValue(beneficiaryType, out decimal cost);

            //Simulate a delay with retrieving the data from a DB
            await Task.Delay(200);
            return cost;
        }

        /// <summary>
        /// Ensures Beneficiary type exists in our mock data.
        /// </summary>
        /// <returns>a bool indicating whether the type was found in the dictionary</returns>
        public async Task<bool> BeneficiaryTypeExists(string beneficiaryType)
        {
            await Task.Delay(200);
            return BenefitsData.Instance.BenefitsLookup.ContainsKey(beneficiaryType);
        }


        /// <summary>
        /// Retrieves the discount percentage as a fraction
        /// </summary>
        public async Task<decimal> GetDiscountAsync()
        {
            await Task.Delay(200);
            return BenefitsData.Instance.Discount;
        }

        /// <summary>
        /// Retrieves the number of pay periods in a year
        /// </summary>
        public async Task<int> GetPayPeriodsAsync()
        {
            await Task.Delay(200);
            return BenefitsData.Instance.PayPeriodsPerYear;
        }
    }
}
