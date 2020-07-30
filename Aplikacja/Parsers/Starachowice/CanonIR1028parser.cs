using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Aplikacja.Parsers.Starachowice
{
    class CanonIR1028parser
    {
        public string PrinterResponse { get; set; }

        public List<String> ParsePrinterResponseCounter()
        {
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
                        if ((nodeElement == 9) || (nodeElement == 6))
                        {
                            elements.Add(node);
                        }
                        nodeElement++;
                    }
                }
                else
                {
                    elements.Add(0.ToString());
                    elements.Add(0.ToString());
                    return elements;
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
    }
}
