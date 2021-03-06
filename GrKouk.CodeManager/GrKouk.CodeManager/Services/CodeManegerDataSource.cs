﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GrKouk.CodeManager.Helpers;
using GrKouk.CodeManager.Models;
using GrKouk.Shared.Core;
using GrKouk.Shared.Mobile.Dtos;
using Newtonsoft.Json;
using Polly;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.Services
{
    public class CodeManagerDataSource : IDataSource
    {
        private const string ApiKeyHeaderName = "ApiKey";

        public async Task<IEnumerable<ProductListDto>> GetAllProductsAsync()
        {
            var webApiErpBaseAddress = Preferences.Get(Constants.WebApiErpBaseAddressKey, "http://localhost:61009/api");
            var apiCall = "/products/productcodes?codebase=1";
            //var apiCall = "/products/allproducts";
            var apiCallAddress = webApiErpBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName,"ff00ff00");
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;

                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                // return null;
                throw new Exception(e.Message, e.InnerException);
            }
        }
        public async Task<IEnumerable<ProductListDto>> GetAllProducts1Async()
        {
            var webApiErpBaseAddress = Preferences.Get(Constants.WebApiErpBaseAddressKey, "http://localhost:61009/api");
            var apiCall = "/products/allproducts";
            var apiCallAddress = webApiErpBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, "ff00ff00");
            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;

                }

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //throw;
            }
        }
        public Task<IEnumerable<ProductListDto>> GetProductsAsync(string productNameFilter)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ProductListDto>> GetNopCodesAsync(string codeBase)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/codes?codebase={codeBase}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, "ff00ff00");

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.GetAsync(apiCallAddress, ct), cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new List<ProductListDto>();
           
        }
        public async Task<IEnumerable<ProductListDto>> GetNopCodes1Async(string codeBase)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/codes?codebase={codeBase}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;
                }
                return new List<ProductListDto>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<ProductListDto>();
                //throw;
            }
        }
        public async Task<IEnumerable<ProductListDto>> GetCodesAsync(string codeBase)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiErpBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/codes?codebase={codeBase}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;
                }
                return new List<ProductListDto>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<ProductListDto>();
                //throw;
            }
        }
        public async Task<IEnumerable<ListItemDto>> GetNopProductsAsync(string codeBase)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/codes?codebase={codeBase}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ListItemDto>>(jsonContent);
                    return itemsList;
                }
                return new List<ListItemDto>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<ListItemDto>();
                //throw;
            }
        }
        public async Task<IEnumerable<ListItemDto>> GetNopProductAttributesAsync(string productId, string attributeId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/codes?productid={productId}&attributeid={attributeId}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ListItemDto>>(jsonContent);
                    return itemsList;
                }
                return new List<ListItemDto>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<ListItemDto>();
                //throw;
            }
        }

        public async Task<IEnumerable<ProductAttrCombinationDto>> GetShopProductAttrCombinations(int shopId, int productId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/ShopProductAttrCombinations?productid={productId}&shopid={shopId}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.GetAsync(apiCallAddress, ct), cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductAttrCombinationDto>>(jsonContent);
                    return itemsList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new List<ProductAttrCombinationDto>();
        }

        public async Task<AffectedResponse> DeleteNopShopProductAttrCombinationsAsync(int shopId, int productId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/DeleteShopProductAttrCombinations?productid={productId}&shopid={shopId}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.PostAsync(apiCallAddress, null, ct),
                    cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var retResponse = JsonConvert.DeserializeObject<AffectedResponse>(jsonContent);
                    return retResponse;
                }
            }
           
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new AffectedResponse
            {
                ToAffectCount = 0,
                AffectedCount = 0
            };
        }

        public async Task<AffectedResponse> UpdateNopShopProductAttrCombStockQuantityAsync(int shopId, int productId,int stockQuantity)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/UpdateShopProductAttrCombStock?productid={productId}&shopid={shopId}&stockQuantity={stockQuantity}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.PostAsync(apiCallAddress, null, ct),
                    cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var retResponse = JsonConvert.DeserializeObject<AffectedResponse>(jsonContent);
                    return retResponse;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new AffectedResponse
            {
                ToAffectCount = 0,
                AffectedCount = 0
            };
        }
        public async Task<AffectedResponse> UpdateNopShopProductAttrCombSkuAsync(int shopId, int productId, string skuToUse)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/UpdateShopProductAttrCombSku?productid={productId}&shopid={shopId}&skuToUse={skuToUse}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.PostAsync(apiCallAddress, null, ct),
                    cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var retResponse = JsonConvert.DeserializeObject<AffectedResponse>(jsonContent);
                    return retResponse;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new AffectedResponse
            {
                ToAffectCount = 0,
                AffectedCount = 0
            };
        }
        public async Task<IEnumerable<ProductListDto>> GetNopShopProductsAutocompleteListAsync(string shop)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/FltShopProductsAutoCompleteList?shop={shop}";
            var apiCallAddress = webApiBaseAddress + apiCall;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName,webNopApiKey);
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;

                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                // return null;
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<IEnumerable<ListItemDto>> GetNopShopProductSlugsListAsync(string shop, int productId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/ShopProductSlugs?shop={shop}&productid={productId}";
            var apiCallAddress = webApiBaseAddress + apiCall;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, "ff00ff00");
            // httpClient.Timeout = TimeSpan.FromMinutes(1);
            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ListItemDto>>(jsonContent);
                    return itemsList;

                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                // return null;
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<ListItemDto> GetNopShopPrimaryProductSlug(string shop, int productId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/ShopProductPrimarySlug?shop={shop}&productid={productId}";
            var apiCallAddress = webApiBaseAddress + apiCall;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, "ff00ff00");
            // httpClient.Timeout = TimeSpan.FromMinutes(1);
            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<ListItemDto>(jsonContent);
                    return itemsList;

                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                // return null;
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<IEnumerable<ProductListDto>> GetNopShopProductPictureListAsync(string shop,int productId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var apiCall = $"/products/ShopProductPictures?shop={shop}&productid={productId}";
            var apiCallAddress = webApiBaseAddress + apiCall;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, "ff00ff00");
           // httpClient.Timeout = TimeSpan.FromMinutes(1);
            try
            {
                var uri = new Uri(apiCallAddress);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;

                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                // return null;
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<IEnumerable<ProductCodeLookupDto>> GetNopCodesAsyncV2(string codeBase)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/codes?codebase={codeBase}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.GetAsync(apiCallAddress, ct), cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductCodeLookupDto>>(jsonContent);
                    return itemsList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new List<ProductCodeLookupDto>();
        }
        public async Task<IEnumerable<ProductListDto>> GetShopFeaturedProductList(int shopId)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/ShopFeaturedProductList?shopid={shopId}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.GetAsync(apiCallAddress, ct), cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonContent);
                    return itemsList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new List<ProductListDto>();
        }
        public async Task<GrKoukApiInfo> GetApiInfoAsync()
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            //var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/info";
            var l = webApiBaseAddress.Length;
            var stripedApiBase = webApiBaseAddress.Substring(0,l-4);
            var apiCallAddress = stripedApiBase + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.GetAsync(apiCallAddress, ct), cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<GrKoukApiInfo>(jsonContent);
                    return apiResponse;
                }
                else
                {
                    return new GrKoukApiInfo()
                    {
                        AssemblyVersion = "Error",
                        ServerName = "Error",
                        ShopName = $"{response.RequestMessage} inner {response.StatusCode.ToString()}"
                    };
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
                string msgInner = e.InnerException?.Message;
                return new GrKoukApiInfo()
                {
                    AssemblyVersion = "Error",
                    ServerName = "Error",
                    ShopName = $"{msg} inner {msgInner}"
                };
            }
           
        }
        public async Task<AffectedResponse> UncheckFeaturedProductsAsync(int shopId,  string productIdList)
        {
            var webApiBaseAddress = Preferences.Get(Constants.WebApiNopBaseAddressKey, "http://localhost:63481/api");
            var webNopApiKey = Preferences.Get(Constants.WebNopApikeyKey, "");
            var apiCall = $"/products/UncheckFeaturedProducts?shopid={shopId}&productIdList={productIdList}";
            var apiCallAddress = webApiBaseAddress + apiCall;

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            CancellationToken cancellationToken = CancellationToken.None;
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(new[] {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, webNopApiKey);

            try
            {
                var response = await policy.ExecuteAsync(ct => httpClient.PostAsync(apiCallAddress, null, ct),
                    cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var retResponse = JsonConvert.DeserializeObject<AffectedResponse>(jsonContent);
                    return retResponse;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new AffectedResponse
            {
                ToAffectCount = 0,
                AffectedCount = 0
            };
        }
    }
}
