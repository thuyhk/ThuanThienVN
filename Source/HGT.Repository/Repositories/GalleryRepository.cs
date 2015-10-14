using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class GalleryRepository
    {
        #region front-end

        // get gallery info
        public static Gallery GetGalleryByObject(this IRepository<Gallery> reponsitory, int objectId, int category, int type)
        {
            var query = reponsitory.Queryable().Where(x => x.ObjectId == objectId && x.Category == category && x.Type == type).FirstOrDefault();
            return query;
        }

        // get list gallery
        public static List<Gallery> GetListGalleryByObjectId(this IRepository<Gallery> reponsitory, int objectId, int category)
        {
            var query = reponsitory.Queryable().Where(x => x.ObjectId == objectId && x.Category == category).ToList();
            return query;
        }

        // get list gallery detail by type object id and type
        public static List<GalleryDetail> GetListGalleryDetailByObjectId(this IRepository<Gallery> repository, int objectId, int category, int type)
        {
            var result = new List<GalleryDetail>();
            var gallery = repository.Queryable().Where(x => x.ObjectId == objectId && x.Category == category && x.Type == type).FirstOrDefault();
            if(gallery!=null)
                result = repository.GetRepository<GalleryDetail>().Queryable().Where(x => x.GalleryId == gallery.Id).ToList();
            return result;
        }
        #endregion

        #region back-end

        // get list gallery detail
        public static List<GalleryDetail> GetListGalleryDetailByGalleryId(this IRepository<Gallery> repository, int galleryId)
        {
            var result = new List<GalleryDetail>();
            var gallery = repository.Queryable().Where(x => x.Id == galleryId).FirstOrDefault();
            if (gallery != null)
                result = repository.GetRepository<GalleryDetail>().Queryable().Where(x => x.GalleryId == gallery.Id).ToList();
            return result;
        }

        #endregion
    }
}
