using System;
using System.Collections.Generic;
using System.Text;

namespace Benefits.BusinessLogic
{
    public class DiscountHandler : IDiscountHandler
    {
        /// <summary>
        /// Determines if the customer is eligible for a Discount. Per the requirements, the customer 
        /// is eligible for a discount if their name starts with "A".  For simplicity, I am just considering first names, case-insensitive
        /// </summary>
        public bool EligibleForDiscount(string name)
        {
            return name.Trim().ToLower().StartsWith("a");
        }
    }
}
