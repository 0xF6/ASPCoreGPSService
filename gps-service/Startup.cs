namespace gps_service
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public class Startup
    {
        public static async Task Main(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe) ?? Path.GetFullPath("./");
            Directory.SetCurrentDirectory(pathToContentRoot);
            await WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .Build()
                .RunAsync();
        }
            

        public Startup(IConfiguration configuration) => Configuration = configuration;

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddDistributedMemoryCache();
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddTransient<DBContext>();
            services.AddHostedService<TransformationService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseMvc();
            app.UseRouting();
            app.UseAuthorization();

            Directory.CreateDirectory("./storage");
            Directory.CreateDirectory("./storage/cfz");
        }


        #region private

        private IConfiguration Configuration { get; }

        #endregion
    }
}
