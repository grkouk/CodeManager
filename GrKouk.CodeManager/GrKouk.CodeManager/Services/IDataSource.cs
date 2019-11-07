using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;
using GrKouk.InfoSystem.Dtos.MobileDtos;

namespace GrKouk.CodeManager.Services
{
    public interface IDataSource
    {
        Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductListDto>> GetProductsAsync(string productNameFilter);
        Task<IEnumerable<CodeDto>> GetCodesAsync(string codeBase);
    }
}
