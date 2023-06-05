using Dapper.WebAPI.Entities;
using Dapper.WebAPI.Interfaces;

namespace Dapper.WebAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IProductRepository productRepository, IUserRepository userRepository, IRoleRepository roleRepository, IRoleClaimRepository roleClaimRepository) 
        {
            Products = productRepository;
            Users = userRepository;
            Roles = roleRepository;
            RoleClaims = roleClaimRepository;
        }
        public IProductRepository Products { get; }
        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; } 
        public IRoleClaimRepository RoleClaims { get; }
    }
}
