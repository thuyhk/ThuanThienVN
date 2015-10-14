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
    public class BannerService : Service<Banner>, IBannerService
    {
        private readonly IRepositoryAsync<Banner> _repository;

        public BannerService(IRepositoryAsync<Banner> repository)
            : base(repository)
        {
            _repository = repository;
        }       
    }
}
