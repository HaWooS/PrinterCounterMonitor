using Aplikacja.Parsers;
using Aplikacja.Parsers.Starachowice;
using Aplikacja.Printers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aplikacja.Connectors
{
    class PrinterHttpConnector
    {
        public List<KeyValuePair<string, string>> PostData { get; set; }
        public HttpContent Content { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public HttpClientHandler HttpClientHandler { get; set; }
        public HttpClient Client { get; set; }
        IEnumerable<Cookie> ResponseCookies { get; set; }
        public Cookie SessionCookie { get; set; }
        public string PrinterResponse { get; set; }
        public string Address { get; set; }
        public dynamic PrinterResponseParser { get; set; }

        public PrinterHttpConnector(string address, dynamic printerResponseParser)
        {
            this.PostData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("sampleparameter", "0"),
            new KeyValuePair<string, string>("sampleparameter", "0")
        };
            this.Address = address;
            this.Content = new FormUrlEncodedContent(PostData);
            this.CookieContainer = new CookieContainer();
            this.HttpClientHandler = new HttpClientHandler();
            this.HttpClientHandler.CookieContainer = CookieContainer;
            this.Client = new HttpClient(this.HttpClientHandler);
            this.SessionCookie = new Cookie();
            this.PrinterResponseParser = printerResponseParser;

        }

        public async System.Threading.Tasks.Task ConnectAndEstabilishSession(Uri connectionUri) 
        {
            try
            {
                HttpResponseMessage response = await Client.PostAsync(connectionUri, this.Content);
                this.ResponseCookies = CookieContainer.GetCookies(connectionUri).Cast<Cookie>();
                this.SessionCookie = ResponseCookies.ElementAt(0);
            }
            catch
            {
                throw new System.Net.Http.HttpRequestException(
                    "Printer is not reachable " + connectionUri);
            }
        }

        public void GetCountersFromPrinter(Uri printerCounterUri) 
        {
            try
            {
                Task.Delay(1000);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(printerCounterUri);
                myRequest.Method = "GET";
                myRequest.Headers.Add(HttpRequestHeader.Cookie, SessionCookie.ToString());
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
               // this.PrinterResponse = sr.ReadToEnd();
                this.PrinterResponseParser.PrinterResponse = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Couldn't get response from printer " + printerCounterUri);
            }

        }


        public List<String> GetParsedCounterResponse()
        {
            List<string> parsedNodes = new List<String>();
            parsedNodes = PrinterResponseParser.ParsePrinterResponseCounter();
            return parsedNodes;
        }
    }
}
