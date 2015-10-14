using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using HGT.Service;
using HGT.Repository.Repositories;


namespace HGT.Service
{

    public class StoreService : Service<Store>, IStoreService
    {
        private readonly IRepositoryAsync<Store> _repository;

        public StoreService(IRepositoryAsync<Store> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Store GetStoreById(int id)
        {
            return _repository.GetStoreById(id);
        }

        public List<Store> GetListStores(int? take)
        {
            return _repository.GetListStores(take);
        }

        public Store GetStoreByAlias(string alias)
        {
            return _repository.GetStoreByAlias(alias);
        }
    }
}
