using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HGT.Repository.Repositories
{
    /// <summary>
    /// TicketRepository
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 01/08/2014		ri.khanh@cateno.no                Created.        
    /// </summary>
    /// 

    public static class SettingRepository
    {
        public static List<Setting> GetSettingAll(this IRepository<Setting> repository)
        {
            var _lst = from k in repository.Queryable()
                       select k;
            return _lst.ToList();
        }
    }
}
