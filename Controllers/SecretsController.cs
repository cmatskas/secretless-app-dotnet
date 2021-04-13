using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web.Resource;

namespace VolcanoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public SecretsController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet]
        public async Task<List<string>> Get()
        {
            var credential = new ChainedTokenCredential(
                new AzureCliCredential(),
                new ManagedIdentityCredential()
            );
            var keyVaultClient = new SecretClient(new Uri(configuration["KeyVaultUri"]), credential);
            var secretOperation = await keyVaultClient.GetSecretAsync("VerySecretValue");

            var secret = secretOperation.Value.Value;
            var secret2 = configuration["SomeConfigValue"];

            return new List<string>(){secret, secret2};
        }
    }
}