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

    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IRepositoryAsync<Order> _repository;

        public OrderService(IRepositoryAsync<Order> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Order GetOrderById(int orderId)
        {
            return _repository.GetOrderById(orderId);
        }

        
    }
}
