using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace Aplikacja.Parsers.Starachowice
{
    class CanonIR2270parser
    {
        public string PrinterResponse { get; set; }
        public List<String> ParsePrinterResponseCounter()
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            List<string> elements = new List<string>();
            if (PrinterResponse != null)
            {
                htmlDocument.LoadHtml(PrinterResponse);
                if (htmlDocument.DocumentNode.SelectNodes("//script[contains(text(), '101')]") != null)
                {
                    var readHtml = htmlDocument.DocumentNode.SelectNodes("//script[contains(text(), '101')]");
                    int nodeElement = 0;
                    string[] numbers = { "0" };
                    try
                    {
                        numbers = Regex.Split(readHtml.ElementAt(3).InnerText, @"\D+");
                    }
                    catch (Exception e)
                    {
                        Console.Write("Error during regex split operation in ir2270 parser");
                    }

                    try
                    {
                        foreach (string value in numbers)
                        {
                            if (nodeElement == 2)
                            {
                                Console.Write(" NODE " + "   " + value);
                                elements.Add(value);
                            }
                            nodeElement++;
                        }
                        return elements;
                    }
                    catch (Exception e)
                    {
                        elements.Add(0.ToString());
                        return elements;
                    }

                }
                else
                {
                    elements.Add(0.ToString());
                    return elements;
                }

            }
            else
            {
                Console.WriteLine("Couldn't get a response from printer");
                elements.Add(0.ToString());
                return elements;
            }

        }
    }
}
