using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class SolutionRepository
    {
        public static Solution GetSolutionById(this IRepository<Solution> repository, int id)
        {
            var query = repository.Queryable().Where(x => x.Id==id).FirstOrDefault();
            return query;
        }

        public static List<Solution> GetListSolutions(this IRepository<Solution> repository, int? rootId)
        {
            var query = new List<Solution>();
            if (rootId == null)
                query = repository.Queryable().Where(x => x.IsActive == true).ToList();
            else
                query = repository.Queryable().Where(x => x.IsActive == true && x.RootId == rootId).ToList();
            return query;
        }

        public static bool ValidateAlias(this IRepository<Solution> repository, string solutionAlias, int? solutionId)
        {
            var query = repository.Queryable().Where(x => x.Alias.Equals(solutionAlias)).FirstOrDefault();
            if (query == null) return true;
            if (query.Id.Equals(solutionId)) return true;
            return false;
        }

        public static List<Solution> GetListSolutions(this IRepository<Solution> repository)
        {
            var query = repository.Queryable();
            return query.ToList();
        }

    }
}
