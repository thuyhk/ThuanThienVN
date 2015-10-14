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
    public class EmailQueueService : Service<EmailQueue>, IEmailQueueService
    {
        private readonly IRepositoryAsync<EmailQueue> _repository;

        public EmailQueueService(IRepositoryAsync<EmailQueue> repository)
            : base(repository)
        {
            _repository = repository;
        }
        public IEnumerable<EmailQueue> GetByEmailFrom(string emailFrom)
        {
            return _repository.GetByEmailFrom(emailFrom);

        }
    }
}
