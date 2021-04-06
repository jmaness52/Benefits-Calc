using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benefits.UI.Models
{
    public class BeneficiaryResponseModel : IComparable<BeneficiaryResponseModel>
    {
        public string name { get; set; }

        public string type { get; set; }

        public decimal yearCost { get; set; }

        public decimal periodCost { get; set; }

        public int CompareTo(BeneficiaryResponseModel compareModel)
        {
            if (compareModel.type.Equals(AppConstants.Employee))
            {
                return 1;
            }

            if (this.type.Equals(AppConstants.Employee))
            {
                return -1;
            }

            return this.name.CompareTo(compareModel.name);
        }
    }
}
