using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class ProductRepository
    {

        #region #frontend

        public static Product GetProductById(this IRepository<Product> repository, int id)
        {
            var _result = repository.Queryable().Where(x => x.Id == id).FirstOrDefault();
            return _result;
        }

        public static Product GetProductByAlias(this IRepository<Product> repository, string alias)
        {
            var _result = repository.Queryable().Where(x => x.Alias == alias.Trim()).FirstOrDefault();
            return _result;
        }

        public static List<Product> GetListProductsByCategoryId(this IRepository<Product> repository, int categoryId, bool showChild = false)
        {
            var result = new List<Product>();
            var category = repository.GetRepository<Category>().Queryable().Where(x => x.Id.Equals(categoryId) && x.IsActive == true).FirstOrDefault();
            if (category != null)
            {
                if (showChild)
                {
                    var lstCateId = new List<int>();
                    var lstTempInt = new List<int>();
                    lstCateId.Add(category.Id);
                    lstTempInt.AddRange(repository.GetRepository<Category>().Queryable().Where(x => x.RootId == category.Id && x.IsActive == true).Select(x => x.Id).ToList());

                    lstCateId.AddRange(lstTempInt);

                    foreach (var cateLevel2 in lstTempInt)
                    {
                        var cateChild2 = repository.GetRepository<Category>().Queryable().Where(x => x.RootId == cateLevel2 && x.IsActive == true).ToList();
                        if (cateChild2 != null && cateChild2.Count > 0)
                        {
                            foreach (var cateItemChild2 in cateChild2)
                            {
                                lstCateId.Add(cateItemChild2.Id);
                            }
                        }
                    }

                    lstCateId = lstCateId.Distinct().ToList();

                    result = (from k in repository.Queryable()
                              where lstCateId.Any(x => x == k.CategoryId)
                              select k).Distinct().ToList();
                }
                else
                {
                    result = repository.Queryable().Where(x => x.CategoryId == categoryId && x.IsActive == true).ToList();
                }
            }
            return result;
        }

        public static List<Product> GetListProductsIsHighlight(this IRepository<Product> repository, int? pageSize)
        {
            var products = new List<Product>();
            var categories = repository.GetRepository<Category>().Queryable();
            products = (from k in repository.Queryable()
                        join c in categories on k.CategoryId equals c.Id
                        where c.IsActive == true && k.IsActive == true //&& k. == true
                        orderby k.CreatedDate descending
                        select k).ToList();
            if (pageSize != null)
                products = products.Take(pageSize.Value).ToList();
            return products;

        }

        public static List<Product> GetListProductsSameCategory(this IRepository<Product> repository, int categoryId, int productId)
        {
            var _lstProduct = new List<Product>();
            _lstProduct = repository.Queryable().Where(x => x.CategoryId == categoryId && x.Id != productId && x.IsActive == true).Take(10).ToList();
            return _lstProduct;

        }

        public static List<Product> GetListProductBySolutionId(this IRepository<Product> repository, int solId)
        {
            var listProduct = new List<Product>();
            var productMap =repository.GetRepository<SolutionProductMapping>().Queryable();
            listProduct = (from k in repository.Queryable()
                            join m in productMap on k.Id equals m.ProductId
                            where k.IsActive ==true && m.SolutionId == solId
                            select k).ToList();
            return listProduct;
        }

        #endregion

        #region #backend

        public static List<Product> GetSearchKey(this IRepository<Product> repository, string keySearch, int categoryId = 0)
        {
            var query = repository.Queryable().Where(x => x.IsActive == true && x.Name.Contains(keySearch)).ToList();
            if (categoryId > 0)
                query = query.Where(x => x.CategoryId.Equals(categoryId)).ToList();
            return query;
        }

        public static bool ValidateAlias(this IRepository<Product> repository, string productAlas, int? productId)
        {
            var query = repository.Queryable().Where(x => x.Name.Equals(productAlas)).FirstOrDefault();
            if (query == null) return true;
            if (query.Id.Equals(productId)) return true;
            return false;
        }

        #endregion
    }
}
