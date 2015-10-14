using System;
using CRM.Entities.Models;
using CRM.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
namespace CRM.Service
{
    public interface ITenantPhraseService : IService<crm_Tenant_Phrases>
    {
        IEnumerable<TenantLanguageModel> GetLanguage(string languagecode);
    }
}
