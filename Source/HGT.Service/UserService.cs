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
using HGT.Core.Models;


namespace HGT.Service
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IRepositoryAsync<User> _repository;

        public UserService(IRepositoryAsync<User> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserModel> GetListUser(bool isAdmin, int permisstionLogin, string userNameLogin, string keyword)
        {
            return _repository.GetListUser(isAdmin, permisstionLogin, userNameLogin, keyword);
        }
        public IEnumerable<UserModel> GetSearchKey(bool isAdmin, int permisstionLogin, string keySearch)
        {
            return _repository.GetSearchKey(isAdmin, permisstionLogin, keySearch);
        }
        public User GetUserByEmail(string email)
        {
            return _repository.GetUserByEmail(email);
        }
        public User GetUserByUsername(string username)
        {
            return _repository.GetUserByUsername(username);
        }
        public User GetUserByToken(string token)
        {
            return _repository.GetUserByToken(token);
        }
        //public IEnumerable<User> GetUserByIsAdmin(bool IsAdmin)
        //{
        //    return _repository.GetUserByIsAdmin(IsAdmin);
        //}
        public bool CheckUserChangePasswordByToken(string email, string token)
        {
            return _repository.CheckUserChangePasswordByToken(email, token);
        }
        public bool CheckPermissionList(int userId, int permisstionLogin)
        {
            return _repository.CheckPermissionList(userId, permisstionLogin);
        }
        public bool CheckPermissionUpdate(int userIdUpdate, int userIdLogin, int permisstionLogin)
        {
            return _repository.CheckPermissionUpdate(userIdUpdate, userIdLogin, permisstionLogin);
        }
        public bool CheckPermissionDelete(int userId, int permisstionLogin)
        {
            return _repository.CheckPermissionDelete(userId, permisstionLogin);
        }
        public bool CheckDuplicatedEmail(string email, int userID)
        {
            return _repository.CheckDuplicatedEmail(email, userID);
        }

        public bool CheckDuplicatedUserName(string userName, int userID)
        {
            return _repository.CheckDuplicatedUserName(userName, userID);
        }

    }
}
