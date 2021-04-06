using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benefits.BusinessLogic.Models;
using Benefits.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Benefits.BusinessLogic
{
    public class BenefitCalculator : IBenefitCalculator
    {

        public IDiscountHandler DiscountHandler { get; set; }
        public IDataAccessor DataAccess { get; set; }

        public BenefitCalculator()
        {
            //Would use DI in production
            DiscountHandler = new DiscountHandler();
            DataAccess = new DataAccessor();
        }

        public async Task<bool> ValidateRequest(List<BeneficiaryRequestModel> request)
        {
            bool isValid = true;

            foreach (BeneficiaryRequestModel b in request)
            {
                bool typeExists = await DataAccess.BeneficiaryTypeExists(b.Type);
                if (!typeExists || string.IsNullOrWhiteSpace(b.Name))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
        public async Task<List<BeneficiaryResponseModel>> CalculateBenefits(List<BeneficiaryRequestModel> request)
        {
            List<BeneficiaryResponseModel> response = new List<BeneficiaryResponseModel>();
            var tempBag = new ConcurrentBag<BeneficiaryResponseModel>();

            //Get constants for calulations - pay periods per year and discount percentage (fraction)
            decimal discount = await DataAccess.GetDiscountAsync();
            decimal payPeriods = await DataAccess.GetPayPeriodsAsync();

            //loop through the beneficiaries in the request and calculate costs for each one.
            //Adding simple parellism makes this a bit faster than a standard foreach - avg 1200ms vs 1600ms avg on a foreach
            IEnumerable<Task> tasks = request.Select(async b =>
            {
                BeneficiaryResponseModel beneficiary = new BeneficiaryResponseModel();

                //Trim the name to prevent issue with discount eligibility if name contains leading spaces
                beneficiary.Name = b.Name.Trim();
                beneficiary.Type = b.Type;
                decimal yearlyCost = await DataAccess.GetBenefitCostsAsync(b.Type);

                //Give the customer a discount if their name is eligible
                if (DiscountHandler.EligibleForDiscount(beneficiary.Name))
                    yearlyCost *= (1 - discount);

                beneficiary.YearCost = yearlyCost;
                beneficiary.PeriodCost = decimal.Round((yearlyCost / payPeriods), 2);

                tempBag.Add(beneficiary);
            });

            await Task.WhenAll(tasks);

            //Convert the temporary concurrent bag to a list and store it in the response
            return tempBag.ToList();
        }

       
    }
}
