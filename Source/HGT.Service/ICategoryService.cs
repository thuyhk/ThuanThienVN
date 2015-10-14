using System;
using HGT.Entities.Models;
using HGT.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
namespace HGT.Service
{
    public interface ICategoryService : IService<Category>
    {
        Category GetCategoryById(int id);

        Category GetCategoryByAlias(string alias);

        List<Category> GetListCategoryByParentId(int? parentId);

        List<Category> GetSearchKey(string key);

        #region back-end

        List<Category> GetListCategory(int? rootId);

        bool ValidateAlias(string categoryAlas, int? categoryId);

        #endregion
    }
}
