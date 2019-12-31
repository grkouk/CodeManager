using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;

using Newtonsoft.Json;
using Xamarin.Essentials;

namespace GrKouk.CodeManager.Services
{
    public class CodeManagerDataSource : IDataSource
    {
        private const string ApiKeyHeaderName = "ApiKey";

        public async Task<IEnumerable<ProductListDto>> GetAllProductsAsync()
        {


            var webApiBaseAddress = Preferences.Get(Constants.WebApiBaseAddressKey, "http://localhost:61009/api");
            var apiCall = "/products/allproducts";
            var apiCallAddress = webApiBaseAddress + apiCall;

            //Thread.Sleep(5000);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName,"ff00ff00");
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
            var webApiBaseAddress = Preferences.Get(Constants.WebApiBaseAddressKey, "http://localhost:63481/api");
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
    }
}
