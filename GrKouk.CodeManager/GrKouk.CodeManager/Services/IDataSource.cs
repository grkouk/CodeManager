using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;


namespace GrKouk.CodeManager.Services
{
    public interface IDataSource
    {
        Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductListDto>> GetProductsAsync(string productNameFilter);
        Task<IEnumerable<ProductListDto>> GetCodesAsync(string codeBase);
        Task<IEnumerable<CodeDto>> GetNopCodesAsync(string codeBase);
    }
}
