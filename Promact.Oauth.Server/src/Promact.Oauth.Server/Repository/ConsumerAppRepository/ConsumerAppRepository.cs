﻿using AutoMapper;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.StringLiterals;
using Microsoft.Extensions.Options;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppRepository
    {
        #region "Private Variable(s)"

        private readonly IDataRepository<ConsumerApps> _appsDataRepository;
        private readonly IMapper _mapperContext;
        private readonly StringLiteral _stringLiterals;
        #endregion

        #region "Constructor"
        public ConsumerAppRepository(IDataRepository<ConsumerApps> appsDataRepository, IMapper mapperContext
            , IOptionsMonitor<StringLiteral> stringLiterals)
        {
            _appsDataRepository = appsDataRepository;
            _mapperContext = mapperContext;
            _stringLiterals = stringLiterals.CurrentValue;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId">passed client Id</param>
        /// <returns>Consumer App object</returns>
        public async Task<ConsumerApps> GetAppDetailsAsync(string clientId)
        {
            return await _appsDataRepository.FirstAsync(x => x.AuthId == clientId);
        }

        /// <summary>
        /// This method used for added consumer app and return consumerApps Id. -An
        /// </summary>
        /// <param name="consumerApps">consumerApp object</param>
        /// <returns>consumerApp Id</returns>
        public async Task<int> AddConsumerAppsAsync(ConsumerAppsAc consumerApps)
        {
            if (await _appsDataRepository.FirstOrDefaultAsync(x => x.Name == consumerApps.Name) == null)
            {
                var consumerAppObject = _mapperContext.Map<ConsumerAppsAc, ConsumerApps>(consumerApps);
                consumerAppObject.AuthId = GetRandomString(true);
                consumerAppObject.AuthSecret = GetRandomString(false);
                consumerAppObject.CreatedDateTime = DateTime.Now;
                _appsDataRepository.AddAsync(consumerAppObject);
                await _appsDataRepository.SaveChangesAsync();
                return consumerAppObject.Id;
            }
            else
                throw new ConsumerAppNameIsAlreadyExists();
        }


        /// <summary>
        /// This method used for get list of apps. -An
        /// </summary>
        /// <returns>list of ConsumerApps</returns>
        public async Task<List<ConsumerApps>> GetListOfConsumerAppsAsync()
        {
           return await _appsDataRepository.GetAll().ToListAsync(); 
        }

        /// <summary>
        /// This method used for get consumer app object by id. -An
        /// </summary>
        /// <param name="id">ConsumerApp Id</param>
        /// <returns>ConsumerApp Object</returns>
        public async Task<ConsumerApps> GetConsumerAppByIdAsync(int id)
        {
            ConsumerApps consumerApps = await _appsDataRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (consumerApps != null)
                return consumerApps;
            else
                throw new ConsumerAppNotFound(); 
            
        }


        /// <summary>
        /// This method used for update consumer app and return consumerApp Id. -An
        /// </summary>
        /// <param name="consumerApps">passed consumerApp Object</param>
        /// <returns>consumerApp Id</returns>
        public async Task<int> UpdateConsumerAppsAsync(ConsumerApps consumerApps)
        {

            if (await _appsDataRepository.FirstOrDefaultAsync(x => x.Name == consumerApps.Name && x.Id != consumerApps.Id) == null)
            {
                _appsDataRepository.UpdateAsync(consumerApps);
                await _appsDataRepository.SaveChangesAsync();
                return consumerApps.Id;
            }
            throw new ConsumerAppNameIsAlreadyExists();
        }

        #endregion

        #region "Private Method(s)"

        /// <summary>
        /// This method used for get random number For AuthId and Auth Secreate. -An
        /// </summary>
        /// <param name="isAuthId">isAuthId = true (get random number for auth id.) and 
        /// isAuthId = false (get random number for auth secreate)</param>
        /// <returns>random string</returns>
        private string GetRandomString(bool isAuthId)
        {
            var random = new Random();
            if (isAuthId)
            {
                return new string(Enumerable.Repeat(_stringLiterals.ConsumerApp.CapitalAlphaNumericString, 15)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(_stringLiterals.ConsumerApp.AlphaNumericString, 30)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }

        }
        #endregion
    }
}
