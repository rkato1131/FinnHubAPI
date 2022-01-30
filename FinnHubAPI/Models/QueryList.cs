using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinnHubAPI.Models
{
    public class Result
    {
        string description { get; set; }
        string displaySymbol { get; set; }
        string symbol { get; set; }
        string type { get; set; }
    }

    public class QueryList
    {
        int count { get; set; }
        List<Result> result { get; set; }
    }
}