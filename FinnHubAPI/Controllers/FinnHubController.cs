using FinnHubAPI.Services;
using FinnHubAPI.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using System.Web.Script.Serialization;
using System;
using System.Threading.Tasks;

namespace FinnHubAPI.Controllers
{
    [RoutePrefix("api/FinnHub")]
    public class FinnHubController : ApiController
    {
        [HttpGet]
        [Route("test")]
        public async Task<IHttpActionResult> IndexAsync()
        {
            using (FinnHubService svc = new FinnHubService())
            {
                try
                {
                    string response = await svc.RetrieveList();
                    Console.WriteLine(response);

                    QueryList list = new JavaScriptSerializer().Deserialize<QueryList>(response);
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

        [HttpGet]
        [Route("getStockDetails")]
        public IHttpActionResult GetStockDetails(string symbol)
        {
            using (FinnHubService svc = new FinnHubService())
            {
                try
                {
                    var response = svc.RetrieveStockDetails(symbol);
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }
        
        [HttpPost]
        [Route("addHolding")]
        public IHttpActionResult AddHolding()
        {
            return Ok();
        }

        [HttpGet]
        [Route("handleLogin")]
        public IHttpActionResult HandleLogin(string username, string password)
        {
            string userId = "";

            using (FinnHubService svc = new FinnHubService())
            {
                userId = svc.handleLogin(username, password);
            }

            return Ok(userId);
        }
    }
}