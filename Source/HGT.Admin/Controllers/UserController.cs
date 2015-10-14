using HGT.Admin.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Providers;
using HGT.Service;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HGT.Admin.Extensions;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using System.ComponentModel;
using HGT.Entities.Models;
using System.Linq.Expressions;
using Newtonsoft.Json;
using HGT.Admin.Filters;
using HGT.Core.Utility;
using HGT.Core.Models;

namespace HGT.Admin.Controllers
{
    public class UserController : AdminController
    {
        // GET: /Login/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        private UserInfo _userinfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        private static LogoModel _logoModel = new LogoModel();

        public UserController(IUnitOfWork unitOfWork, IUserService userService, IRoleService roleService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._roleService = roleService;

            if (_logoModel == null)
                _logoModel = new LogoModel();
        }

        #region Search
        /*15/09/2014 rilk Update Search*/
        public ActionResult FilterUser()
        {
            string searchValue = Request.Params["filter[filters][0][value]"];
            if (!string.IsNullOrEmpty(searchValue))
            {
                var user = _userService.GetSearchKey(true, _userinfo.MaskPermission, searchValue).Take(50);
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        /*End 12/08/2014 */
        #endregion

        //
        // GET: /Login/
        public ActionResult Index()
        {
            //string userName = userinfo.UserName;
            //var model = new LoginModel();

            int permissionLogin = _userinfo.MaskPermission;
            bool permissionUser = _userService.CheckPermissionList(_userinfo.ID, permissionLogin);
            if (!permissionUser) return RedirectToAction("AccessDeny", "Role");

            return View(permissionLogin);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request, string keyword = null)
        {
            int total = 0;
            var query = _userService.GetListUser(true, _userinfo.MaskPermission, _userinfo.UserName, keyword);
            //var data = query.ToList().OrderBy(x => x.UserName).Skip(request.Page - 1).Take(request.PageSize).AsEnumerable();

            /*
             * Order sort User
             */
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("UserName", ListSortDirection.Descending);
            var asc = sortDescriptor.SortDirection.Equals(ListSortDirection.Ascending) ? true : false;
            Func<UserModel, object> selector = x => x.UserName;

            switch (sortDescriptor.Member)
            {
                case "FullName":
                    selector = x => x.FullName;
                    break;
                case "Email":
                    selector = x => x.Email;
                    break;
            }

            var data = query.Order(selector, asc).Skip(request.Page - 1).Take(request.PageSize).AsEnumerable();

            total = query.Count();
            ViewBag.total = total;
            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };
            return Json(result);
        }

        //
        // GET: /Login/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int permissionLogin = _userinfo.MaskPermission;
            bool permissionUser = _userService.CheckPermissionList(id, permissionLogin);
            if (!permissionUser) return RedirectToAction("AccessDeny", "Role");

            //
            var model = new UserModel();
            model = _userService.Find(id).ToModel();

            var role = _roleService.Find(model.RoleId);

            model.RoleName = role.RoleName;
            model.Image = Globals.ImagePath(ConstantKeys.AvatarFolder, model.Image);

            return Json(model);
        }

        //
        // GET: /Login/Create
        public ActionResult Create()
        {
            var permissionLogin = _userinfo.MaskPermission;
            var roleList = _roleService.GetByPermissionEqual(permissionLogin, (int)RoleType.NormalUser);

            var model = new UserModel();
            foreach (var c in roleList)
                model.RoleList.Add(new SelectListItem() { Text = c.RoleName, Value = c.RoleId.ToString() });
            
            return View(model);
        }

        //
        // POST: /Login/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    var roleID = model.RoleId;
                    string encryptPassword = "";
                    string passwordSalt = "";
                    passwordSalt = EncryptProvider.GenerateSalt();
                    encryptPassword = EncryptProvider.EncryptPassword(model.Password, passwordSalt);

                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    model.LastLogonDate = DateTime.Now;

                    model.PasswordSalt = passwordSalt;
                    model.Password = encryptPassword;
                    model.IsAdmin = true;

                    #region upload file image to server

                    try
                    {
                        if (_logoModel != null && !string.IsNullOrEmpty(_logoModel.FileName))
                        {
                            model.Image = _logoModel.FileName;
                            var srcFile = Path.Combine(Server.MapPath(Globals.TempImagePath(_logoModel.FileName)));
                            var desFile = Path.Combine(Server.MapPath(Globals.ImagePath(ConstantKeys.AvatarFolder, _logoModel.FileName)));

                            // delete image if exist
                            IOUtility.DeleteFile(desFile);

                            // get temp file
                            string tempFile = Server.MapPath(Globals.TempImagePath(_logoModel.FileName));

                            // get folder thumbs
                            string thumbFolder = Server.MapPath(Globals.ThumbPath(ConstantKeys.AvatarFolder, _logoModel.FileName));

                            // get folder image
                            string imageFolder = Server.MapPath(Globals.ImagePath(ConstantKeys.AvatarFolder, _logoModel.FileName));

                            // create thumb image
                            System.Drawing.Image img = ImageUtility.ScaleImage(tempFile, 208, 180, "#ffffff");
                            ImageUtility.SaveImage(thumbFolder, img);
                            img.Dispose();

                            // create image
                            img = ImageUtility.ScaleImage(tempFile, 200, 200, "#ffffff");
                            ImageUtility.SaveImage(imageFolder, img);
                            img.Dispose();

                            // delete temp file
                            IOUtility.DeleteFile(srcFile);

                            //// move full images
                            //IOUtility.MoveFile(srcFile, desFile);
                        }
                        else
                            model.Image = "default.jpg";
                        _logoModel = null;
                    }
                    catch (Exception ex)
                    {
                        model.Image = "default.jpg";
                        ex.ToString();
                    }

                    #endregion

                    var _userEntity = model.ToEntity();

                    _userEntity.ObjectState = ObjectState.Added;
                    _userService.Insert(_userEntity);
                    _unitOfWork.SaveChanges();


                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Login/Edit/5
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var permissionLogin = _userinfo.MaskPermission;
            bool permissionUser = _userService.CheckPermissionUpdate(id, _userinfo.ID, permissionLogin);

            if (!permissionUser) 
                return RedirectToAction("AccessDeny", "Role");

            var model = _userService.Find(id).ToModel();

            if (model == null)
            {
                return RedirectToAction("AccessDeny", "Role");
            }

            var roleList = _roleService.GetByPermissionEqual(permissionLogin, (int)RoleType.NormalUser);
            foreach (var c in roleList)
                model.RoleList.Add(new SelectListItem() { Text = c.RoleName, Value = c.RoleId.ToString() });

            return View(model);
        }

        //
        // POST: /Login/Edit/5
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                var entity = _userService.Find(model.UserId);
                if (entity != null)
                {
                    //var _userUpdate = userModel.ToEntity();
                    entity.ModifiedDate = DateTime.Now;

                    if (entity.FullName != model.FullName)
                        entity.FullName = model.FullName;

                    if (entity.Email != model.Email)
                        entity.Email = model.Email;

                    if (entity.Phone != model.Phone)
                        entity.Phone = model.Phone;

                    if (entity.Mobile != model.Mobile)
                        entity.Mobile = model.Mobile;

                    if (entity.RoleId != model.RoleId)
                        entity.RoleId = model.RoleId;

                    #region upload image

                    if (_logoModel != null && !string.IsNullOrEmpty(_logoModel.FileName))
                    {
                        //delete file old
                        if (entity.Image != "default.jpg")
                        {
                            var pathOld = Globals.ImagePath(entity.Image);
                            IOUtility.DeleteFile(pathOld);
                        }

                        entity.Image = _logoModel.FileName;
                        var srcFile = Path.Combine(Server.MapPath(Globals.TempImagePath(_logoModel.FileName)));
                        var desFile = Path.Combine(Server.MapPath(Globals.ImagePath(ConstantKeys.AvatarFolder, _logoModel.FileName)));

                        // delete image if exist
                        IOUtility.DeleteFile(desFile);

                        // get temp file
                        string tempFile = Server.MapPath(Globals.TempImagePath(_logoModel.FileName));

                        // get folder thumbs
                        string thumbFolder = Server.MapPath(Globals.ThumbPath(ConstantKeys.AvatarFolder, _logoModel.FileName));

                        // get folder image
                        string imageFolder = Server.MapPath(Globals.ImagePath(ConstantKeys.AvatarFolder, _logoModel.FileName));

                        // create thumb image
                        System.Drawing.Image img = ImageUtility.ScaleImage(tempFile, 208, 180, "#ffffff");
                        ImageUtility.SaveImage(thumbFolder, img);
                        img.Dispose();

                        // create image
                        img = ImageUtility.ScaleImage(tempFile, 200, 200, "#ffffff");
                        ImageUtility.SaveImage(imageFolder, img);
                        img.Dispose();

                        // delete temp file
                        IOUtility.DeleteFile(srcFile);

                        //// move full images
                        //IOUtility.MoveFile(srcFile, desFile);                       
                    }
                    _logoModel = null;

                    #endregion

                    entity.ObjectState = ObjectState.Modified;
                    entity.Active = model.Active;

                    _userService.Update(entity);
                    _unitOfWork.SaveChanges();

                    // TODO: Add update logic here
                }
                //return RedirectToAction("Index");
                return Json(new { Status = 1, Message = "Update success." });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Update error." });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //check role follow view
                var permissionLogin = _userinfo.MaskPermission;

                var permissionUser = _userService.CheckPermissionDelete(id, permissionLogin);

                if (!permissionUser) return RedirectToAction("AccessDeny", "Role");

                var User = _userService.Find(id);

                //delete file on server
                var physicalPath = User.Image;

                if (System.IO.File.Exists(Path.Combine(Server.MapPath(physicalPath))))
                    System.IO.File.Delete(Path.Combine(Server.MapPath(physicalPath)));

                //delete data
                _userService.Delete(User);
                _unitOfWork.SaveChanges();

                return Json(new { Status = 1, Message = "Delete success!" });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Delete error!" });
            }
        }

        // GET: /Tenant/changePass/5
        [HttpPost, ActionName("ChangePass")]
        public ActionResult ChangePass(int id, string user, string passOld, string passNew)
        {
            try
            {

                var userLoginDto = _userService.Find(_userinfo.ID);
                bool login = EncryptProvider.EncryptPassword(passOld, userLoginDto.PasswordSalt).Equals(userLoginDto.Password);

                if (login)
                {
                    var userDto = _userService.Find(id);

                    string encryptPassword = "";
                    string passwordSalt = "";
                    passwordSalt = EncryptProvider.GenerateSalt();
                    encryptPassword = EncryptProvider.EncryptPassword(passNew, passwordSalt);

                    userDto.PasswordSalt = passwordSalt;
                    userDto.Password = encryptPassword;

                    userDto.ObjectState = ObjectState.Modified;
                    _userService.Update(userDto);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    return Json(new { Status = 0, Message = "Password old not correct!" });
                }

                return Json(new { Status = 1, Message = "Change password success!" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = "Change password error!" });
            }
        }


        //upload file : auto save avatar
        public ActionResult SaveLogoUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (_logoModel != null)
                {
                    DeleteTempFile(_logoModel.FileName);
                    _logoModel = null;
                }

                // Upload file in to UploadFolder
                var fileName = ImageUtility.RandomString() + Path.GetFileName(file.FileName);
                var pathFile = Globals.TempImagePath(fileName);
                var physhicalPath = Path.Combine(Server.MapPath(pathFile));
                file.SaveAs(physhicalPath);

                _logoModel = new LogoModel() { FileName = fileName, PictureUrl = pathFile };
                file.SaveAs(physhicalPath);

            }
            return Json(_logoModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveLogoUpload()
        {
            DeleteTempFile(_logoModel.FileName);
            _logoModel = null;
            return Json("");
        }

        private void DeleteTempFile(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var physicalPath = Path.Combine(Server.MapPath(Globals.TempImagePath(filename)));
                IOUtility.DeleteFile(physicalPath);
            }
        }

        //check user name Exitst
        [HttpPost]
        public JsonResult DoesUserNameExist(string userName, int userId)
        {
            var user = _userService.CheckDuplicatedUserName(userName, userId);
            return Json(user);
        }

        //check user email Exitst
        [HttpPost]
        public JsonResult DoesUserEmailExist(string email, int userId)
        {
            var user = _userService.CheckDuplicatedEmail(email, userId);
            return Json(user);

        }
    }
}
