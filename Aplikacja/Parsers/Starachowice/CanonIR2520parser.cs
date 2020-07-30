using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Aplikacja.Parsers.Starachowice
{
    class CanonIR2520parser
    {
        public string PrinterResponse { get; set; }

        public CanonIR2520parser() {
            Console.Write("Parser created");
            this.PrinterResponse = "";
        }
        public List<String> ParsePrinterResponseCounter()
        {
            Console.Write("WCHODZI ");
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            List<string> elements = new List<string>();
            if (PrinterResponse != null)
            {
                htmlDocument.LoadHtml(PrinterResponse);
                if (htmlDocument.GetElementbyId("mainCounterInformationModule") != null)
                {
                    var headlineText = htmlDocument.GetElementbyId("mainCounterInformationModule");
                    string[] numbers = Regex.Split(headlineText.InnerText, @"\D+");
                    int nodeElement = 0;
                    foreach (var node in numbers)
                    {
                        if (nodeElement == 3)
                        {
                            elements.Add(node);
                        }
                        nodeElement++;
                    }


                    return elements;
                }
                else
                {
                    elements.Add(0.ToString());
                    elements.Add(0.ToString());
                    return elements;
                }
            }
            else
            {
                elements.Add(0.ToString());
                elements.Add(0.ToString());
                return elements;
            }

        }
    }
}
