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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FloridaKeysSingles
{
    public class HttpTunnel
    {
        public async Task<Response> Login(string username,string password)
        {
            var handler = new SocketsHttpHandler();


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
                    {"identity",$"{username}" },
                    {"password",$"{password}"},
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
        }
       
        }
    

