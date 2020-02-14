using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;
using GrKouk.Shared.Core;
using GrKouk.Shared.Mobile.Dtos;


namespace GrKouk.CodeManager.Services
{
    public interface IDataSource
    {
        Task<AffectedResponse> UpdateNopShopProductAttrCombSkuAsync(int shopId, int productId, string skuToUse);
        Task<AffectedResponse> UpdateNopShopProductAttrCombStockQuantityAsync(int shopId, int productId,int stockQuantity);
        Task<AffectedResponse> DeleteNopShopProductAttrCombinationsAsync(int shopId,int productId);
        Task<IEnumerable<ProductListDto>> GetNopShopProductsAutocompleteListAsync(string shop);
        Task<IEnumerable<ListItemDto>> GetNopShopProductSlugsListAsync(string shop,int productId);
        Task<ListItemDto> GetNopShopPrimaryProductSlug(string shop, int productId);
        Task<IEnumerable<ProductListDto>> GetNopShopProductPictureListAsync(string shop, int productId);
        Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductListDto>> GetProductsAsync(string productNameFilter);
        Task<IEnumerable<ProductListDto>> GetCodesAsync(string codeBase);
        Task<IEnumerable<ProductListDto>> GetNopCodesAsync(string codeBase);
        Task<IEnumerable<ProductCodeLookupDto>> GetNopCodesAsyncV2(string codeBase);
        Task<IEnumerable<ListItemDto>> GetNopProductsAsync(string codeBase);
        Task<IEnumerable<ListItemDto>> GetNopProductAttributesAsync(string productId, string attributeId);

        Task<IEnumerable<ProductAttrCombinationDto>> GetShopProductAttrCombinations(int shopId, int productId);
    }
}
