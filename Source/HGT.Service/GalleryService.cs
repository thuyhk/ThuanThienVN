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
    public class GalleryService : Service<Gallery>, IGalleryService
    {
        private readonly IRepositoryAsync<Gallery> _repository;

        public GalleryService(IRepositoryAsync<Gallery> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Gallery GetGalleryByObject(int objectId, int category, int type)
        {
            return _repository.GetGalleryByObject(objectId,category, type);
        }

        public List<Gallery> GetListGalleryByObjectId(int objectId, int category)
        {
             return _repository.GetListGalleryByObjectId(objectId, category);
        }

        public List<GalleryDetail> GetListGalleryDetailByObjectId(int objectId, int category, int type)
        {
            return _repository.GetListGalleryDetailByObjectId(objectId,category, type);
        }
        public List<GalleryDetail> GetListGalleryDetailByGalleryId( int galleryId)
        {
            return _repository.GetListGalleryDetailByGalleryId(galleryId);
        }
    }
}
