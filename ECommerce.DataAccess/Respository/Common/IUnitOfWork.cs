using ECommerce.DataAccess.Respository.ProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.Common
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }

        IProductImageRepository ProductImage { get; }

        IProductTranslationRepository ProductTranslation { get; }

        Task Save();
    }
}