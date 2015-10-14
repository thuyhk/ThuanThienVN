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
    public class Phrase : Service<crm_Phrases>, CRM.Service.IPhraseService
    {
        private readonly IRepositoryAsync<crm_Phrases> _repository;

        public Phrase(IRepositoryAsync<crm_Phrases> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<LanguageModel> GetLanguage(string languagecode)
        {
            return _repository.GetLanguage(languagecode);
        }
    }
}
