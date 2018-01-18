﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CookieSharing.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseEnvironment("Development")
                .UseUrls("http://localhost:5001")
                .Build();
    }
}
