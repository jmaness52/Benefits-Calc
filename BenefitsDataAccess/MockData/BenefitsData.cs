using System;
using System.Collections.Generic;
using System.Text;


namespace Benefits.DataAccess
{
    
    /// <summary>
    /// A singleton class to hold our Benefits Data. In production this data
    /// would be stored in a Database.
    /// </summary>
    public sealed class BenefitsData
    {

        #region Singleton Implementation
        //In production, would probably use DI instead of this pattern

        private static BenefitsData _instance = null;
        private static readonly object _lock = new object();

        private BenefitsData()
        {
        }

        public static BenefitsData Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BenefitsData();
                    }

                    return _instance;
                }
            }
        }

        #endregion

        private decimal _discount = 0.10M;
        private int _payPeriods = 26;

        public const string Employee = "Employee";
        public const string Dependent = "Dependent";

        //If the business needs change we could add more categories such as:
        //private const string DomesticPartner = "DomesticPartner";

        private Dictionary<string, decimal> _benefitsLookup = new Dictionary<string, decimal>()
        {
            {Employee, 1000M},
            {Dependent, 500M}
        };

        public decimal Discount
        {
            get { return _discount; }
        }

        public int PayPeriodsPerYear
        {
            get { return _payPeriods; }
        }

        /// <summary>
        /// A dictionary for looking up cost for a given beneficiary type
        /// </summary>
        public Dictionary<string, decimal> BenefitsLookup
        {
            get { return _benefitsLookup; }
        }

    }
}
