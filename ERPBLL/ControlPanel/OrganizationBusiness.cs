﻿using ERPBLL.ControlPanel.Interface;
using ERPBO.ControlPanel.DomainModels;
using ERPBO.ControlPanel.DTOModels;
using ERPDAL.ControlPanelDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.ControlPanel
{
    public class OrganizationBusiness:IOrganizationBusiness // Business Class //
    {
        private readonly IControlPanelUnitOfWork _controlPanelUnitOfWork; // database
        private readonly OrganizationRepository _organizationRepository; // table

        public OrganizationBusiness(IControlPanelUnitOfWork controlPanelUnitOfWork)
        {
            this._controlPanelUnitOfWork = controlPanelUnitOfWork;
            _organizationRepository = new OrganizationRepository(this._controlPanelUnitOfWork);
        }

        public IEnumerable<Organization> GetAllOrganizations()
        {
            return _organizationRepository.GetAll();
        }

        public Organization GetOrganizationById(long id)
        {
            return _organizationRepository.GetAll(org=> org.OrgId == id).FirstOrDefault();
        }

        public bool SaveOrganization(OrganizationDTO org, long userId)
        {
            if (org.OrgId <= 0)
            {
                Organization organization = new Organization()
                {
                    OrganizationName = org.OrganizationName,
                    ShortName =org.ShortName,
                    Address = org.Address,
                    PhoneNumber= org.PhoneNumber,
                    MobileNumber = org.MobileNumber,
                    Website = org.Website,
                    Email = org.Email,
                    Fax  =org.Fax,
                    OrgLogoPath = org.OrgLogoPath,
                    ReportLogoPath = org.ReportLogoPath,
                    IsActive = org.IsActive,
                    EUserId = userId,
                    EntryDate = DateTime.Now
                };
                _organizationRepository.Insert(organization);
            }
            else
            {
                var orgInDb = GetOrganizationById(org.OrgId);
                if(orgInDb != null)
                {
                    orgInDb.ShortName = org.ShortName;
                    orgInDb.Address = org.Address;
                    orgInDb.PhoneNumber = org.PhoneNumber;
                    orgInDb.MobileNumber = org.MobileNumber;
                    orgInDb.Website = org.Website;
                    orgInDb.Email = org.Email;
                    orgInDb.Fax = org.Fax;
                    orgInDb.OrgLogoPath = org.OrgLogoPath;
                    orgInDb.ReportLogoPath = org.ReportLogoPath;
                    orgInDb.IsActive = org.IsActive;
                    orgInDb.UpUserId = userId;
                    orgInDb.UpdateDate = DateTime.Now;
                    _organizationRepository.Update(orgInDb);
                }
            }
            return _organizationRepository.Save();
        }
    }
}
