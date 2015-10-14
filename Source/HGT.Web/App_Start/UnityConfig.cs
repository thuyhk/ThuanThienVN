using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using HGT.Service;
using Repository.Pattern.DataContext;
using HGT.Entities.Models;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HGT.Web.Models;


namespace HGT.Web.App_Start
{
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {

            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            //container.RegisterType<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();

            container
                .RegisterType<IDataContextAsync, HGTContext>(new PerRequestLifetimeManager())

                .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
                .RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager())

                .RegisterType<IRepositoryAsync<Category>, Repository<Category>>()
                .RegisterType<IRepositoryAsync<Gallery>, Repository<Gallery>>()
                .RegisterType<IRepositoryAsync<GalleryDetail>, Repository<GalleryDetail>>()
                .RegisterType<IRepositoryAsync<Product>, Repository<Product>>()
                .RegisterType<IRepositoryAsync<Banner>, Repository<Banner>>()
                .RegisterType<IRepositoryAsync<News>, Repository<News>>()
                .RegisterType<IRepositoryAsync<Setting>, Repository<Setting>>()
                .RegisterType<IRepositoryAsync<Store>, Repository<Store>>()
                .RegisterType<IRepositoryAsync<Order>, Repository<Order>>()
                .RegisterType<IRepositoryAsync<OrderDetail>, Repository<OrderDetail>>()
                .RegisterType<IRepositoryAsync<Cart>, Repository<Cart>>()
                .RegisterType<IRepositoryAsync<Content>, Repository<Content>>()
                .RegisterType<IRepositoryAsync<Solution>, Repository<Solution>>()
                .RegisterType<IRepositoryAsync<SolutionProductMapping>, Repository<SolutionProductMapping>>()

                //admin
                .RegisterType<IRepositoryAsync<User>, Repository<User>>()
                .RegisterType<IRepositoryAsync<Role>, Repository<Role>>()
                .RegisterType<IRepositoryAsync<EmailTemplate>, Repository<EmailTemplate>>()
                .RegisterType<IRepositoryAsync<EmailQueue>, Repository<EmailQueue>>()

                //.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager())
                .RegisterType<ICategoryService, CategoryService>()
                .RegisterType<IProductService, ProductService>()
                .RegisterType<IGalleryService, GalleryService>()
                .RegisterType<IGalleryDetailService, GalleryDetailService>()
                .RegisterType<IBannerService, BannerService>()
                .RegisterType<INewsService, NewsService>()
                .RegisterType<ISettingService, SettingService>()
                .RegisterType<IStoreService, StoreService>()
                .RegisterType<IOrderService, OrderService>()
                .RegisterType<IOrderDetailService, OrderDetailService>()
                .RegisterType<ICartService, CartService>()
                .RegisterType<IContentService, ContentService>()
                .RegisterType<ISolutionService, SolutionService>()
                .RegisterType<ISolutionProductService, SolutionProductService>()

                //admin 
                .RegisterType<IUserService, UserService>()
                .RegisterType<IRoleService, RoleService>()
                .RegisterType<IEmailTemplateService, EmailTemplateService>()
                .RegisterType<IEmailQueueService, EmailQueueService>()
                ;
        }
    }
}