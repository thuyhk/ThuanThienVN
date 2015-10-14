using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using CRM.Service;
using CRM.Repository.Repositories;


namespace CRM.Service
{
    /// <summary>
    /// CountryService
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 07/07/2014		thuyhk@cateno.no                Created.        
    /// </summary>
    /// 

    public class TenantCountryService: Service<crm_Tenant_Countries>, ITenantCountryService
    {
        private readonly IRepositoryAsync<crm_Tenant_Countries> _repository;

        public TenantCountryService(IRepositoryAsync<crm_Tenant_Countries> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<crm_Tenant_Countries> GetAllCountries()
        {
            return _repository.GetAllCountries();
        }

        public crm_Tenant_Countries GetCountryById(int countryId)
        {
            return _repository.GetCountryById(countryId);
        }
    }
}
