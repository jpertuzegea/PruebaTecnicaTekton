//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using Infraestructure.Entitys;
using Infraestructure.Models;

namespace Interfaces.Interfaces
{
    public interface IUserServices
    {
        Task<ResultModel<Users[]>> UserList();
        Task<ResultModel<string>> UserAdd(Users UsersModel); 
        Task<ResultModel<Users>> GetUserByUserId(int UserId);
        Task<ResultModel<Users>> GetUserByUserNetwork(string UserNetwork); 
        Task<ResultModel<string>> UserUpdate(Users UserModel);  
        Task<ResultModel<string>> DeleteCacheSecurity();
        Task<ResultModel<LoginModel>> LogIn(LoginModel LoginModel); 

    }
}
