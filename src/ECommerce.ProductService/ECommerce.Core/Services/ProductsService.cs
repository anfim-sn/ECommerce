using AutoMapper;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services;

internal class ProductsService(IProductRepository productRepository, IMapper mapper) : IProductsService
{
    
}