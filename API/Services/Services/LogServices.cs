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
using Microsoft.IdentityModel.Protocols;
using Services.Utilities;

namespace Services.Services
{
    public static  class LogServices
    {
        public static void WriteLog(string log)
        {
            string ruta_logs = String.Format(@"C:/LosPruebaTecnica/" );
            if (!System.IO.File.Exists(ruta_logs))
            {
                if (!Directory.Exists(ruta_logs))
                {
                    Directory.CreateDirectory(ruta_logs);
                  
                    using (StreamWriter mylogs = System.IO.File.AppendText(ruta_logs + "LOGS_Tekton.txt"))
                    {
                        mylogs.Close();
                    }
                }
            }


            try
            { 
                StreamWriter sw = new StreamWriter(ruta_logs + "LOGS_Tekton.txt", true);
                  
                sw.WriteLine("-------------------------------------------------------------------------------------------------");
                sw.WriteLine("Info [ " + UtilitiesDate.GetCurrentHourAndDateLocalString() + " ] : ");
               
                sw.WriteLine(log); 
                sw.Close();
            }
            catch (Exception e)
            {
                throw;
            }

        }
         
    }
}