using System;
using System.Collections.Generic;
using System.Text;

namespace Aplikacja.Display
{
    class PrinterDisplayObject
    {
        public string Name { get; set; }
        public string Serial { get; set; }
        public string Address { get; set; }
        public int BlackCounter { get; set; }
        public int ColorCounter { get; set; }

        public string ErrorDescription { get; set; }


        public PrinterDisplayObject(string name, string serial, string address, int blackCounter, int colorCounter, string errorDescription)
        {
            this.Name = name;
            this.Serial = serial;
            this.Address = address;
            this.BlackCounter = blackCounter;
            this.ColorCounter = colorCounter;
            this.ErrorDescription = errorDescription;
        }

    }
    
}
