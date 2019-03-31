using DataManager.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Middlewares
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _config;

        public BasicAuthenticationMiddleware(RequestDelegate next, IConfiguration iConfig)
        {
            _next = next;
            _config = iConfig;
        }

        public async Task Invoke(HttpContext context, EncryptionHelper encryptionHelper)
        {

            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                string encryptedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                byte[] encryptedUsernamePasswordBytes = Convert.FromBase64String(encryptedUsernamePassword);
                string usernamePassword = encryptionHelper.Decrypt(encryptedUsernamePasswordBytes);

                string[] stringSplits = usernamePassword.Split(':');
                string username = stringSplits[0];
                string password = stringSplits[1];

                if (IsAuthorized(username, password))
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //Unauthorized
                    return;
                }
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }
        }
        public bool IsAuthorized(string username, string password)
        {
            var basicAuthUserName = _config.GetValue<string>("BasicAuth:UserName");
            var basicAuthPassword = _config.GetValue<string>("BasicAuth:Password");
            // Check that username and password are correct
            return username.Equals(basicAuthUserName, StringComparison.InvariantCultureIgnoreCase)
                   && password.Equals(basicAuthPassword);
        }
    }
}
