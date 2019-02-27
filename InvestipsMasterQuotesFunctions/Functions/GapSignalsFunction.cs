using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Investips.Data.Models;
using InvestipsMasterQuotesFunctions.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace InvestipsMasterQuotesFunctions.Functions
{
    public static class GapSignalsFunction
    {
        [FunctionName("GapSignalsFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req, TraceWriter log)
        {
            IEnumerable<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(q => q.Symbol == "MED").ToList();
                log.Info("Quote received");
            }

            var marks = new SignalMarks()
            {
                time = new List<long>
                {
                    1520985600,
                    1548633600,
                    1550793600,
                    1533268800,
                    1541566800,
                    1542690000,
                    1545973200,
                    1548133200
                },
                color = new List<string>
                {
                    "green",
                    "green",
                    "green",
                    "green",
                    "green",
                    "green",
                    "green",
                    "green"
                },
                text = new List<string>
                {
                    "Gap",
                    "Gap",
                    "Gap",
                    "Gap",
                    "Gap",
                    "Gap",
                    "3 Green Arrows",
                    "3 Green Arrows"
                },
                label = new List<string>
                {
                    "G",
                    "G",
                    "G",
                    "G",
                    "G",
                    "G",
                    "^",
                    "^"
                },
                labelFontColor = new List<string>
                {
                    "black",
                    "black",
                    "black",
                    "black",
                    "black",
                    "black",
                    "black",
                    "black"
                },
                minSize = new List<int>
                {
                    20,
                    20,
                    20,
                    20,
                    20,
                    20,
                    20,
                    20
                }
            };
            var jsonToReturn = JsonConvert.SerializeObject(marks);

            //return new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent(quotes, Encoding.UTF8, "application/json")
            //};

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
            //return req.CreateResponse(HttpStatusCode.OK, jsonToReturn);
        }

        public class Person : TableEntity
        {
            public string Name { get; set; }
        }
    }
}
