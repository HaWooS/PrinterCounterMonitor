using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace Aplikacja.Parsers.Starachowice
{
    class CanonIR2200parser
    {
        public string PrinterResponse { get; set; }
        public List<String> ParsePrinterResponseCounter()
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            List<string> elements = new List<string>();
            if (PrinterResponse != null)
            {
                htmlDocument.LoadHtml(PrinterResponse);
                if (htmlDocument.DocumentNode.SelectNodes("//script[contains(text(), '102')]") != null)
                {
                    var readHtml = htmlDocument.DocumentNode.SelectNodes("//script[contains(text(), '102')]");
                    string[] numbers = { "0" };
                    int nodeElement = 0;
                    try
                    {
                        numbers = Regex.Split(readHtml.ElementAt(3).InnerText, @"\D+");
                    }
                    catch (Exception e)
                    {
                        Console.Write("Error during regex split operation in ir2200 parser");
                    }

                    try
                    {
                        foreach (string value in numbers)
                        {
                            //Console.Write(" NODE " + nodeElement + "   " + value);
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (nodeElement == 2)
                                {
                                    int i = int.Parse(value);
                                    elements.Add(i.ToString());
                                }
                            }

                            nodeElement++;
                        }
                        return elements;
                    }
                    catch (Exception e)
                    {
                        elements.Add(1.ToString());
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
                elements.Add(0.ToString());
                return elements;
            }

        }
    }
}
