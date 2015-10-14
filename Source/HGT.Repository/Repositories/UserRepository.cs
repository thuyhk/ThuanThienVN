using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using HGT.Core.Models;

namespace HGT.Repository.Repositories
{
    public static class UserRepository
    {
        public static IEnumerable<UserModel> GetListUser(this IRepository<User> repository, bool isAdmin, int permisstionLogin, string userNameLogin, string keyword)
        {
            var role = repository.GetRepository<Role>().Queryable();

            var query = (from u in repository.Queryable()
                         join r in role on u.RoleId equals r.RoleId
                         where u.IsAdmin.Equals(isAdmin) && r.MaskPermission <= permisstionLogin
                         select new UserModel
                         {
                             UserId = u.UserId,
                             UserName = u.UserName,
                             FullName = u.FullName,
                             Email = u.Email,
                             Active = u.Active,
                             RoleName = r.RoleName,
                             ShowDelete = isAdmin == false ? true : (r.MaskPermission < permisstionLogin),
                             showUpdate = isAdmin == false ? true : ((r.MaskPermission < permisstionLogin) || (r.MaskPermission.Equals(permisstionLogin) && u.UserName.Equals(userNameLogin)))
                         });

            if (!String.IsNullOrEmpty(keyword))
                query = query.Where(p => p.FullName.Contains(keyword));

            return query.ToList();
        }

        public static IEnumerable<UserModel> GetSearchKey(this IRepository<User> repository, bool isAdmin, int permisstionLogin, string keySearch)
        {
            var role = repository.GetRepository<Role>().Queryable();

            var query = (from u in repository.Queryable()
                         join r in role on u.RoleId equals r.RoleId
                         where u.IsAdmin.Equals(isAdmin) && r.MaskPermission <= permisstionLogin && u.FullName.Contains(keySearch)
                         select new UserModel
                         {
                             UserId = u.UserId,
                             UserName = u.UserName,
                             FullName = u.FullName,
                             Email = u.Email,
                             Active = u.Active
                         }).ToList();


            return query;
        }


        public static User GetUserByEmail(this IRepository<User> repository, string email)
        {
            var _entity = from k in repository.Queryable()
                          where k.Email == email
                          select k;
            return _entity.FirstOrDefault();
        }

        public static User GetUserByToken(this IRepository<User> repository, string token)
        {
            var _entity = from k in repository.Queryable()
                          where k.TokenForgotPassWord.Equals(token)
                          select k;
            return _entity.FirstOrDefault();
        }

        public static User GetUserByUsername(this IRepository<User> repository, string username)
        {
            var _entity = from k in repository.Queryable()
                          where k.UserName == username
                          select k;
            return _entity.FirstOrDefault();
        }

        //public static IEnumerable<User> GetUserByIsAdmin(this IRepository<User> repository, bool IsAdmin)
        //{
        //    var users = from k in repository.Queryable()
        //                where k.IsAdmin == IsAdmin
        //                select k;
        //    return users;
        //}

        public static bool CheckUserChangePasswordByToken(this IRepository<User> repository, string email, string token = "")
        {
            var result = (from u in repository.Queryable() where u.Email.Equals(email) && u.TokenForgotPassWord.Equals(token) select u).FirstOrDefault();
            if (result == null) return true;
            return false;
        }

        public static bool CheckPermissionList(this IRepository<User> repository, int userId, int permisstionLogin)
        {
            var role = repository.GetRepository<Role>().Queryable();

            var query = (from u in repository.Queryable()
                         join r in role on u.RoleId equals r.RoleId
                         where u.UserId.Equals(userId) && r.MaskPermission <= permisstionLogin
                         select new
                         {
                             MaskPermission = r.MaskPermission
                         }).ToList();

            if (query.Count == 0) return false;
            return true;
        }

        public static bool CheckPermissionUpdate(this IRepository<User> repository, int userIdUpdate, int userIdLogin, int permisstionLogin)
        {
            var role = repository.GetRepository<Role>().Queryable();

            var query = (from u in repository.Queryable()
                         join r in role on u.RoleId equals r.RoleId
                         where ((r.MaskPermission < permisstionLogin) || (r.MaskPermission.Equals(permisstionLogin) && u.UserId.Equals(userIdUpdate)))
                         select new
                         {
                             UserID = u.UserId,
                             MaskPermission = r.MaskPermission
                         }).ToList();

            if (query.Count == 0) return false;
            ///////////////
            var queryDto = query.FirstOrDefault();
            if (permisstionLogin.Equals(queryDto.MaskPermission))
            {
                return (queryDto.UserID.Equals(userIdLogin));
            }
            return true;
        }

        public static bool CheckPermissionDelete(this IRepository<User> repository, int userId, int permisstionLogin)
        {
            var role = repository.GetRepository<Role>().Queryable();

            var query = (from u in repository.Queryable()
                         join r in role on u.RoleId equals r.RoleId
                         where u.UserId.Equals(userId) && r.MaskPermission < permisstionLogin
                         select new
                         {
                             MaskPermission = r.MaskPermission
                         }).ToList();

            if (query.Count == 0) return false;
            return true;
        }

        //check email duplicated
        public static bool CheckDuplicatedEmail(this IRepository<User> repository, string email, int userID)
        {
            var result = (from u in repository.Queryable() where u.Email.Equals(email) select u).FirstOrDefault();
            if (result == null) return true;
            if (result.UserId.Equals(userID)) return true;
            return false;

        }
        //check uesrName duplicated
        public static bool CheckDuplicatedUserName(this IRepository<User> repository, string userName, int userID)
        {
            var result = (from u in repository.Queryable() where u.UserName.Equals(userName) select u).FirstOrDefault();
            if (result == null) return true;
            if (result.UserId.Equals(userID)) return true;
            return false;

        }
    }
}
