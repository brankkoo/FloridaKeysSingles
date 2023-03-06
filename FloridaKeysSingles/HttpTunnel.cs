using FloridaKeysSingles;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FloridaKeysSingles
{
    public class HttpTunnel
    {
        WebClientAware _webClient = new WebClientAware();

        public async Task<Response> Login()
        {
            using (var client = new HttpClient())
            {

                string url = "https://www.floridakeyssingles.com/";

                HttpResponseMessage responseMessage = await client.GetAsync(url);



                string html = await responseMessage.Content.ReadAsStringAsync();

                // Parse the HTML content using HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode crsfNode = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/div/div/div[1]/div/div[2]/div/div/div[1]/div/div/div/form/input[2]");
                string crsfValue = crsfNode.Attributes["value"].Value;



                // Create the form data to be posted to the login page
                var formData = new Dictionary<string, string>()
                {
                    {"form_name","sign-in" },
                    {crsfNode.Attributes["name"].Value, crsfValue },
                    {"identity","seven237" },
                    {"password","Raisgod237"},
                    {"remember","on" },


                };


                var content = new FormUrlEncodedContent(formData);
                string moded = await content.ReadAsStringAsync();
                moded += "&";



                client.DefaultRequestHeaders.Referrer = new Uri("https://www.floridakeyssingles.com");
                client.DefaultRequestHeaders.Host = "www.floridakeyssingles.com";
                client.DefaultRequestHeaders.Add("origin", "https://www.floridakeyssingles.com");
                client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                client.DefaultRequestHeaders.Add("sec-fetch-dest", "");
                client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
                client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                client.DefaultRequestHeaders.Add("authority", "www.floridakeyssingles.com");
                // Post the form data to the login page
                HttpResponseMessage response = await client.PostAsync("https://www.floridakeyssingles.com/base/user/ajax-sign-in/", new StringContent(moded, Encoding.UTF8, "application/x-www-form-urlencoded"));

                if (response.IsSuccessStatusCode)
                {

                    string responseContent = await response.Content.ReadAsStringAsync();

                    Response? res = JsonConvert.DeserializeObject<Response>(responseContent);
                    return res;



                }

                else
                {
                    throw new Exception("Login failed");
                }
            }
        }

        public async Task<Response> PostData(string username, string password)
        {
            Response res = new Response();
            try
            {
                string firstContact = await _webClient.DownloadStringTaskAsync("https://www.floridakeyssingles.com/");
                var match = new Regex("name=\"csrf_token\" id=\"([^\"]*?)\" type=\"hidden\" value=\"([^\"]*?)\"").Match(firstContact);
                var crsfValue = match.Groups[2].Value;

                string url = "https://www.floridakeyssingles.com/base/user/ajax-sign-in/";

                _webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=utf-8";
                _webClient.Headers.Add("x-requested-with", "XMLHttpRequest");
                _webClient.Headers[HttpRequestHeader.Referer] = "https://www.floridakeyssingles.com/";
                _webClient.Headers[HttpRequestHeader.Host] = "www.floridakeyssingles.com";
                _webClient.Headers.Add("origin", "https://www.floridakeyssingles.com");
                _webClient.Headers.Add("sec-fetch-mode", "cors");
                _webClient.Headers.Add("sec-fetch-dest", "");
                _webClient.Headers.Add("sec-fetch-site", "same-origin");
                _webClient.Headers.Add("sec-ch-ua-platform", "Windows");
                _webClient.Headers.Add("sec-ch-ua-mobile", "?0");
                _webClient.Headers.Add("authority", "www.floridakeyssingles.com");

                var formData = new Dictionary<string, string>()
                                {
            {"form_name","sign-in" },
            {"csrf_token", crsfValue },
            {"identity",$"{username}" },
            {"password",$"{password}"},
            {"remember","on" },
                                };

                var content = new FormUrlEncodedContent(formData);
                string moded = await content.ReadAsStringAsync();
                moded += "&";
                byte[] byResp = await _webClient.UploadDataTaskAsync(new Uri(url), "POST", Encoding.UTF8.GetBytes(moded));
                string resp = Encoding.UTF8.GetString(byResp);
                if (!string.IsNullOrEmpty(resp))
                {
                    res = JsonConvert.DeserializeObject<Response>(resp);
                    return res;
                }
                else
                {
                    throw new Exception("Login failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }


    }
}

