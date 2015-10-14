using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class CategoryRepository
    {
        #region #frontend

        public static List<Category> GetListCategoryByParentId(this IRepository<Category> repository, int? rootId)
        {
            var query = repository.Queryable().Where(x => x.IsActive == true).ToList();
            if (rootId != null)
                query = query.Where(x => x.RootId == rootId).ToList();
            return query;
        }

        public static Category GetCategoryById(this IRepository<Category> repository, int id)
        {
            var query = repository.Queryable().Where(x => x.IsActive == true && x.Id == id).FirstOrDefault();
            return query;
        }

        public static Category GetCategoryByAlias(this IRepository<Category> repository, string categoryAlias)
        {
            var query = repository.Queryable().Where(x => x.IsActive == true && x.Alias == categoryAlias).FirstOrDefault();
            return query;
        }

        #endregion

        #region #backend

        public static List<Category> GetSearchKey(this IRepository<Category> repository, string keySearch)
        {
            var query = repository.Queryable().Where(x => x.Name.Contains(keySearch)).ToList();
            return query;
        }

        public static List<Category> GetListCategory(this IRepository<Category> repository, int? rootId)
        {
            var query = new List<Category>();
            if (rootId == null)
                query = repository.Queryable().Where(x => x.IsActive == true).ToList();
            else
                query = repository.Queryable().Where(x => x.IsActive == true && x.RootId == rootId).ToList();
            return query;
        }

        public static bool ValidateAlias(this IRepository<Category> repository, string categoryAlias, int? categoryId)
        {
            var query = repository.Queryable().Where(x => x.Alias.Equals(categoryAlias)).FirstOrDefault();
            if (query == null) return true;
            if (query.Id.Equals(categoryId)) return true;
            return false;
        }

        #endregion
    }
}
