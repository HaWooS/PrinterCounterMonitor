using System;
using System.Collections.Generic;
using System.Text;

namespace Aplikacja.Printers
{
    abstract class Creator
    {

        public abstract IPrinter GetPrinter(string v1, string v2, string v3, int v4, int v5, int v6, string v7, string v8);

    }
}
