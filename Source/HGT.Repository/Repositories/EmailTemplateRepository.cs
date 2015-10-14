using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using Repository.Pattern.Repositories;

namespace HGT.Repository.Repositories
{
    public static class EmailTemplateRepository
    {
        public static EmailTemplate GetEmailByID(this IRepository<EmailTemplate> repository, int emailTemplateID)
        {
            var emailTemplate = from k in repository.Queryable()
                                where k.Deleteable && k.EmailTemplateId.Equals(emailTemplateID)
                                select k;
            return emailTemplate.FirstOrDefault();
        }
    }
}
