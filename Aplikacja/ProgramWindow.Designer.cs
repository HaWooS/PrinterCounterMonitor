using Aplikacja.Printers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Aplikacja.Display;
using System.Data;
using OfficeOpenXml;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace Aplikacja
{
   
    partial class ProgramWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private List<Printer> printersToDeserialize;
        private List<Printer> lop;
        private DataGridView dataGridView;
        private List<PrinterDisplayObject> printerListToDisplay;
        private DataSet dataSet;
        private SaveFileDialog saveFileDialog;
        //buttons
        private Button exportToExcelButton;
        private Button refreshCountersButton;
        private Button addNewPrinterButton;
        private Button confirmation;
        private Button rejection;
        private Button deletePrinterButton;
        private Button saveAndExitButton;
        //timer
        private Timer timer;
        //removing printer
        private int IndexOfPrinterToRemove { get; set; }
        //printer creator
        PrinterCreator bpf;
        //background worker
        private BackgroundWorker backgroundWorker { get; set; }
        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private async void InitializeComponent()
        {
            exportToExcelButton = new Button();
            exportToExcelButton.Location = new Point(670, 100);
            exportToExcelButton.Width = 100;
            exportToExcelButton.Height = 40;
            exportToExcelButton.Text = "Eksportuj do Excela";
            exportToExcelButton.Click += ExportToExcel;

            refreshCountersButton = new Button();
            refreshCountersButton.Location = new Point(670, 150);
            refreshCountersButton.Width = 100;
            refreshCountersButton.Height = 40;
            refreshCountersButton.Text = "Pobierz liczniki";
            refreshCountersButton.Click += CheckCountersFromButton;

            addNewPrinterButton = new Button();
            addNewPrinterButton.Location = new Point(670, 200);
            addNewPrinterButton.Width = 100;
            addNewPrinterButton.Height = 40;
            addNewPrinterButton.Text = "Dodaj drukarkę";
            addNewPrinterButton.Click += AddNewPrinterDialog;

            deletePrinterButton = new Button();
            deletePrinterButton.Location = new Point(670, 250);
            deletePrinterButton.Width = 100;
            deletePrinterButton.Height = 40;
            deletePrinterButton.Text = "Skasuj";
            deletePrinterButton.Click += deleteExistingPrinter;

            saveAndExitButton = new Button();
            saveAndExitButton.Location = new Point(670, 400);
            saveAndExitButton.Width = 100;
            saveAndExitButton.Height = 40;
            saveAndExitButton.Text = "Zapisz zmiany i wyjdź";
            saveAndExitButton.Click += SaveAndExit;

 
            timer = new Timer();
            timer.Interval = (3000);
            timer.Tick += new EventHandler(RefreshObjects);
            timer.Start();


            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;


            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Kontroler Liczników Drukarek";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.dataGridView = new DataGridView();
            dataGridView.Width = 650;
            dataGridView.Height = 500;

            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.FileName = "";
            this.saveFileDialog.Filter = "Excel |*.xlsx";


            ListBox listOfPrinters = new ListBox();
            listOfPrinters = new System.Windows.Forms.ListBox();
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "Stany liczników";
            this.ResumeLayout(false);
            this.SuspendLayout();

            bpf = new PrinterCreator();
            this.lop = new List<Printer>();
            this.printersToDeserialize = new List<Printer>();
            //deserialize
            var fileStream = File.ReadAllText("Printers.json");
            printersToDeserialize = JsonConvert.DeserializeObject<List<Printer>>(fileStream);
            foreach (var printerToDeserialize in printersToDeserialize)
            {
                lop.Add((Printer)bpf.GetPrinter(printerToDeserialize.Name, printerToDeserialize.Serial, printerToDeserialize.Address, 0, 0, printerToDeserialize.ConnectionTypeNumberMethod, printerToDeserialize.ConnectionUri.ToString(), printerToDeserialize.PrinterCounterUri.ToString()));
            }
       
            dataSet = new DataSet();
            printerListToDisplay = new List<PrinterDisplayObject>();
            dataGridView.DataSource = printerListToDisplay;
       

            foreach (var printer in lop)
            {
                printerListToDisplay.Add(new PrinterDisplayObject(printer.Name,printer.Serial,printer.Address,printer.BlackCounter,printer.ColorCounter, printer.ErrorDescrption));
            }

            dataGridView.CellContentClick += dataGridView_CellContentClick;
            this.Controls.Add(dataGridView);
            this.Controls.Add(exportToExcelButton);
            this.Controls.Add(refreshCountersButton);
            this.Controls.Add(addNewPrinterButton);
            this.Controls.Add(deletePrinterButton);
            this.Controls.Add(saveAndExitButton);

            CheckCounters();

        
        }


        private void SaveAndExit(object sender, EventArgs e)
        {
            var JSONresultDeserialize = JsonConvert.SerializeObject(this.lop);
            File.WriteAllText("Printers.json", JSONresultDeserialize);
            this.Close();
        }

        private void deleteExistingPrinter(object sender, EventArgs e)
        {
            this.lop.RemoveAt(this.IndexOfPrinterToRemove);
         
            this.printerListToDisplay.Clear();
            foreach (var printer in lop)
            {
                printerListToDisplay.Add(new PrinterDisplayObject(printer.Name, printer.Serial, printer.Address, printer.BlackCounter, printer.ColorCounter, printer.ErrorDescrption));
            }
            this.Controls.Remove(dataGridView);
            this.Controls.Add(dataGridView);

            this.dataGridView.Update();
            this.dataGridView.Refresh();

         

        }

        public void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                DataGridViewRow row = this.dataGridView.Rows[e.RowIndex];
                Console.Write(row.ToString());
                this.IndexOfPrinterToRemove = e.RowIndex;
        }
    
        public void RefreshObjects(object sender, EventArgs e)
        {
            this.printerListToDisplay.Clear();
            foreach (var printer in lop)
            {
                printerListToDisplay.Add(new PrinterDisplayObject(printer.Name, printer.Serial, printer.Address, printer.BlackCounter, printer.ColorCounter, printer.ErrorDescrption));
            }
            this.dataGridView.Update();
            this.dataGridView.Refresh();
        }

        private async void ExportToExcel(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            System.Data.DataTable excelFile = new System.Data.DataTable();
            var table = CreateTableForExcelDocument();
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Worksheet1");
                    var excelWorksheet = excel.Workbook.Worksheets["Worksheet1"];

                    var headerRow = new List<string[]>()
                     {
                       new string[] { "Kopiarka", "Nr seryjny", "Czarny", "Kolor", "Adres IP" }
                     };
                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                    excelWorksheet.Cells[2, 1].LoadFromDataTable(table,false,default);
                    FileInfo file = new FileInfo(this.saveFileDialog.FileName.ToString());
                    excel.SaveAs(file);
                }
                MessageBox.Show("Pomyślnie zapisano plik");
            }
        }

        public System.Data.DataTable CreateTableForExcelDocument()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("Kopiarka", typeof(string));
            table.Columns.Add("Numer", typeof(string));
            table.Columns.Add("Czarny", typeof(int));
            table.Columns.Add("Kolor", typeof(int));
            table.Columns.Add("Adres IP", typeof(string));

            foreach (var printer in printerListToDisplay)
            {
                table.Rows.Add(printer.Name,printer.Serial,printer.BlackCounter,printer.ColorCounter,printer.Address);
            }
          
              
            dataSet.Tables.Add(table);
            return table;
        }

        public void CheckCountersFromButton(object sender, EventArgs e)
        {
            RefreshObjects(sender, e);
            Parallel.ForEach(lop, async printer =>
            {
                await printer.setConnectionAsync();
                printer.checkCounter();
                
            });
        }
        public void CheckCounters()
        {
   
            
            Parallel.ForEach(lop, async printer =>
            {
                await printer.setConnectionAsync();
                printer.checkCounter();
           
            });

            
        }

        public void AddNewPrinterDialog(object sender, EventArgs e)
        {
            this.addNewPrinterButton.Visible = false;
            this.exportToExcelButton.Visible = false;
            this.refreshCountersButton.Visible = false;
            this.deletePrinterButton.Visible = false;
            this.saveAndExitButton.Visible = false;
            Label modelLabel = new Label() { Left = 420, Top = 20, Text = "Model" };
            TextBox modelBox = new TextBox() { Left = 250, Top = 50, Width = 400 };

            Label serialLabel = new Label() { Left = 420, Top = 90, Text = "Nr seryjny" };
            TextBox serialBox = new TextBox() { Left = 250, Top = 120, Width = 400 };

            Label ipLabel = new Label() { Left = 420, Top = 160, Text = "Adres IP" };
            TextBox ipBox = new TextBox() { Left = 250, Top = 190, Width = 400 };

            Label connectionOptionLabel = new Label() { Left = 410, Top = 230, Text = "Opcja polaczenia" };
            NumericUpDown connectionOption = new NumericUpDown() { Left = 400, Top = 260 };
            connectionOption.Maximum = 5;
            connectionOption.Minimum = 1;

            Label loginUrlLabel = new Label() { Left = 420, Top = 300, Text = "Login URL" };
            TextBox loginUrlBox = new TextBox() { Left = 250, Top = 330, Width = 400 };

            Label counterUrlLabel = new Label() { Left = 420, Top = 370, Text = "URL licznika" };
            TextBox counterUrlBox = new TextBox() { Left = 250, Top = 400, Width = 400 };
            

            this.confirmation = new Button() { Text = "Dodaj", Left = 400, Width = 100, Top = 430, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => {
                this.dataGridView.Visible = true;
                this.exportToExcelButton.Visible = true;
                this.refreshCountersButton.Visible = true;
                this.addNewPrinterButton.Visible = true;
                this.deletePrinterButton.Visible = true;
                this.saveAndExitButton.Visible = true;
      

                var modelText = modelBox.Text;
                var serialText = serialBox.Text;
                var ipText = ipBox.Text;
                var connectionOptionChoice = (int)connectionOption.Value;
                var loginUrlText = loginUrlBox.Text;
                var counterUrlText = counterUrlBox.Text;

                addNewPrinter(modelText, serialText, ipText, connectionOptionChoice, loginUrlText, counterUrlText);
                modelBox.Visible = false;
                modelLabel.Visible = false;
    
                serialBox.Visible = false;
                serialLabel.Visible = false;
            
                ipBox.Visible = false;
                ipLabel.Visible = false;
       
                connectionOption.Visible = false;
              
                loginUrlLabel.Visible = false;
                loginUrlBox.Visible = false;
         
                counterUrlLabel.Visible = false;
                counterUrlBox.Visible = false;

                connectionOption.Visible = false;
                connectionOptionLabel.Visible = false;
                };

            this.rejection = new Button() { Text = "Wstecz", Left = 400, Width = 100, Top = 460, DialogResult = DialogResult.OK };
            rejection.Click += (sender, e) => {
                this.dataGridView.Visible = true;
                this.exportToExcelButton.Visible = true;
                this.refreshCountersButton.Visible = true;
                this.addNewPrinterButton.Visible = true;
            };
            //model 
            modelBox.Visible = true;
            modelLabel.Visible = true;
            this.Controls.Add(modelBox);
            this.Controls.Add(modelLabel);
            //Nr seryjny
            serialBox.Visible = true;
            serialLabel.Visible = true;
            this.Controls.Add(serialBox);
            this.Controls.Add(serialLabel);
            //Adres IP
            ipBox.Visible = true;
            ipLabel.Visible = true;
            this.Controls.Add(ipBox);
            this.Controls.Add(ipLabel);
            //Opcja polaczenia
            connectionOption.Visible = true;
            this.Controls.Add(connectionOption);
            this.Controls.Add(connectionOptionLabel);
            //Url loginu
            loginUrlLabel.Visible = true;
            loginUrlBox.Visible = true;
            this.Controls.Add(loginUrlLabel);
            this.Controls.Add(loginUrlBox);
            //Url licznikow
            counterUrlLabel.Visible = true;
            counterUrlBox.Visible = true;
            this.Controls.Add(counterUrlLabel);
            this.Controls.Add(counterUrlBox);


            this.Controls.Add(confirmation);
            this.Controls.Add(rejection);
            this.AcceptButton = confirmation;
            this.dataGridView.Visible = false;
        }

        public void addNewPrinter(string modelText, string serialText, string ipText, int connectionOptionChoice, string connectionUrl, string counterUrl)
        {
            this.lop.Add((Printer)bpf.GetPrinter(modelText, serialText, ipText, 0, 0, connectionOptionChoice, connectionUrl, counterUrl));
            this.printerListToDisplay.Clear();
            foreach (var printer in lop)
            {
                printerListToDisplay.Add(new PrinterDisplayObject(printer.Name, printer.Serial, printer.Address, printer.BlackCounter, printer.ColorCounter, printer.ErrorDescrption));
            }
            this.Controls.Remove(dataGridView);
            this.Controls.Add(dataGridView);

            this.dataGridView.Update();
            this.dataGridView.Refresh();
       
            this.Controls.Remove(confirmation);
            this.Controls.Remove(rejection);


        }

    }
}

