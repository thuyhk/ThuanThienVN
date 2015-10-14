using System;
using HGT.Entities.Models;
using HGT.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
namespace HGT.Service
{
    public interface IProductService : IService<Product>
    {
        Product GetProductById(int id);

        Product GetProductByAlias(string product);

        List<Product> GetListProductsByCategoryId(int id, bool showChild=false);

        List<Product> GetListProductsIsHighlight(int? pageSize);

        List<Product> GetListProductsSameCategory(int categoryId, int productId);

        List<Product> GetSearchKey(string key, int categoryId = 0);

        bool ValidateAlias(string productAias, int? productId);

        List<Product> GetListProductBySolutionId(int solId);
    }
}
