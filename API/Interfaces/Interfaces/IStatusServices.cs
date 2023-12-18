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
    public interface IStatusServices
    {
        Task<ResultModel<string>> StatusAdd(Status StatusModel);

        Task<ResultModel<Status[]>> StatusList();

        Task<ResultModel<Status>> GetStatusByStatusId(int Id);

        Task<ResultModel<string>> StatusUpdate(Status StatusModel);
         
    }
}
