using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using HGT.Service;
using HGT.Repository.Repositories;


namespace HGT.Service
{
    public class EmailTemplateService : Service<EmailTemplate>, IEmailTemplateService
    {
        private readonly IRepositoryAsync<EmailTemplate> _repository;

        public EmailTemplateService(IRepositoryAsync<EmailTemplate> repository)
            : base(repository)
        {
            _repository = repository;
        }
        public EmailTemplate GetEmailByID(int emailTemplateID)
        {
            return _repository.GetEmailByID(emailTemplateID);

        }
    }
}
