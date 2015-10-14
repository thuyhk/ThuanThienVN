using System;
using CRM.Entities.Models;
using CRM.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Http;
namespace CRM.Service
{
    /// <summary>
    /// ICountryService
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 07/07/2014		thuyhk@cateno.no                Created.        
    /// </summary>
    /// 

    public interface ICountryService: IService<crm_Countries> 
    {
        List<crm_Countries> GetAllCountries();

        crm_Countries GetCountryById(int countryId);
    }
}
