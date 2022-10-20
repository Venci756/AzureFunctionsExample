using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoFuncs;
using TodoFuncs.Data;

[assembly:WebJobsStartup(typeof(Startup))]
namespace TodoFuncs
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AppConnection");
            
            builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.BuildServiceProvider();
        }
    }
}