﻿using ERPBO.ControlPanel.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ControlPanelDAL
{
    public class OrganizationRepository : ControlPanelBaseRepository<Organization>
    {
        public OrganizationRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }

    public class BranchRepository : ControlPanelBaseRepository<Branch>
    {
        public BranchRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }

    public class AppUserRepository : ControlPanelBaseRepository<AppUser>
    {
        public AppUserRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }
    public class RoleRepository : ControlPanelBaseRepository<Role>
    {
        public RoleRepository(IControlPanelUnitOfWork controlPanelUnitOfWork) : base(controlPanelUnitOfWork) { }
    }

}
