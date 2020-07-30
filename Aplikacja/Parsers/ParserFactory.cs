using Aplikacja.Parsers.Starachowice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplikacja.Parsers
{
    static class ParserFactory
    {

        public static dynamic GetNewPrinterParser(string name)
        {
            if (name.Equals("Canon IR 2520i"))
            {
                return new CanonIR2520parser();
            }
            if (name.Equals("Canon IR 2520i Scan"))
            {
                return new CanonIR2520Scanparser();
            }
            else if (name.Equals("Canon IR 1730i"))
            {
                return new CanonIR1730parser();
            }
            else if (name.Equals("Canon IR 1730i Scan"))
            {
                return new CanonIR1730Scanparser();
            }
            else if (name.Equals("Canon IR 3225"))
            {
                return new CanonIR3225parser();
            }
            else if (name.Equals("Canon IR 3235"))
            {
                return new CanonIR3235parser();
            }
            else if (name.Equals("Canon IR 2200"))
            {
                return new CanonIR2200parser();
            }
            else if (name.Equals("Canon IR 2270"))
            {
                return new CanonIR2270parser();
            }
            else if (name.Equals("Canon IR 1028"))
            {
                return new CanonIR1028parser();
            }
            else if (name.Equals("Canon IR 1028 Scan"))
            {
                return new CanonIR1028Scanparser();
            }
            else
            {
                Console.WriteLine("The name of the printer cannot be assigned to specified parser");
                return null;
            }
        }
    }
}
