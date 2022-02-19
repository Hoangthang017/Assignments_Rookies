using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DataAccess.Entities;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request;
using ECommerce.Models.ViewModels;

namespace ECommerce.DataAccess.Respository.ProductRepo
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> Create(CreateProductRequest product);

        Task<bool> Update(UpdateProductRequest request);
    }
}