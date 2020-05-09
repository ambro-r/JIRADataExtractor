using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor
{
    class JIRAConnectionHandler
    {

        private string UserName;
        private string Password;
        private string BaseUrl;
        private JIRAConnectionHandler() { }

        public JIRAConnectionHandler(String userName, String password, String baseURL)
        {
            this.UserName = userName;
            this.Password = password;
            this.BaseUrl = baseURL;
        }

        private string GetEncodedCredentials(string UserName, string Password)
        {
            string mergedCredentials = String.Format("{0}:{1}", UserName, Password);
            byte[] byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        public string execute(string url)
        {
            Log.Debug("Excuting request to {url}", url);
            var client = new RestClient(BaseUrl + url);
            var request = new RestRequest
            {
                Method = Method.GET,
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Authorization", "Basic " + GetEncodedCredentials(UserName, Password));
            var response = client.Execute(request);
            return response.Content;
        }

    }
}
