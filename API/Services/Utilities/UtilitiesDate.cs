//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

namespace Services.Utilities
{
    public static class UtilitiesDate
    {
        public static DateTime GetCurrentHourAndDateLocal()
        {
            return (DateTime.UtcNow).AddHours(-5); // para colombia, se deben restar 5 horas a la hora utc (Meridiano de Greenwich)
        }

        public static string GetCurrentHourAndDateLocalString()
        {
            return GetCurrentHourAndDateLocal().ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
