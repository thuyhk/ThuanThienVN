using HGT.Core.Models;
using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace HGT.Core.Extensions
{
    public static class MappingExtensions
    {
        #region #category
        public static CategoryModel ToModel(this Category entity)
        {
            Mapper.CreateMap<Category, CategoryModel>();
            return Mapper.Map<Category, CategoryModel>(entity);
        }

        public static List<CategoryModel> ToListModel(this List<Category> entity)
        {
            Mapper.CreateMap<Category, CategoryModel>();
            return Mapper.Map<List<Category>, List<CategoryModel>>(entity);
        }
        #endregion category

        #region #product
        public static ProductModel ToModel(this Product entity)
        {
            var model = new ProductModel();
            Mapper.CreateMap<Product, ProductModel>()
                .ForMember(m => m.CategoryAlias, conf => conf.MapFrom(m => m.Category.Alias));
            Mapper.Map(entity, model);

            return model;

            //Mapper.CreateMap<Product, ProductModel>(); 
            // Mapper.Map<Product, ProductModel>(entity);


        }

        public static List<ProductModel> ToListModel(this List<Product> entity)
        {
            Mapper.CreateMap<Product, ProductModel>(); 
            return Mapper.Map<List<Product>, List<ProductModel>>(entity);
        }
        #endregion product

        #region #content
        public static ContentModel ToModel(this Content entity)
        {
            var model = new ContentModel();
            AutoMapper.Mapper.CreateMap<Content, ContentModel>();
            AutoMapper.Mapper.Map(entity, model);
            return model;
        }

        public static Content ToEntity(this ContentModel model)
        {
            var entity = new Content();
            AutoMapper.Mapper.CreateMap<ContentModel, Content>();
            AutoMapper.Mapper.Map(model, entity);
            return entity;
        }

        #endregion

        #region #product
        public static BannerModel ToModel(this Banner entity)
        {
            Mapper.CreateMap<Banner, BannerModel>();
            return Mapper.Map<Banner, BannerModel>(entity);
        }

        public static List<BannerModel> ToListModel(this List<Banner> entity)
        {
            Mapper.CreateMap<Banner, BannerModel>();
            return Mapper.Map<List<Banner>, List<BannerModel>>(entity);
        }
        #endregion product

        #region #email  queue
        public static EmailQueueModel ToModel(this EmailQueue entity)
        {
            Mapper.CreateMap<EmailQueue, EmailQueueModel>();
            return Mapper.Map<EmailQueue, EmailQueueModel>(entity);
        }

        public static List<EmailQueueModel> ToListModel(this List<EmailQueue> entity)
        {
            Mapper.CreateMap<EmailQueue, EmailQueueModel>();
            return Mapper.Map<List<EmailQueue>, List<EmailQueueModel>>(entity);
        }
        #endregion

        #region solution
        public static SolutionModel ToModel(this Solution entity)
        {
            Mapper.CreateMap<Solution, SolutionModel>();
            return Mapper.Map<Solution, SolutionModel>(entity);
        }

        public static List<SolutionModel> ToListModel(this List<Solution> entity)
        {
            Mapper.CreateMap<Solution, SolutionModel>();
            return Mapper.Map<List<Solution>, List<SolutionModel>>(entity);
        }
        #endregion
    }
}
