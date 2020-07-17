using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PunkDemo.Services
{
    public static class WebRequestService
    {
        private static WebRequest _request;

        public async static Task<(HttpStatusCode, string)> GetRequest(string url)
        {
            string result = "";
            _request = WebRequest.Create(url);

            _request.Credentials = CredentialCache.DefaultCredentials;
            _request.Headers.Clear();

            // Get the response.  
            WebResponse response = await _request.GetResponseAsync();
            var status = ((HttpWebResponse)response).StatusCode;
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    // Read the content.  
                    result = reader.ReadToEnd();
                }
            }

            response.Close();

            return (status, result);
        }

    }
}