using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class NewsRepository
    {
        public static List<News> GetListNewsByCategoryId(this IRepository<News> repository, int? categoryId)
        {
            var query = repository.Queryable().ToList();
            if (categoryId.Value > 0)
                query = query.Where(x => x.CategoryId == categoryId).ToList();
            return query;
        }

        public static News GetNewsById(this IRepository<News> repository, int id)
        {
            var query = repository.Queryable().Where(x => x.Id == id).FirstOrDefault();
            return query;
        }

        public static News GetNewsByAlias(this IRepository<News> repository, string Alias)
        {
            var query = repository.Queryable().Where(x => x.Alias == Alias).FirstOrDefault();
            return query;
        }

        public static List<News> GetListOtherNews(this IRepository<News> repository, int? categoryId, int newId)
        {
            var query = repository.Queryable().Where(x => x.Id != newId).Take(10).ToList();
            if (categoryId.Value > 0)
                query = query.Where(x => x.CategoryId == categoryId).ToList();
            return query;
        }

        public static List<News> GetListTopNews(this IRepository<News> repository, int? categoryId)
        {
            var query = repository.Queryable().OrderByDescending(x => x.CreatedDate).Take(6).ToList();
            if (categoryId.Value > 0)
                query = query.Where(x => x.CategoryId == categoryId).ToList();
            return query;
        }

        #region #backend

        public static List<News> GetSearchKey(this IRepository<News> repository, string keySearch)
        {
            var query = repository.Queryable().Where(x=>x.Title.Contains(keySearch) || x.Content.Contains(keySearch)).ToList();
            return query;
        }

        public static bool CheckNewsTitleExist(this IRepository<News> repository, string name, int id)
        {
            var query = repository.Queryable().Where(x => x.Title.Equals(name)).FirstOrDefault();
            if (query == null) return true;
            if (query.Id.Equals(id)) return true;
            return false;
        }

        #endregion
    }
}
