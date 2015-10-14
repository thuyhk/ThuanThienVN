using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using System.Net;
using System.Web;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using System.ComponentModel;
using System.Linq.Expressions;
using System.IO;
using AutoMapper;
using System.Data.Entity;
using Repository.Pattern.DataContext;
using System.Security.AccessControl;
using System.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Linq;
using HGT.Admin.Filters;
using HGT.Service;
using HGT.Admin.Models;
using System.Runtime.CompilerServices;
using HGT.Core;
using HGT.Repository.Models;
using HGT.Admin.Extensions;
using HGT.Entities.Models;

namespace HGT.Admin.Controllers
{
    /// <summary>
    /// Setting controller
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 01/08/2014		ri.khanh@cateno.no                Updated        
    /// 01/08/2014		ri.khanh@cateno.no                Updated (upload file into folder Setting) 
    /// </summary>
    /// 

    [AuthFilter]
    public class EmailTemplateController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private static LogoModel _logoModel = new LogoModel();
        private readonly IEmailTemplateService _emailTemplateService;

        private  UserInfo _userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;

        #endregion

        #region Constructors

        public EmailTemplateController(IUnitOfWork unitOfWork, IEmailTemplateService emailTemplateService)
        {
            this._unitOfWork = unitOfWork;
            this._emailTemplateService = emailTemplateService;
        }

        #endregion

        #region Utilities

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            int _total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("Name", ListSortDirection.Descending);

            sortDescriptor.Member = sortDescriptor.Member ?? "Name";
            Func<IQueryable<EmailTemplate>, IOrderedQueryable<EmailTemplate>> order;
            var data = new List<EmailTemplate>();
            switch (sortDescriptor.Member)
            {
                default:
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.Name);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.Name);
                    }
                    break;
            }

            data = _emailTemplateService.Select(null, order, null, request.Page, request.PageSize).ToList();

            _total = _emailTemplateService.Select(null, order, null, null, null).Count();
            ViewBag.Total = _total;
            var result = new DataSourceResult()
            {
                Data = data,
                Total = _total
            };
            return Json(result);
        }

        #region edit email

        // GET: /setting/Edit/5
        public ActionResult Edit(int? id = 0)
        {
            //check role
            if (!(_userInfo.IsSA) || id == 0)
            {
                return RedirectToAction("AccessDeny", "Role");
            }
            //////////////
            var model = new EmailTemplateModel();
            var emailTemplate = _emailTemplateService.Find(id);

            model = emailTemplate.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Edit(EmailTemplateModel emailTemplateModel)
        {
            if (ModelState.IsValid)
            {
                if (_userInfo.IsSA)
                {
                    var emailTemplate = _emailTemplateService.Find(emailTemplateModel.EmailTemplateId);
                    if (emailTemplate != null)
                    {
                        emailTemplate.LastModified = DateTime.Now;

                        if (emailTemplate.Name != emailTemplateModel.Name)
                            emailTemplate.Name = emailTemplateModel.Name;

                        if (emailTemplate.Subject != emailTemplateModel.Subject)
                            emailTemplate.Subject = emailTemplateModel.Subject;

                        if (emailTemplate.Content != emailTemplateModel.Content)
                            emailTemplate.Content = emailTemplateModel.Content;

                        _emailTemplateService.Update(emailTemplate);
                        _unitOfWork.SaveChanges();

                    }
                }
                return JsonConvert.SerializeObject(new { Status = 1, Message = "Update success." });
            }
            return null;

        }
        #endregion

        #endregion
    }
}