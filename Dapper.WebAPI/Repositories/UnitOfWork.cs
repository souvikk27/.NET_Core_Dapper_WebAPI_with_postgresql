using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;

namespace Dapper.WebAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IProductRepository productRepository) 
        {
            Products = productRepository;
        }
        public IProductRepository Products { get; }
    }
}
