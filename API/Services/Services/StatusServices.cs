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
using Interfaces.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Services
{
    public class StatusServices : IStatusServices
    {
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitofwork;
        private readonly ILogger<StatusServices> logger; 

        public StatusServices(IMemoryCache _memoryCache, IConfiguration _configuration, IUnitOfWork _unitofwork, ILogger<StatusServices> _logger )
        {
            memoryCache = _memoryCache;
            configuration = _configuration;
            unitofwork = _unitofwork;
            logger = _logger; 
        }


        public async Task<ResultModel<string>> StatusAdd(Status StatusModel)
        {
            LogServices.WriteLog("Inicio Agregar Status");

            ResultModel<string> ResultModel = new ResultModel<string>();
            Status[] List = null; 
            try
            {
                ResultModel<Status[]> Result = await StatusList();
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

                Status Status = List.FirstOrDefault(x => x.StatusName.ToUpper() == StatusModel.StatusName.ToUpper());

                if (Status != null)
                {
                    LogServices.WriteLog("Nombre ya Existe");
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Nombre ya Existe";
                    ResultModel.Data = null;
                    return ResultModel;
                }

                unitofwork.GetRepository<Status>().Add(StatusModel);

                if (!unitofwork.SaveChanges())
                {
                    LogServices.WriteLog("Status No Creado");
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Status No Creado";
                    ResultModel.Data = null;
                    return ResultModel;
                }

                LogServices.WriteLog("tatus Creado Con Exito");
                ResultModel.HasError = false;
                ResultModel.Messages = "Status Creado Con Exito";
                ResultModel.Data = null;

                memoryCache.Remove(CachingList.ListStatus);// Se borra el cache de los Statuses

                return ResultModel;
            }
            catch (Exception Error)
            {
                LogServices.WriteLog($"Error Técnico Al Guardar Status: {Error.Message}");
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Guardar Status: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();

                logger.LogError($"Error: {Error.ToString()}");

                return ResultModel;
            }

        }

        public async Task<ResultModel<Status[]>> StatusList()
        {
            return await memoryCache.GetOrCreate(CachingList.ListStatus, async CacheEntry =>
            {
                Cache Cache = configuration.GetSection("Cache").Get<Cache>();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddHours(Cache.ExpirationCacheInHours);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                ResultModel<Status[]> ResultModel = new ResultModel<Status[]>();

                try
                {
                    IEnumerable<Status> List = await unitofwork.GetRepository<Status>().Get();
                    ResultModel.HasError = false;
                    ResultModel.Messages = "Statuses Listados Con Exito";
                    ResultModel.Data = List.ToArray();
                    LogServices.WriteLog("Statuses Listados Con Exito");

                    return ResultModel;
                }
                catch (Exception Error)
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Error Técnico Listando Statuses";
                    ResultModel.ExceptionMessage = Error.ToString();

                    LogServices.WriteLog("Error Técnico Listando Statuses");
                    logger.LogError($"Error: {Error.ToString()}");

                    return ResultModel;
                }
            });
        }

        public async Task<ResultModel<Status>> GetStatusByStatusId(int Id)
        {
            ResultModel<Status> ResultModel = new ResultModel<Status>();

            try
            {
                ResultModel<Status[]> Result = await StatusList();

                if (!Result.HasError)
                {
                    Status[] List = Result.Data;
                    Status Status = List.Where(x => x.StatusId == Id).FirstOrDefault();

                    if (Status != null)
                    {
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Status Encontrado";
                        ResultModel.Data = Status;
                        LogServices.WriteLog("Status Encontrado");
                    }
                    else
                    {
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Status No Encontrado";
                        ResultModel.Data = null;
                        LogServices.WriteLog("Status No Encontrado");
                    }
                }

                return ResultModel;

            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = "Error Técnico Buscando Usuari";
                ResultModel.ExceptionMessage = Error.ToString();

                LogServices.WriteLog("Error Técnico Buscando Usuari");

                return ResultModel;
            }
        }

        public async Task<ResultModel<string>> StatusUpdate(Status StatusModel)
        {
            ResultModel<string> ResultModel = new ResultModel<string>();

            try
            {
                ResultModel<Status> Result = await GetStatusByStatusId((int)StatusModel.StatusId);
                Status Status;

                if (!Result.HasError)
                {
                    Status = Result.Data;
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = Result.ExceptionMessage;
                    return ResultModel;
                }

                ResultModel<Status[]> ListStatuss = await StatusList();

                if (!ListStatuss.HasError)
                {
                    Status[] List = ListStatuss.Data;
                    Status Statuss = List.FirstOrDefault(x => x.StatusName.ToUpper() == StatusModel.StatusName.ToUpper() && x.StatusId != StatusModel.StatusId);

                    if (Statuss != null)
                    {
                        LogServices.WriteLog("Nombre ya Existe");
                        ResultModel.HasError = true;
                        ResultModel.Messages = "Nombre ya Existe";
                        ResultModel.Data = null;
                        return ResultModel;
                    }
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = Result.ExceptionMessage;
                    return ResultModel;
                }

                Status.StatusName = StatusModel.StatusName; 

                unitofwork.GetRepository<Status>().Update(Status);

                if (!unitofwork.SaveChanges())
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Status No Modificado";
                    ResultModel.Data = null;

                    LogServices.WriteLog("Status No Modificado");

                    return ResultModel;
                }

                ResultModel.HasError = false;
                ResultModel.Messages = "Status Modificado Con Exito";
                ResultModel.Data = null;
                LogServices.WriteLog("Status Modificado Con Exito");
                memoryCache.Remove(CachingList.ListStatus);// Se borra el cache de los Statuses

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Modificar Status: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();

                LogServices.WriteLog($"Error: {Error.ToString()}"); 

                return ResultModel;
            }
        }
          
    }
}