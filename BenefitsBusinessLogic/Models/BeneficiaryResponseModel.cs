using System;
using System.Collections.Generic;
using System.Text;

namespace Benefits.BusinessLogic.Models
{
    public class BeneficiaryResponseModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public decimal YearCost { get; set; }

        public decimal PeriodCost { get; set; }
    }
}
