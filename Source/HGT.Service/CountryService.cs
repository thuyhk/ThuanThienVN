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

    public class CountryService: Service<crm_Countries>, ICountryService
    {
        private readonly IRepositoryAsync<crm_Countries> _repository;

        public CountryService(IRepositoryAsync<crm_Countries> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<crm_Countries> GetAllCountries()
        {
            return _repository.GetAllCountries();
        }

        public crm_Countries GetCountryById(int countryId)
        {
            return _repository.GetCountryById(countryId);
        }
    }
}
