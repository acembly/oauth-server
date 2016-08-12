﻿using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.AutoMapper;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Seed;
using Promact.Oauth.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Tests
{
    public class BaseProvider
    {
        public IServiceProvider serviceProvider { get; set; }

        private MapperConfiguration _mapperConfiguration { get; set; }

        public BaseProvider()
        {

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<PromactOauthDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IEnsureSeedData, EnsureSeedData>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IConsumerAppRepository, ConsumerAppRepository>();
            services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));

            //Register Mapper
            services.AddSingleton<IMapper>(sp => _mapperConfiguration.CreateMapper());

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddDbContext<PromactOauthDbContext>(options =>
                options.UseInMemoryDatabase());
             serviceProvider = services.BuildServiceProvider();
            
            
        }
    }
}