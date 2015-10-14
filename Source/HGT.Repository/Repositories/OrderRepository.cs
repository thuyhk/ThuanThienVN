using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class OrderRepository
    {
        public static Order GetOrderById(this IRepository<Order> repository, int orderId)
        {
            var query = repository.Queryable().Where(x => x.Id==orderId).FirstOrDefault();
            return query;
        }
    }
}
