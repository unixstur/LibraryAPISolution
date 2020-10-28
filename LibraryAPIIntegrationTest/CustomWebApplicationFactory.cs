using LibraryAPI;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryAPIIntegrationTest
{
    public class CustomWebApplicationFactory: WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.Single(s =>
                    s.ServiceType == typeof(IProvideServerStatusInformation)
                    );

                services.Remove(serviceDescriptor);
                services.AddTransient<IProvideServerStatusInformation, DummyServerStatus>();
            });
        }
    }

    public class DummyServerStatus: IProvideServerStatusInformation
    {
        public LibraryAPI.Models.Status.GetStatusResponse GetCurrentStatus()
        {
            return new LibraryAPI.Models.Status.GetStatusResponse
            {
                Message = "The Crow Flies at Midnight",
                CreatedAt = new DateTime(1969, 4, 20, 23, 59, 00)
            };
        }
    }
}
