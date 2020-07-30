using Aplikacja.Connectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Aplikacja.Printers
{
    class PrinterCreator : Creator
    {
     

        public override IPrinter GetPrinter(string name, string serial, string address, int blackCounter, int colorCounter, int connectionTypeNumberMethod, string connectionUri, string printerCounterUri)
        {
            return new Printer(name, serial, address, blackCounter, colorCounter, connectionTypeNumberMethod, connectionUri, printerCounterUri);
        }
    }
}
