
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PunkDemo.Filters;
using PunkDemo.Models;
using PunkDemo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace PunkDemo.Controllers
{
    public class DemoController : ApiController
    {
        private string api = "https://api.punkapi.com/v2";

        [HttpGet]
        public string Index()
        {
            return "Hello devs";
        }


        [HttpPost]
        [ActionName("rate")]
        [ValidationFilter]
        public async Task<HttpResponseMessage> RateBeer(int id, [FromBody] UserRating rating)
        {
            try
            {
                if(rating == null || rating.Rating < 0 || rating.Rating > 5)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Rating is Invalid");
                }

                //validate Id
                var result = await WebRequestService.GetRequest($"{api}/beers/{id}");
                if(result.Item1 == HttpStatusCode.OK && !string.IsNullOrEmpty(result.Item2))
                {
                    JArray list = JArray.Parse(result.Item2);
                    if(list.Count < 1)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadGateway, "Beer doesn't exists");
                    }

                    PunkService.SaveRating(id, rating);
                    return Request.CreateResponse(HttpStatusCode.OK, "Ratings saved");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Beer Id");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ActionName("search")]
        public async Task<HttpResponseMessage> GetRatings(string query)
        {
            if(string.IsNullOrEmpty(query))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Search query is missing");
            }

            try
            {
                (HttpStatusCode Status, string Data) result = await WebRequestService.GetRequest($"{api}/beers?beer_name={query}");
                if(result.Status != HttpStatusCode.OK || string.IsNullOrEmpty(result.Data))
                {
                    return Request.CreateResponse(HttpStatusCode.BadGateway, "Proxy server did not return a valid response");
                }

                ICollection<BeerItem> beerList = PunkService.GetRating(result.Data);
                if (beerList == null || beerList.Count < 1) {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Search query did not match any beers");
                }

                return Request.CreateResponse(HttpStatusCode.OK, beerList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}