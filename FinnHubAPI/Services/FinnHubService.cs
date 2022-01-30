using FinnHubAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace FinnHubAPI.Services
{
    public class FinnHubService :IDisposable
    {
        public void Dispose() { }

        public async Task<string> RetrieveList()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync("https://finnhub.io/api/v1/search?q=apple&token=c60873qad3ibmuq1bmj0").Result;

                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    return contents;
                }
                else
                {
                    throw new HttpException("Internal Server Error");
                }
            }
        }

        public object RetrieveStockDetails(string symbol)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync("https://finnhub.io/api/v1/quote?symbol=" + symbol + "&token=c60873qad3ibmuq1bmj0").Result;

                if (response.IsSuccessStatusCode)
                {
                    var contents = response.Content.ReadAsStringAsync().Result;
                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject(contents);
                    return json;
                } else
                {
                    throw new HttpException("Internal Server Error");
                }
            }
        }

        public string handleLogin(string username, string password)
        {
            string userId = "";

            using (SqlService svc = new SqlService(Config.SQLConnectionString))
            {

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter("@Username", username, 255, System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input));
                parameters.Add(new DBParameter("@Pass", password, 255, System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input));

                using (SqlDataReader reader = svc.ExecDataReaderSP("dbo.HandleLogin", parameters, 0))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userId = svc.GetSQLStringValue("UserId", reader);
                        }
                    }
                }
            }

            return userId;
        }

        /*
        public void createNewUser(string username, string password)
        {
            using (SqlService svc = new SqlService(Config.SQLConnectionString))
            {
                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter("@Username", username, 255, System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input));
                parameters.Add(new DBParameter("@Pass", password, 255, System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input));
                parameters.Add(new DBParameter("@UserId", Guid.NewGuid().ToString(), 255, System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input));

                using(SqlDataReader reader = svc.ExecDataReaderSP("InsertNewUser", parameters, 0))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userId = svc.GetSQLStringValue("UserId", reader);
                        }
                    }
                }
            }
        }
        */
    }
}