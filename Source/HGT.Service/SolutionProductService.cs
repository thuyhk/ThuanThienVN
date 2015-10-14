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
    public class SolutionProductService : Service<SolutionProductMapping>, ISolutionProductService
    {
        private readonly IRepositoryAsync<SolutionProductMapping> _repository;

        public SolutionProductService(IRepositoryAsync<SolutionProductMapping> repository)
            : base(repository)
        {
            _repository = repository;
        }        
    }
}
