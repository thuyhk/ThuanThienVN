using HGT.Entities.Models;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Repository.Repositories
{
    public static class OrderDetailRepository
    {
        public static List<OrderDetail> ListOrderDetailByOrderId(this IRepository<OrderDetail> repository, int orderId)
        {
            var query = repository.Queryable().Where(x => x.OrderId==orderId).ToList();
            return query;
        }
    }
}
