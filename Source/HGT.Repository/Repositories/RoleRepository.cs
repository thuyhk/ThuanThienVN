using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using Repository.Pattern.Repositories;

namespace HGT.Repository.Repositories
{
    public static class RoleRepository
    {
        public static IEnumerable<Role> GetByPermissionEqual(this IRepository<Role> repository, int permisstion, int permissionUser)
        {
            var role = from k in repository.Queryable()
                        where k.Active && k.MaskPermission <= permisstion && k.MaskPermission > permissionUser
                        select k;
            return role;
        }
    }
}
