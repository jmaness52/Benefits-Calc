using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benefits.UI
{

    public class AppConstants
    {
        public const string PostURL = "https://localhost:5001/api/benefits/calculate";
        public const string Employee = "Employee";
        public const string Dependent = "Dependent";
        public const int MaxDependents = 10;
    }
}
