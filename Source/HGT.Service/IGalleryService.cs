using HGT.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
namespace HGT.Service
{
    public interface IGalleryService: IService<Gallery>
    {
       Gallery GetGalleryByObject(int objectId, int category, int type);
       List<Gallery> GetListGalleryByObjectId(int objectId, int category);
       List<GalleryDetail> GetListGalleryDetailByObjectId(int objectId, int category, int type);
       List<GalleryDetail> GetListGalleryDetailByGalleryId(int galleryId);
    }
}
