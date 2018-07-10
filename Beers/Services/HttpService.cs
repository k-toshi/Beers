using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Beers.Models;
using Beers.Controllers;

namespace Beers.Services
{
    public static class HttpService
    {

        private const string BaseUrl = "http://13.230.105.151/";
        //private const string BaseUrl = "http://192.168.100.100:8080/";

        public static string GetBaseUrl() { return BaseUrl; }

        public static async Task<HttpServiceResult<Type>> GetDataFromService<Type>(string queryString)
        {
            HttpServiceResult<Type> hts = new HttpServiceResult<Type>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BaseUrl + queryString);
            SetHttpServiceResult(hts, response);
            return hts;
        }

        public static async Task<HttpServiceResult<Type>> GetDataFromServiceWithToken<Type>(string queryString)
        {
            HttpServiceResult<Type> hts = new HttpServiceResult<Type>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserController.GetLoginUserToken());
            HttpResponseMessage response = await client.GetAsync(BaseUrl + queryString);
            SetHttpServiceResult(hts, response);
            return hts;
        }

        public static async Task<HttpServiceResult<Type>> PostDataFromService<Type>(string queryString, Dictionary<string,string> contents)
        {
            HttpServiceResult<Type> hts = new HttpServiceResult<Type>();
            HttpClient client = new HttpClient();
            var iparams = new FormUrlEncodedContent(contents);
            HttpResponseMessage response = await client.PostAsync(BaseUrl + queryString,iparams);
            SetHttpServiceResult(hts, response);
            return hts;
        }

        public static async Task<HttpServiceResult<Type>> PostDataFromServiceWithToken<Type>(string queryString, Dictionary<string, string> contents)
        {
            HttpServiceResult<Type> hts = new HttpServiceResult<Type>();
            HttpClient client = new HttpClient();
            var iparams = new FormUrlEncodedContent(contents);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserController.GetLoginUserToken());
            HttpResponseMessage response = await client.PostAsync(BaseUrl + queryString,iparams);
            SetHttpServiceResult(hts, response);
            return hts;
        }

        public static async Task<HttpServiceResult<Type>> PutDataFromServiceWithToken<Type>(string queryString, Dictionary<string, string> contents)
        {
            HttpServiceResult<Type> hts = new HttpServiceResult<Type>();
            HttpClient client = new HttpClient();
            var iparams = new FormUrlEncodedContent(contents);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserController.GetLoginUserToken());
            HttpResponseMessage response = await client.PutAsync(BaseUrl + queryString,iparams);
            SetHttpServiceResult(hts, response);
            return hts;
        }

        private static void SetHttpServiceResult<Type>(HttpServiceResult<Type> hts,HttpResponseMessage response)
        {
            if ((response != null) && (response.IsSuccessStatusCode))
            {
                string json = response.Content.ReadAsStringAsync().Result;
                hts.ResultData = JsonConvert.DeserializeObject<Type>(json);
            }
            else
            {
                hts.IsError = true;
                string json = response.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(json);
                if (result["error_description"] != null) hts.ErrorMessage = result["error_description"];
                else if (result["error"] != null) hts.ErrorMessage = result["error"];
                else if (result["ErrorMessage"] != null) hts.ErrorMessage = result["ErrorMessage"];
                else if(result["Message"] != null) hts.ErrorMessage = result["Message"];
                else hts.ErrorMessage = "不明なエラーが発生しました。";
            }
        }
    }
}
