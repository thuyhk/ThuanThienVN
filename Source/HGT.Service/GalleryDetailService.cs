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
    public class GalleryDetailService : Service<GalleryDetail>, IGalleryDetailService
    {
        private readonly IRepositoryAsync<GalleryDetail> _repository;

        public GalleryDetailService(IRepositoryAsync<GalleryDetail> repository)
            : base(repository)
        {
            _repository = repository;
        }

        
    }
}
