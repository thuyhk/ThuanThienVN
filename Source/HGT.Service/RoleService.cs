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
    public class RoleService : Service<Role>, IRoleService
    {
        private readonly IRepositoryAsync<Role> _repository;

        public RoleService(IRepositoryAsync<Role> repository)
            : base(repository)
        {
            _repository = repository;
        }
        public IEnumerable<Role> GetByPermissionEqual(int permisstion, int permissionUser)
        {
            return _repository.GetByPermissionEqual(permisstion, permissionUser);

        }
    }
}
