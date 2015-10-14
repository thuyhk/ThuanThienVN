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

    public class OrderDetailService : Service<OrderDetail>, IOrderDetailService
    {
        private readonly IRepositoryAsync<OrderDetail> _repository;

        public OrderDetailService(IRepositoryAsync<OrderDetail> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<OrderDetail> ListOrderDetailByOrderId(int orderId)
        {
            return _repository.ListOrderDetailByOrderId(orderId);
        }

        
    }
}
