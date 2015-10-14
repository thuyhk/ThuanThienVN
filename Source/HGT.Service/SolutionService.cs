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

    public class SolutionService : Service<Solution>, ISolutionService
    {
        private readonly IRepositoryAsync<Solution> _repository;

        public SolutionService(IRepositoryAsync<Solution> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public  List<Solution> GetListSolutions(int? rootId)
        {
            return _repository.GetListSolutions(rootId);
        }

        public bool ValidateAlias(string solutionAlas, int? solutionId)
        {
            return _repository.ValidateAlias(solutionAlas, solutionId);
        }

        public Solution GetSolutionById(int solId)
        {
            return _repository.GetSolutionById(solId);
        }       
 
        public List<Solution> GetListSolutions()
        {
            return _repository.GetListSolutions();
        }
    }
}
