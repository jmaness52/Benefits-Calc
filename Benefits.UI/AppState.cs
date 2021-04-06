using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benefits.UI.Models;

namespace Benefits.UI
{
    /// <summary>
    /// A singleton to share result between pages
    /// </summary>
    public class AppState
    { 
        public List<BeneficiaryResponseModel> Result { get; set; } = new List<BeneficiaryResponseModel>();
    }
}
