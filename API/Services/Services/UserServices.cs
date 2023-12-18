//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using BaseServices.Resources;
using BCrypt.Net;
using Infraestructure.Entitys;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using Interfaces.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IConfiguration configuration;
        private readonly ILogger<UserServices> logger;
        private readonly IMemoryCache memoryCache;  

        public UserServices(
             IMemoryCache _memoryCache,
             IConfiguration _configuration,
             IUnitOfWork _unitofwork,
             ILogger<UserServices> _logger            
             )
        {
            unitofwork = _unitofwork;
            configuration = _configuration;
            logger = _logger;
            memoryCache = _memoryCache;
           }


        public async Task<ResultModel<string>> UserAdd(Users UserModel)
        {
            ResultModel<string> ResultModel = new ResultModel<string>();
            string Pass = UserModel.Password;
            Users[] List = null;

            try
            {
                ResultModel<Users[]> Result = await UserList();
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
                  
                if (UserModel.UserNetwork == null)
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Formato de nombre No válido";
                    LogServices.WriteLog("Formato de nombre No válido");
                    ResultModel.Data = null;
                    return ResultModel;
                }

                Users User = List.FirstOrDefault(x => x.UserNetwork.ToUpper() == UserModel.UserNetwork.ToUpper());

                if (User != null)
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "UserName ya Existe";
                    LogServices.WriteLog("UserName ya Existe");
                    ResultModel.Data = null;
                    return ResultModel;
                }

                UserModel.Password = ComputeHash(UserModel.Password, HashType.SHA256);// Encripta la clave 
                
                unitofwork.GetRepository<Users>().Add(UserModel);

                if (!unitofwork.SaveChanges())
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "User No Creado";
                    LogServices.WriteLog("User No Creado");
                    ResultModel.Data = string.Empty;
                }
                else
                {
                    ResultModel.HasError = false;
                    ResultModel.Messages = "User Creado Con Exito";
                    LogServices.WriteLog("User Creado Con Exito");
                    ResultModel.Data = string.Empty;

                    memoryCache.Remove(CachingList.ListUsers);// Se borra el cache de los Usuarios
                }

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Guardar User: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");
                return ResultModel;
            }
        }

        public async Task<ResultModel<Users[]>> UserList()
        {
            return await memoryCache.GetOrCreate(CachingList.ListUsers, async CacheEntry =>
            {
                Cache Cache = configuration.GetSection("Cache").Get<Cache>();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddHours(Cache.ExpirationCacheInHours);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                ResultModel<Users[]> ResultModel = new ResultModel<Users[]>();

                try
                {
                    IEnumerable<Users> List = await unitofwork.GetRepository<Users>().Get();
                    ResultModel.HasError = false;
                     
                    ResultModel.Data = List.ToArray();

                    ResultModel.Messages = "Usuarios Listados Con Exito";
                    LogServices.WriteLog("Usuarios Listados Con Exito");
                    return ResultModel;
                }
                catch (Exception Error)
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Error Técnico Listando Usuarios";
                    ResultModel.Data = null;
                    ResultModel.ExceptionMessage = Error.ToString();
                     
                    LogServices.WriteLog($"Error: {Error.ToString()}");
                    return ResultModel;
                }
            });
        }

        private string ComputeHash(string input, HashType hashType)
        {
            byte[] numArray;
            byte[] bytes = Encoding.ASCII.GetBytes(input);

            using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashType.ToString().ToUpperInvariant()))
            {
                numArray = hashAlgorithm.ComputeHash(bytes);
            }
            return BitConverter.ToString(numArray).Replace("-", string.Empty).ToLower();
        }
         
        public async Task<ResultModel<Users>> GetUserByUserId(int UserId)
        {
            ResultModel<Users> ResultModel = new ResultModel<Users>();

            try
            {
                ResultModel<Users[]> Result = await UserList();

                if (!Result.HasError)
                {
                    Users[] List = Result.Data;
                    Users User = List.Where(x => x.UserId == UserId).FirstOrDefault();

                    if (User != null)
                    { 
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Usuario Encontrado";
                        ResultModel.Data = User;
                        LogServices.WriteLog("Usuario Encontrado");
                    }
                    else
                    {
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Usuario No Encontrado";
                        ResultModel.Data = null;
                        LogServices.WriteLog("Usuario No Encontrado");
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

        public async Task<ResultModel<Users>> GetUserByUserNetwork(string UserNetwork)
        {
            ResultModel<Users> ResultModel = new ResultModel<Users>();

            try
            {
                ResultModel<Users[]> Result = await UserList();

                if (!Result.HasError)
                {
                    Users[] List = Result.Data;
                    Users User = List.Where(x => x.UserNetwork.ToUpper() == UserNetwork.ToUpper()).FirstOrDefault();

                    if (User != null)
                    { 
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Usuario Encontrado";
                        ResultModel.Data = User;
                        LogServices.WriteLog("Usuario Encontrado");
                    }
                    else
                    {
                        ResultModel.HasError = false;
                        ResultModel.Messages = "Usuario No Encontrado";
                        ResultModel.Data = null;
                        LogServices.WriteLog("Usuario No Encontrado");
                    }
                }
                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = "Error Técnico Buscando Usuario";
                ResultModel.ExceptionMessage = Error.ToString();
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");
                return ResultModel;
            }
        }
 
        public async Task<ResultModel<string>> UserUpdate(Users UserModel)
        { 
            ResultModel<string> ResultModel = new ResultModel<string>();

            try
            {
                ResultModel<Users> Result = await GetUserByUserId((int)UserModel.UserId);
                Users User;

                if (!Result.HasError)
                {
                    User = Result.Data;
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = Result.ExceptionMessage;
                    return ResultModel;
                }

                ResultModel<Users[]> ListUsers = await UserList();

                if (!ListUsers.HasError)
                {
                    Users[] List = ListUsers.Data;
                     
                    Users Users = List.FirstOrDefault(x => x.UserNetwork.ToUpper() == UserModel.UserNetwork.ToUpper() && x.UserId != UserModel.UserId);

                    if (Users != null)
                    {
                        ResultModel.HasError = true;
                        ResultModel.Messages = "UserName ya Existe";
                        ResultModel.Data = null;
                        LogServices.WriteLog("UserName ya Existe");
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

                User.Name = UserModel.Name;
                User.UserNetwork = UserModel.UserNetwork;
                User.Password = ComputeHash(UserModel.Password, HashType.SHA256);// Encripta la clave 

                unitofwork.GetRepository<Users>().Update(User);

                if (!unitofwork.SaveChanges())
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = "Usuario No Modificado";
                    ResultModel.Data = null;
                    LogServices.WriteLog("Usuario No Modificado");
                    return ResultModel;
                }

                ResultModel.HasError = false;
                ResultModel.Messages = "Usuario Modificado Con Exito";
                ResultModel.Data = null;
                LogServices.WriteLog("Usuario Modificado Con Exito");

                memoryCache.Remove(CachingList.ListUsers);// Se borra el cache de los Usuarios

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Modificar Usuario: {Error.ToString()}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");

                return ResultModel;
            }
        }
 
    
        public async Task<ResultModel<string>> DeleteCacheSecurity()
        {
            ResultModel<string> ResultModel = new ResultModel<string>();
            try
            {
                await Task.Run(() =>
                { 
                    memoryCache.Remove(CachingList.ListUsers);
                    memoryCache.Remove(CachingList.ListStatus);
                    memoryCache.Remove(CachingList.ListProducts);

                });

                ResultModel.HasError = false;
                ResultModel.Messages = "Cache Eliminado Exitosamente";
                ResultModel.Data = string.Empty;
                LogServices.WriteLog("Cache Eliminado Exitosamente");
                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Eliminando Cache: {Error.Message}";
                ResultModel.ExceptionMessage = Error.ToString();
                ResultModel.Data = null;
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");
                return ResultModel;
            }
        }

        public async Task<ResultModel<LoginModel>> LogIn(LoginModel LoginModel)
        {
            ResultModel<LoginModel> ResultModel = new ResultModel<LoginModel>();

            if (LoginModel.UserNetwork == null || LoginModel.Password == null)
            {
                ResultModel.HasError = false;
                ResultModel.Data = null;
                ResultModel.Messages = "Usuario y Clave son requeridos";
                LogServices.WriteLog("Usuario y Clave son requeridos");
                return ResultModel;
            }

            try
            {
                LoginModel.Password = ComputeHash(LoginModel.Password, HashType.SHA256);

                var Result = await UserList();

                if (!Result.HasError)
                {
                    Users[] UserList = Result.Data;

                    Users Users = UserList.FirstOrDefault(x => x.UserNetwork.ToUpper() == LoginModel.UserNetwork.ToUpper() && x.Password == LoginModel.Password);

                    if (Users != null)
                    { 
                        LoginModel.IsLogued = true;
                        LoginModel.Token = BuildToken(Users);
                        LoginModel.Password = "";

                        ResultModel.HasError = false;
                        ResultModel.Data = LoginModel;
                        ResultModel.Messages = "Usuario Logueado Con Exito";
                        LogServices.WriteLog("Usuario Logueado Con Exito");

                    }
                    else
                    {
                        LoginModel.IsLogued = false;
                        LoginModel.Token = "";
                        LoginModel.Password = "";

                        ResultModel.HasError = true;
                        ResultModel.Data = LoginModel;
                        ResultModel.Messages = "Usuario NO Logueado";
                        LogServices.WriteLog("Usuario NO Logueado");

                    }
                }
                else
                {
                    ResultModel.HasError = true;
                    ResultModel.Messages = Result.Messages;
                    ResultModel.Data = null;
                    ResultModel.ExceptionMessage = Result.ExceptionMessage;
                }

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Iniciar Sesion: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();
                 
                LogServices.WriteLog($"Error: {Error.ToString()}");

                return ResultModel;
            }
        }

     
        private string BuildToken(Users User)
        {
            int user = (int)User.UserId; 

            var Claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, User.UserNetwork),
                new Claim("UserId", User.UserId.ToString()), 
                new Claim("Name", User.Name),
                new Claim("UserNetwork", User.UserNetwork.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Genera un GUID por cada token
            };

            JWTAuthentication JWTAuthenticationSection = configuration.GetSection("JWTAuthentication").Get<JWTAuthentication>();

            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTAuthenticationSection.Secret));
            var Credenciales = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            DateTime Expiration = DateTime.Now.AddMinutes(JWTAuthenticationSection.ExpirationInMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "",
               audience: "",
               claims: Claims,
               expires: Expiration,
               signingCredentials: Credenciales,
               notBefore: DateTime.Now.AddMilliseconds(2)
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
          

    }
}