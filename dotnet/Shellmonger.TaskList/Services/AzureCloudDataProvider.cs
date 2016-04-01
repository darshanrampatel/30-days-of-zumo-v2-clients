﻿using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shellmonger.TaskList.Services
{
    /// <summary>
    /// Concrete implementation of the ICloudDataProvider
    /// that uses Azure Mobile Apps
    /// </summary>
    public class AzureCloudDataProvider : ICloudDataProvider
    {
        static string AppBaseUri = "https://30-days-of-zumo-v2.azurewebsites.net";
        static MobileServiceClient _client = null;

        /// <summary>
        /// Debug statement output for current user
        /// </summary>
        /// <param name="op">operation</param>
        /// <param name="c">client we are using</param>
        private void WriteDebugClientUser(string op, IMobileServiceClient c)
        {
            if (c.CurrentUser != null)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("AzureCloutDataProvider.{0}: CurrentUser = {1}", op, c.CurrentUser.UserId));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(String.Format("AzureCloutDataProvider.{0}: CurrentUser = null", op));
            }
        }

        /// <summary>
        /// Singleton Pattern - create a new MobileServiceClient
        /// </summary>
        /// <returns></returns>
        private static MobileServiceClient GetClient()
        {
            if (_client == null)
            {
                _client = new MobileServiceClient(AppBaseUri);
            }
            return _client;
        }

        /// <summary>
        /// Login to the service
        /// </summary>
        public async Task LoginAsync()
        {
            var client = AzureCloudDataProvider.GetClient();
            try
            {
                WriteDebugClientUser("LoginAsync", client);
                await client.LoginAsync("aad");
            }
            catch (InvalidOperationException exception)
            {
                throw new CloudAuthenticationException("Login Failed", exception);
            }
        }

        /// <summary>
        /// Logout of the service
        /// </summary>
        public async Task LogoutAsync()
        {
            var client = AzureCloudDataProvider.GetClient();
            try
            {
                WriteDebugClientUser("LogoutAsync", client);
                await client.LogoutAsync();
            }
            catch (InvalidOperationException exception)
            {
                throw new CloudAuthenticationException("Logout Failed", exception);
            }
        }

        public void Trace(string className, string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("TRACE:{0}:{1}", className, message));
        }
    }
}
