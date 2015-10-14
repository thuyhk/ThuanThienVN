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
    public class PhraseLanguageService : Service<crm_PhraseLanguages>, CRM.Service.IPhraseLanguageService
    {
        private readonly IRepositoryAsync<crm_PhraseLanguages> _repository;

        public PhraseLanguageService(IRepositoryAsync<crm_PhraseLanguages> repository)
            : base(repository)
        {
            _repository = repository;
        }

    }
}
