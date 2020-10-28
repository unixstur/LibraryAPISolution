using LibraryAPI.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class HealthMonitoringAPIServerStatus : IProvideServerStatusInformation
    {
        public GetStatusResponse GetCurrentStatus()
        {
            return new GetStatusResponse 
            { 
                Message = "Everything is Good!", 
                CreatedAt = DateTime.Now 
            };
        }
    }
}
