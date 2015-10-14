using AutoMapper;
using HGT.Core;
using HGT.Entities.Models;
using HGT.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using HGT.Core.Models;

namespace HGT.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void LoadConfiguration()
        {
            //Mapper.CreateMap<Category, CategoryModel>();
            //Mapper.CreateMap<Product, ProductModel>();
            //Mapper.CreateMap<News, NewsViewModel>();

            // load
            //var ds = IoC.Kernel.Get<IDataService>();
        }        
    }
}