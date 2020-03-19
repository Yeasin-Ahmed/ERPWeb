using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
   public class DescriptionBusiness: IDescriptionBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly DescriptionRepository descriptionRepository; // table
        public DescriptionBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            descriptionRepository = new DescriptionRepository(this._productionDb);
        }

        public IEnumerable<Description> GetDescriptionByOrgId(long orgId)
        {
            return descriptionRepository.GetAll(des => des.OrganizationId == orgId).ToList();
        }
    }
}
