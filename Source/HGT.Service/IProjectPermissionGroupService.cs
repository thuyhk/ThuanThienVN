using System;
using CRM.Entities.Models;
using CRM.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Http;
namespace CRM.Service
{
    /// <summary>
    /// ILogService
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 10/07/2014		thuyhk@cateno.no                Created.        
    /// </summary>
    /// 
    public interface IProjectPermissionGroupService : IService<crm_Project_Permissions_Groups>
    {
        
    }
}
