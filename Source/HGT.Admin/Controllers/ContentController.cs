using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using System.ComponentModel;
using System.Linq.Expressions;
using Newtonsoft.Json;
using HGT.Entities.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Providers;
using HGT.Core.Models;
using HGT.Core.Extensions;
using HGT.Admin.Models;
using HGT.Admin.Filters;
using HGT.Admin.Extensions;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;


namespace HGT.Admin.Controllers
{
    public class ContentController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentService _contentService;
        private readonly IEmailTemplateService _emailTemplateService;

        #endregion

        #region Contructors

        public ContentController(IUnitOfWork unitOfWork, IContentService contentService, IEmailTemplateService emailTemplateService)
        {
            this._unitOfWork = unitOfWork;
            this._contentService = contentService;
            this._emailTemplateService = emailTemplateService;
        }

        #endregion

        #region Methods

        #region edit content
        public ActionResult Index(int? id)
        {
            if (id == 0 || id == null)
                id = 1;
            var model = new ContentModel();
            var entity = _contentService.Find(id);
            if (entity != null)
                model = entity.ToModel();

            ViewData["CategoryId"] = id.ToString();
            ViewData["ListContents"] = GetListContents();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditContent(ContentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _contentService.Find(model.Id);
                if (entity != null)
                {
                    entity.LastUpdated = DateTime.Now;

                    if (entity.Name != model.Name)
                        entity.Name = model.Name;
                    if (entity.Value != model.Value)
                        entity.Value = model.Value;

                    _contentService.Update(entity);
                    _unitOfWork.SaveChanges();

                    return JsonConvert.SerializeObject(new { Status = ResultStatus.Success, Message = "Data were saved successfully!" });
                }
                return JsonConvert.SerializeObject(new { Status = ResultStatus.Fail, Message = "Data were saved unsuccessfully!" });
            }
            return JsonConvert.SerializeObject(new { Status = ResultStatus.Fail, Message = "Data were saved unsuccessfully!" });
        }

        public List<SelectListItem> GetListContents()
        {
            var model = new List<SelectListItem>();
            foreach (var item in _contentService.ODataQueryable())
            {
                model.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }

        #endregion

        #region email template

        public ActionResult Template()
        {
            return View();
        }

        public ActionResult ListTemplate([DataSourceRequest] DataSourceRequest request)
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

        // GET: /setting/Edit/5
        public ActionResult EditEmail(int? id = 0)
        {
            var model = new EmailTemplateModel();
            var emailTemplate = _emailTemplateService.Find(id);

            model = emailTemplate.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditEmail(EmailTemplateModel emailTemplateModel)
        {
            if (ModelState.IsValid)
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
                return JsonConvert.SerializeObject(new { Status = 1, Message = "Update success." });
            }
            return null;

        }

        #endregion

        #endregion
    }
}
