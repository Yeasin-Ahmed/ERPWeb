using ERPBO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Common
{
    public class Utility
    {
        public static IEnumerable<Dropdown> ListOfReqStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>() {
                new Dropdown{text=RequisitionStatus.Pending,value=RequisitionStatus.Pending },
                new Dropdown{text=RequisitionStatus.Recheck,value=RequisitionStatus.Recheck },
                new Dropdown{text=RequisitionStatus.Approved,value=RequisitionStatus.Approved },
                new Dropdown{text=RequisitionStatus.Accepted,value=RequisitionStatus.Accepted },
                new Dropdown{text=RequisitionStatus.Rejected,value=RequisitionStatus.Pending }
            };
            return dropdowns;
        }
    }
}
