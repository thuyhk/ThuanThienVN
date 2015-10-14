using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class StoreRepository
    {
        public static Store GetStoreById(this IRepository<Store> repository, int id)
        {
            var _result = repository.Queryable().Where(x => x.Id == id).FirstOrDefault();
            return _result;
        }

        public static List<Store> GetListStores(this IRepository<Store> repository, int? take)
        {
            var _result = new List<Store>();
            if (take != null || take > 0)
                _result = repository.Queryable().Take(take.Value).ToList();
            else
                _result = repository.Queryable().ToList();
            return _result;
        }

        public static Store GetStoreByAlias(this IRepository<Store> repository, string alias)
        {
            return repository.Queryable().Where(x => x.Alias==alias).FirstOrDefault();
        }
    }
}
