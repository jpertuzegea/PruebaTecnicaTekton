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
    public interface IProductsServices
    {
        Task<ResultModel<string>> ProductsAdd(Products ProductsModel);
        Task<ResultModel<Products[]>> ProductsList();
        Task<ResultModel<Products>> GetProductsByProductsId(int Id);
        Task<ResultModel<string>> ProductsUpdate(Products ProductsModel);

    }
}
