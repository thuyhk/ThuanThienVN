using System;
using HGT.Entities.Models;
using HGT.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
namespace HGT.Service
{
    public interface IProductPictureService : IService<ProductPicture>
    {
        ProductPicture GetPictureById(int id);
        List<ProductPicture> GetPicturesByProductId(int productId);
    }
}
