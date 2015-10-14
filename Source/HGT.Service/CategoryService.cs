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

    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IRepositoryAsync<Category> _repository;

        public CategoryService(IRepositoryAsync<Category> repository)
            : base(repository)
        {
            _repository = repository;
        }

        #region frontend

        public Category GetCategoryById(int id)
        {
            return _repository.GetCategoryById(id);
        }

        public Category GetCategoryByAlias(string alias)
        {
            return _repository.GetCategoryByAlias(alias);
        }

        public List<Category> GetListCategoryByParentId(int? rootId)
        {
            return _repository.GetListCategoryByParentId(rootId);
        }

        #endregion

        #region backend

        public List<Category> GetSearchKey(string key)
        {
            return _repository.GetSearchKey(key);
        }

        public List<Category> GetListCategory(int? rootId)
        {
            return _repository.GetListCategory(rootId);
        }

        public bool ValidateAlias(string categoryAlias, int? categoryId)
        {
            return _repository.ValidateAlias(categoryAlias, categoryId);
        }

        #endregion
    }
}
