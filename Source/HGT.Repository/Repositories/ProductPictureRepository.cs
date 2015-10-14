using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class ProductPictureRepository
    {

        public static ProductPicture GetPictureById(this IRepository<ProductPicture> repository, int id)
        {
            var query = repository.Queryable().Where(x => x.Id == id).FirstOrDefault();
            return query;
        }

        public static List<ProductPicture> GetPicturesByProductId(this IRepository<ProductPicture> repository, int productId)
        {
            var query = repository.Queryable().Where(x => x.ProductId == productId).ToList();
            return query;
        }  
    }
}
