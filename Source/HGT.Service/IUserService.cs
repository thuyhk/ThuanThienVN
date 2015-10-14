using System;
using HGT.Entities.Models;
using HGT.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
using HGT.Core.Models;
namespace HGT.Service
{
    public interface IUserService : IService<User>
    {
        IEnumerable<UserModel> GetListUser(bool isAdmin, int permisstionLogin, string userNameLogin, string keyword);
        IEnumerable<UserModel> GetSearchKey(bool isAdmin, int permisstionLogin, string keySearch);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        User GetUserByToken(string token);
        //IEnumerable<User> GetUserByIsAdmin(bool IsAdmin);
        bool CheckUserChangePasswordByToken(string email, string token);
        bool CheckPermissionList(int userId, int permisstionLogin);
        bool CheckPermissionUpdate(int userIdUpdate, int userIdLogin, int permisstionLogin);
        bool CheckPermissionDelete(int userId, int permisstionLogin);
        bool CheckDuplicatedEmail(string email, int userID);
        bool CheckDuplicatedUserName(string userName, int userID);
    }
}
