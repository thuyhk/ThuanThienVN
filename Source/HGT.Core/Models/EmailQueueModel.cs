using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core.Models
{
    public class EmailQueueModel : BaseModel
    {
        public bool Active { get; set; }
        public string EmailSubject { get; set; }
        public string DisplayNameFrom { get; set; }
        public string EmailFrom { get; set; }
        public string DisplayNameTo { get; set; }
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }
        public string EmailBody { get; set; }
        public int DepartmentId { get; set; }
        public string EmailContact { get; set; }
        public string PhoneContact { get; set; }
        public string CompanyName { get; set; }
        public string Note { get; set; }
        public string SenderIP { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
