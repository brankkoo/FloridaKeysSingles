using System.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FloridaKeysSingles;

 public class WebClientAware : WebClient
        {

            string _responseUri;
            string _exception;
            public string Exception => _exception;
            public string ResponseUri => _responseUri;



            public CookieContainer CookieContainer = new CookieContainer();
            protected override WebRequest GetWebRequest(Uri address)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls  /*| SecurityProtocolType.Ssl3*/;
                WebRequest request = base.GetWebRequest(address);

                if (request is HttpWebRequest)
                {
                    var hwr = request as HttpWebRequest;
                    hwr.ServicePoint.Expect100Continue = false;
                    hwr.AllowAutoRedirect = false;
                    hwr.CookieContainer = CookieContainer;
                    hwr.ServerCertificateValidationCallback += ValidateServerCertificate;
                    hwr.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    hwr.KeepAlive = false;
                }
                return request;
            }

            public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }



            WebResponse _response;
            protected override WebResponse GetWebResponse(WebRequest request)
            {
                try
                {

                    _response = base.GetWebResponse(request);

                    if (_response is HttpWebResponse)
                    {
                        var resp = _response as HttpWebResponse;
                        _responseUri = resp.Headers["Location"];

                    }
                    return _response;
                }

                catch (WebException ex)
                {
                    _exception = ex.Message;
                }
                
                return _response;
            }




        }
