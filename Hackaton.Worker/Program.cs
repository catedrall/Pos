using Hackaton.Api.Data;
using Hackaton.Worker.Repository.Interface;
using Hackaton.Worker.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<DbContextClass>(options =>
                        options.UseSqlServer("HakatonApi"));

                    services.AddTransient<IEmailService, EmailService>();

                    services.AddHostedService<Worker>();
                });
    }
}
