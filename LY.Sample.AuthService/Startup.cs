using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using IdentityServer4.AccessTokenValidation;
using LY.Sample.AuthService.Authorize;
using LY.Sample.AuthService.Core;
using LY.Sample.AuthService.Data;
using LY.Sample.AuthService.Dependency;
using LY.Sample.AuthService.Exceptions;
using LY.Sample.AuthService.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SM.AutoMapper.Extensions;
using SM.UnitOfWork;
using Module = Autofac.Module;

namespace LY.Sample.AuthService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //全局过滤器
            services.AddControllers(options =>
                {
                    options.Filters.Add(new AuthorizeFilter());
                    options.Filters.Add(new ModelActionFilter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;  //抑制系统自带模型验证
                })
                .AddControllersAsServices();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserStore, UserStore>();
            services.AddSingleton(new ContainerBuilder());
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            }).AddUnitOfWork<AppDbContext>();  //UnitOfWork
            services.AddCap(options =>
            {
                options.UseEntityFramework<AppDbContext>();
                options.UseRabbitMQ(o =>
                {
                    o.HostName = Configuration["RabbitMQ:HostName"];
                    o.VirtualHost = Configuration["RabbitMQ:VirtualHost"];
                    o.Port = int.Parse(Configuration["RabbitMQ:Port"]);
                    o.UserName = Configuration["RabbitMQ:UserName"];
                    o.Password = Configuration["RabbitMQ:Password"];
                });
                options.FailedRetryCount = 5;
                options.FailedRetryInterval = 60;
                options.UseDashboard();

            });
            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "Auth Service";
                settings.Description = "授权服务";
                settings.Version = "v1";
                settings.DocumentName = "auth";
                settings.AddSecurity("身份认证Token", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme()
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey
                });
            });
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
                .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
                .AddInMemoryClients(InMemoryConfig.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                .AddExtensionGrantValidator<SmsVerficationCodeValidator>();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.ApiName = "authservice";
                    options.ApiSecret = "secret";
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                });
            var redis = new CSRedis.CSRedisClient(Configuration.GetSection("Redis:Host").Value);
            RedisHelper.Initialization(redis);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            services.AddAutoMapper();
            ConfigureContainer(new ContainerBuilder());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHost host,ContainerBuilder builder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UserAppExceptionHandler(options => { options.HandleType = AppExceptionHandleType.JsonHandle; });
            app.UseAutoMapper();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<DefaultModuleRegister>();
        }
    }

   
}
