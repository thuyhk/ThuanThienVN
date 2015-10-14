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
    public class TenantPhraseLanguageService  : Service<crm_Tenant_PhraseLanguages>, CRM.Service.ITenantPhraseLanguageService
    {
        private readonly IRepositoryAsync<crm_Tenant_PhraseLanguages> _repository;

        public TenantPhraseLanguageService(IRepositoryAsync<crm_Tenant_PhraseLanguages> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<TenantPhraseLanguageModel> GetPhraseValue(string phrasename, string languagecode = "nb-NO")
        {
            return _repository.GetPhraseValue(phrasename, languagecode);
        }

        
    }
}
