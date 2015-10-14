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
    public class NewsService : Service<News>, INewsService
    {
        private readonly IRepositoryAsync<News> _repository;

        public NewsService(IRepositoryAsync<News> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<News> GetListNewsByCategoryId(int? categoryId)
        {
            return _repository.GetListNewsByCategoryId(categoryId);
        }

        public News GetNewsById(int id)
        {
            return _repository.GetNewsById(id);
        }

        public News GetNewsByAlias(string alias)
        {
            return _repository.GetNewsByAlias(alias);
        }

        public List<News> GetListOtherNews(int? categoryId, int newId)
        {
            return _repository.GetListOtherNews(categoryId, newId);
        }

        public List<News> GetListTopNews(int? categoryId)
        {
            return _repository.GetListTopNews(categoryId);
        }

        public List<News> GetSearchKey(string key)
        {
            return _repository.GetSearchKey(key);
        }

        public bool CheckNewsTitleExist(string name, int id)
        {
            return _repository.CheckNewsTitleExist(name, id);
        }
    }
}
