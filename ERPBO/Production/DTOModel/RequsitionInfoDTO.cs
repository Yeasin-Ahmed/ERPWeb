﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
   public class RequsitionInfoDTO
    {
        public long ReqInfoId { get; set; }
        [StringLength(100)]
        public string ReqInfoCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long WarehouseId { get; set; }
        public long LineId { get; set; }
        public long DescriptionId { get; set; }

        //Navi

        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string LineNumber { get; set; }
        public string ModelName { get; set; }
        public int Qty { get; set; }

    }
}
