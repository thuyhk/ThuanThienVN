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

    public class ProductService : Service<Product>, IProductService
    {
        private readonly IRepositoryAsync<Product> _repository;

        public ProductService(IRepositoryAsync<Product> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Product GetProductById(int id)
        {
            return _repository.GetProductById(id);
        }

        public Product GetProductByAlias(string alias)
        {
            return _repository.GetProductByAlias(alias);
        }

        public List<Product> GetListProductsByCategoryId(int id, bool showChild = false)
        {
            return _repository.GetListProductsByCategoryId(id, showChild);
        }

        public List<Product> GetListProductsIsHighlight(int? pageSize)
        {
            return _repository.GetListProductsIsHighlight(pageSize);
        }

        public List<Product> GetListProductsSameCategory(int categoryId, int productId)
        {
            return _repository.GetListProductsSameCategory(categoryId, productId);
        }

        public List<Product> GetSearchKey(string key, int categoryId=0)
        {
            return _repository.GetSearchKey(key, categoryId);
        }

        public bool ValidateAlias(string productAlias, int? productId)
        {
            return _repository.ValidateAlias(productAlias, productId);
        }

        public List<Product> GetListProductBySolutionId(int solId)
        {
            return _repository.GetListProductBySolutionId(solId);
        }
    }
}
