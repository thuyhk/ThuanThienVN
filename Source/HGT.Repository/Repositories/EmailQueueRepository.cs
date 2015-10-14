using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using Repository.Pattern.Repositories;

namespace HGT.Repository.Repositories
{
    public static class EmailQueueRepository
    {
        public static IEnumerable<EmailQueue> GetByEmailFrom(this IRepository<EmailQueue> repository, string emailFrom)
        {
            var emailQueue = from k in repository.Queryable()
                             where k.EmailFrom.Equals(emailFrom)
                             select k;
            return emailQueue;
        }
    }
}
