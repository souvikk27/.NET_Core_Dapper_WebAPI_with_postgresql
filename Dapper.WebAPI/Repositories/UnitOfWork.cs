using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;

namespace Dapper.WebAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IProductRepository productRepository, IUserRepository userRepository) 
        {
            Products = productRepository;
            Users = userRepository;
        }
        public IProductRepository Products { get; }
        public IUserRepository Users { get; }
    }
}
