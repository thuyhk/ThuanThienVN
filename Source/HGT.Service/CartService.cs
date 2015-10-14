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
    public class CartService : Service<Cart>, ICartService
    {
        private readonly IRepositoryAsync<Cart> _repository;

        public CartService(IRepositoryAsync<Cart> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}
