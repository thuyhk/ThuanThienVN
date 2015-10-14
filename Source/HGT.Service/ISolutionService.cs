using System;
using HGT.Entities.Models;
using HGT.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
namespace HGT.Service
{
    public interface ISolutionService : IService<Solution>
    {
        Solution GetSolutionById(int solId);
        List<Solution> GetListSolutions(int? rootId);
        bool ValidateAlias(string solutionAlas, int? solutionId);
        List<Solution> GetListSolutions();
    }
}
