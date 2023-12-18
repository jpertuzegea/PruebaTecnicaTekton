//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

namespace Infraestructure.Models
{
    public class ResultModel<T> where T : class
    {
        public bool HasError { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? Messages { get; set; }
        public T? Data { get; set; }
    }
}
