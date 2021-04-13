using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace VolcanoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VolcanoController : ControllerBase
    {
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_cosmos_data" };

        [HttpGet]
        public async Task<IEnumerable<Volcano>> Get()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var data = await DataService.GetVolcanos();
            return data.Take(50);
        }
    }
}