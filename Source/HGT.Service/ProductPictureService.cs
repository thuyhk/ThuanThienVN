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
    public class ProductPictureService : Service<ProductPicture>, IProductPictureService
    {
        private readonly IRepositoryAsync<ProductPicture> _repository;

        public ProductPictureService(IRepositoryAsync<ProductPicture> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public ProductPicture GetPictureById(int id)
        {
            return _repository.GetPictureById(id);
        }

        public List<ProductPicture> GetPicturesByProductId(int productId)
        {
            return _repository.GetPicturesByProductId(productId);
        }
       
    }
}
