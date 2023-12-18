//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using BaseServices.Resources;
using Infraestructure.Entitys;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using RestSharp;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Interfaces.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace Services.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitofwork;
        private readonly ILogger<ProductsServices> logger;

        public ProductsServices(IMemoryCache _memoryCache, IConfiguration _configuration, IUnitOfWork _unitofwork, ILogger<ProductsServices> _logger)
        {
            memoryCache = _memoryCache;
            configuration = _configuration;
            unitofwork = _unitofwork;
            logger = _logger;
        }


        public async Task<ResultModel<string>> ProductsAdd(Products ProductsModel)
        {
            ResultModel<string> ResultModel = new ResultModel<string>();
            Products[] List = null;
            try
            {
                ResultModel<Products[]> Result = await ProductsList();
                if (!Result.HasError)
                {
                    List = Result.Data;
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = Result.ExceptionMessage;
                    return ResultModel;
                }
                 
                unitofwork.GetRepository<Products>().Add(ProductsModel);

                if (!unitofwork.SaveChanges())
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Products No Creado";
                    LogServices.WriteLog("Products No Creado");
                    ResultModel.Data = null;
                    return ResultModel;
                }

                ResultModel.HasError = false;
                ResultModel.Messages = "Products Creado Con Exito";
                ResultModel.Data = null;
                LogServices.WriteLog("Products Creado Con Exito");
                memoryCache.Remove(CachingList.ListProducts);// Se borra el cache de los Productses

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Guardar Products: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();

                LogServices.WriteLog($"Error: {Error.ToString()}");
                return ResultModel;
            }

        }

        public async Task<ResultModel<Products[]>> ProductsList()
        {
            return await memoryCache.GetOrCreate(CachingList.ListProducts, async CacheEntry =>
            {
                Cache Cache = configuration.GetSection("Cache").Get<Cache>();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddHours(Cache.ExpirationCacheInHours);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                ResultModel<Products[]> ResultModel = new ResultModel<Products[]>();

                try
                {
                    IEnumerable<Products> List = await unitofwork.GetRepository<Products>().Get();
                    ResultModel.HasError = false;
                    ResultModel.Messages = "Products Listados Con Exito";
                    ResultModel.Data = List.ToArray();
                    LogServices.WriteLog("Products Listados Con Exito");
                    return ResultModel;
                }
                catch (Exception Error)
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Error Técnico Listando Productses";
                    ResultModel.ExceptionMessage = Error.ToString();
                     
                    LogServices.WriteLog($"Error: {Error.ToString()}");

                    return ResultModel;
                }
            });
        }

        public async Task<ResultModel<Products>> GetProductsByProductsId(int Id)
        {
            ResultModel<Products> ResultModel = new ResultModel<Products>();

            try
            {
                ResultModel<Products[]> Result = await ProductsList();

                if (!Result.HasError)
                {
                    Products[] List = Result.Data;
                    Products Products = List.Where(x => x.ProductId == Id).FirstOrDefault();

                    if (Products != null)
                    {
                        Products.Discount = GetDiscountByProductId((int)Products.ProductId);

                        Products.FinalPrice = (Products.Price * (100 - Products.Discount) / 100);

                        ResultModel.HasError = false;
                        ResultModel.Messages = "Product Encontrado";
                        ResultModel.Data = Products;
                        LogServices.WriteLog("Product Encontrado");
                    }
                    else
                    {
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Product No Encontrado";
                        ResultModel.Data = null;
                        LogServices.WriteLog("Product No Encontrado");
                    }
                }

                return ResultModel;

            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = "Error Técnico Buscando Usuari";
                ResultModel.ExceptionMessage = Error.ToString();
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");

                return ResultModel;
            }
        }

        public async Task<ResultModel<string>> ProductsUpdate(Products ProductsModel)
        {
            ResultModel<string> ResultModel = new ResultModel<string>();

            try
            {
                ResultModel<Products> Result = await GetProductsByProductsId((int)ProductsModel.ProductId);
                Products Products;

                if (!Result.HasError)
                {
                    Products = Result.Data;
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = Result.ExceptionMessage;
                    return ResultModel;
                }

                ResultModel<Products[]> ListProductss = await ProductsList();

                if (!ListProductss.HasError)
                {
                    Products[] List = ListProductss.Data;
                    Products Productss = List.FirstOrDefault(x => x.ProductId != ProductsModel.ProductId);
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = Result.ExceptionMessage;
                    return ResultModel;
                }

                Products.Name = ProductsModel.Name;
                Products.StatusId = ProductsModel.StatusId;
                Products.Description = ProductsModel.Description;
                Products.Price = ProductsModel.Price;
                 
                unitofwork.GetRepository<Products>().Update(Products);

                if (!unitofwork.SaveChanges())
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Products No Modificado";
                    ResultModel.Data = null;

                    LogServices.WriteLog("Products No Modificado");

                    return ResultModel;
                }

                ResultModel.HasError = false;
                ResultModel.Messages = "Products Modificado Con Exito";
                ResultModel.Data = null;

                LogServices.WriteLog("Products Modificado Con Exito");

                memoryCache.Remove(CachingList.ListProducts);// Se borra el cache de los Productses

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Modificar Products: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");
                return ResultModel;
            }
        }

        private decimal GetDiscountByProductId(int productId)
        {
            try
            {
                string urlService = $"{configuration.GetValue<string>($"DispuntApiURL")}?productId=" + productId;
                var client = new RestClient(urlService);
                var request = new RestRequest(new Uri(urlService), Method.Get);

                RestResponse response = client.Execute(request);
                LogServices.WriteLog("Respuesta servicio Discount " + JsonConvert.SerializeObject(response));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return int.Parse(response.Content);

                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}