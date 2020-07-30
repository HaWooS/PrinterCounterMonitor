using Aplikacja.Connectors;
using Aplikacja.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplikacja.Printers
{
    class Printer : IPrinter
    {
        public string Name { get; set; }
        public string Serial { get; set; }
        public string Address { get; set; }
        public int BlackCounter { get; set; }
        public int ColorCounter { get; set; }
        [JsonIgnore]
        public string ErrorDescrption { get; set; }
        [JsonIgnore]
        public PrinterHttpConnector PrinterHttpConnector{ get; set; }
        public int ConnectionTypeNumberMethod { get; set; }
        public Uri ConnectionUri { get; set; }
        public Uri PrinterCounterUri { get; set; }
        public dynamic PrinterResponseParser { get; set; }

        public Printer(string name, string serial, string address, int blackCounter, int colorCounter,int connectionTypeNumberMethod, string connectionUri, string printerCounterUri)
        {

            this.Name = name;
            this.Serial = serial;
            this.Address = address;
            this.BlackCounter = blackCounter;
            this.ColorCounter = colorCounter;
            this.PrinterResponseParser = ParserFactory.GetNewPrinterParser(name);
            this.PrinterHttpConnector = new PrinterHttpConnector(address, PrinterResponseParser);
            this.ConnectionTypeNumberMethod = connectionTypeNumberMethod;
            this.PrinterCounterUri = new Uri(printerCounterUri);
            this.ConnectionUri = new Uri(connectionUri);
            this.ErrorDescrption = "";
           
        }
        public Printer() { }

        public int checkCounter()
        {
            var conn = this.ConnectionTypeNumberMethod;
            switch (conn) {
                //Drukarki czarne
                case 1:
                    PrinterHttpConnector.GetCountersFromPrinter(PrinterCounterUri);
                    var unparsedCountersOption1 = PrinterHttpConnector.GetParsedCounterResponse();
                    this.BlackCounter = Convert.ToInt32(unparsedCountersOption1.ElementAt(0));
                    break;
                //Drukarki kolorowe ze skanerem
                case 2:
                    PrinterHttpConnector.GetCountersFromPrinter(PrinterCounterUri);
                    var unparsedCountersOption4 = PrinterHttpConnector.GetParsedCounterResponse();
                    this.BlackCounter = Convert.ToInt32(unparsedCountersOption4.ElementAt(1));
                    this.ColorCounter = Convert.ToInt32(unparsedCountersOption4.ElementAt(0));
                    break;
            }




            return this.BlackCounter;
        }

        public async System.Threading.Tasks.Task setConnectionAsync()
        {
            var conn = this.ConnectionTypeNumberMethod;
            switch (conn)
            {
                case 1: try { await PrinterHttpConnector.ConnectAndEstabilishSession(this.ConnectionUri); }
                    catch (Exception E) { ErrorConnectionHandler(); }
                    break;
                    //implementuj tutaj drugą metodę
                case 2:
                    try { await PrinterHttpConnector.ConnectAndEstabilishSession(this.ConnectionUri); }
                    catch (Exception E) { ErrorConnectionHandler(); }
                    break;
            }
            
        }

        public void ErrorConnectionHandler()
        {
            this.ErrorDescrption = "Error during connection";
        }

       
        void IPrinter.setConnectionAsync()
        {
            throw new NotImplementedException();
        }

        void IPrinter.checkCounter()
        {
            throw new NotImplementedException();
        }
    }
}
