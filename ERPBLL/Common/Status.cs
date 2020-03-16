using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Common
{
    public class RequisitionStatus
    {
        // Never change this fields values //
        public static readonly string Pending = "Pending";
        public static readonly string Rejected = "Rejected";
        public static readonly string Recheck = "Recheck";
        public static readonly string Approved = "Approved";
        public static readonly string Accepted = "Accepted";
    }

    public class StockStatus
    {
        public static readonly string StockIn = "Stock-In";
        public static readonly string StockOut = "Stock-Out";
        public static readonly string StockReturn = "Stock-Return";
    }
}
