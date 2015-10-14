using HGT.Admin.Models;
using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using HGT.Core;
using HGT.Core.Models;

namespace HGT.Admin.Extensions
{
    public static class MappingExtensions
    {
        // user entity => user model
        public static UserModel ToModel(this User entity)
        {
            var model = new UserModel();
            AutoMapper.Mapper.CreateMap<User, UserModel>();
            AutoMapper.Mapper.Map(entity, model);
            return model;
        }

        // user model => user entity
        public static User ToEntity(this UserModel model)
        {
            var entity = new User();
            AutoMapper.Mapper.CreateMap<UserModel, User>();
            AutoMapper.Mapper.Map(model, entity);
            return entity;
        }


        //public static Category ToEntity(this CategoryModel1 model)
        //{
        //    var entity = new Category();
        //    AutoMapper.Mapper.CreateMap<CategoryModel1, Category>();
        //    AutoMapper.Mapper.Map(model, entity);
        //    return entity;
        //}

        //public static CategoryModel1 ToModel(this Category entity)
        //{
        //    var model = new CategoryModel1();
        //    AutoMapper.Mapper.CreateMap<Category, CategoryModel1>();
        //    AutoMapper.Mapper.Map(entity, model);
        //    return model;
        //}


        //public static List<CategoryModel1> ToListModel(this List<Category> entity)
        //{
        //    var model = new List<CategoryModel1>();
        //    AutoMapper.Mapper.CreateMap<List<Category>, List<CategoryModel1>>();
        //    AutoMapper.Mapper.Map(entity, model);
        //    return model;
        //}


        public static EmailTemplateModel ToModel(this EmailTemplate entity)
        {
            var model = new EmailTemplateModel();
            AutoMapper.Mapper.CreateMap<EmailTemplate, EmailTemplateModel>();
            AutoMapper.Mapper.Map(entity, model);
            return model;
        }

        public static EmailTemplate ToEntity(this EmailTemplateModel model)
        {
            var entity = new EmailTemplate();
            AutoMapper.Mapper.CreateMap<EmailTemplateModel, EmailTemplate>();
            AutoMapper.Mapper.Map(model, entity);
            return entity;
        }
        


        public static NewsModel ToModel(this News entity)
        {
            var model = new NewsModel();
            AutoMapper.Mapper.CreateMap<News, NewsModel>();
            AutoMapper.Mapper.Map(entity, model);
            return model;
        }

        public static News ToEntity(this NewsModel model)
        {
            var entity = new News();
            AutoMapper.Mapper.CreateMap<NewsModel, News>();
            AutoMapper.Mapper.Map(model, entity);
            return entity;
        }

        public static BannerModel ToModel(this Banner entity)
        {
            var model = new BannerModel();
            AutoMapper.Mapper.CreateMap<Banner, BannerModel>();
            AutoMapper.Mapper.Map(entity, model);
            return model;
        }

        public static Banner ToEntity(this BannerModel model)
        {
            var entity = new Banner();
            AutoMapper.Mapper.CreateMap<BannerModel, Banner>();
            AutoMapper.Mapper.Map(model, entity);
            return entity;
        }
    }

}