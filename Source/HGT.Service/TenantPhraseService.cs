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
    public class TenantPhraseService : Service<crm_Tenant_Phrases>, CRM.Service.ITenantPhraseService
    {
        private readonly IRepositoryAsync<crm_Tenant_Phrases> _repository;

        public TenantPhraseService(IRepositoryAsync<crm_Tenant_Phrases> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<TenantLanguageModel> GetLanguage(string languagecode)
        {
            return _repository.GetLanguage(languagecode);
        }

    }
}
