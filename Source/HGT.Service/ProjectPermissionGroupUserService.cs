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
    /// <summary>
    /// LogService
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 10/07/2014		thuyhk@cateno.no                Created.        
    /// </summary>
    /// 

    public class ProjectPermissionGroupUserService : Service<crm_Project_Permissions_GroupUsers>, IProjectPermissionGroupUserService
    {
        private readonly IRepositoryAsync<crm_Project_Permissions_GroupUsers> _repository;

        public ProjectPermissionGroupUserService(IRepositoryAsync<crm_Project_Permissions_GroupUsers> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}
