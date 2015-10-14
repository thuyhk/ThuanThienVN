using System;
using HGT.Entities.Models;
using HGT.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
namespace HGT.Service
{
    public interface INewsService : IService<News>
    {
        List<News> GetListNewsByCategoryId(int? categoryId);

        News GetNewsById(int id);

        News GetNewsByAlias(string alias);

        List<News> GetListOtherNews(int? categoryId, int newId);

        List<News> GetListTopNews(int? categoryId);

        bool CheckNewsTitleExist(string name, int id);

        List<News> GetSearchKey(string key);
    }
}
