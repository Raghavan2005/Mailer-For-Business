using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExcelDataReader;
using ClosedXML.Excel;
using Microsoft.VisualBasic.FileIO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.NetworkInformation;


namespace Mailer_For_Business.Windows.Dash
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        string files;
     //  int totolfileselected = 0;
        int Totolrowcount = 1;
        int Totalcolumncount = 1;
        DataSet csvDataSet, xlsxDataSet;
        String typestate ="csv";
        public Dashboard()
        {
            InitializeComponent();
            filetype.SelectedIndex = 0;
            if ((bool)autofiletype.IsChecked)
            {
                filetype.IsEnabled = false;
            }



        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
            else {
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                WindowState = WindowState.Maximized;
                    
                    };
        }
        //window states
        


        //
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_MouseEnter(object sender, MouseEventArgs e)
        {

            btnClose.Foreground = Brushes.White;
        }

        //
        private void Imagesettings_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Imagehelp_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        //
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            // Handle file drop
            try {
                string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Read only the first file
                string filePath = filename.FirstOrDefault();
               
                // Check if a file path is available
                if (!string.IsNullOrEmpty(filePath))
                {
                    // Process the file (e.g., read data from CSV or XLSX file)
                    files = filePath;
                    //xlsxDataSet = ReadXlsxFile(xlsxFilePath);

                    // Check file extension
                    string extension = System.IO.Path.GetExtension(files).ToLower();
                if (extension == ".csv" || extension == ".xlsx")
                {
                    messageBox.Settext("Information", "File was Loaded as >> " + files);
                   
                    bool? result = messageBox.ShowDialog();
                        // Process each dropped file
                        // MessageBox.Show("Dropped file: " + file);
                        //  totolfileselected++;
                        if (extension == ".csv")
                        {//true
                         // totolfileselected++;
                            typestate = "csv";
                            CountRows(files);
                            //statustext.Text = "Total Rows : " + Totolrowcount.ToString() + " || " + "Total Title Found : " + Totalcolumncount.ToString();
                            selectstateupdate();
                            AutoLoad();
                            textupdateui();
                        }
                        else
                        {//false
                         // totolfileselected++;
                            typestate = "xlsx";
                            CountRows(files);
                            //  statustext.Text = "Total Rows : " + Totolrowcount.ToString() + " || " + "Total Title Found : " + Totalcolumncount.ToString();
                            selectstateupdate();
                            AutoLoad();
                            textupdateui();

                        }
                    }
                else
                {
                   
                    //  MessageBox.Show("Unsupported file format. Please drop only CSV or XLSX files.");
                    messageBox.Settext("Unsupported file format", "Please drop only CSV or XLSX files. >>" + filename);
                    messageBox.ShowDialog();

                }
                }

            } catch
            {
                messageBox.Settext("Error", "sdfsdfs>" + files);
                messageBox.ShowDialog();
            }
            
           
            
            
        }


        private void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // Allow selecting multiple files
            CustomMessageBox messageBox = new CustomMessageBox();
            // Set filter for file extension
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx";

            bool? result = openFileDialog.ShowDialog();

            // Process selected files
            if (result == true)
            {
                  files = openFileDialog.FileName;
                messageBox.Settext("Information", "File was Loaded as >> " + files);
                messageBox.ShowDialog();
              
                    string extension = System.IO.Path.GetExtension(files).ToLower();
                if (extension == ".csv")
                {//true
                 // totolfileselected++;
                    typestate = "csv";
                    CountRows(files);
                    //statustext.Text = "Total Rows : " + Totolrowcount.ToString() + " || " + "Total Title Found : " + Totalcolumncount.ToString();
                    selectstateupdate();
                    AutoLoad();
                    textupdateui();
                }
                else
                {//false
                 // totolfileselected++;
                    typestate = "xlsx";
                    CountRows(files);
                    //  statustext.Text = "Total Rows : " + Totolrowcount.ToString() + " || " + "Total Title Found : " + Totalcolumncount.ToString();
                    selectstateupdate();
                    AutoLoad();
                    textupdateui();

                }



            }
        }

        private void resetimport_Click(object sender, RoutedEventArgs e)
        {
           Totolrowcount = 0;
          Totalcolumncount = 0;
            //statustext.Text = "Please Select File Format : (.csv //.xlsx)";
            csvDataGrid.ItemsSource =" ";
            textupdateui();
            files = "";
            filetypetxt.Content ="Select The File";
        }

      void textupdateui()
        {
            if (!string.IsNullOrEmpty(files))
            {
                string filenames = System.IO.Path.GetFileNameWithoutExtension(files).ToLower();
                filetypetxt.Content = filenames.ToString() + "."+ typestate.ToLower();
                rowcount.Content = Totolrowcount.ToString();
                columncount.Content = Totalcolumncount.ToString();
                pendingtxt.Content = "0/" + Totolrowcount.ToString();


            }
                
        }


        int CountRows(string filePath)
        {
            int rowCount = 0;
            int columnCount = 0;
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return 0;
            }

            // Determine the file type (CSV or Excel)
            string extension = System.IO.Path.GetExtension(filePath);
            if (extension == ".csv")
            {
                // Read CSV file
                using (StreamReader reader = new StreamReader(filePath))
                {

                    string firstLine = reader.ReadLine();
                    if (firstLine != null)
                    {
                        // Split the first line into fields using the comma as the delimiter
                        string[] fields = firstLine.Split(',');
                        // Count the number of fields, which corresponds to the number of columns
                        columnCount = fields.Length;
                    }
                    //read row
                    while (reader.ReadLine() != null)
                    {
                        rowCount++;
                    }
                    // Read the first line
                  
                  
                }
            }

            else if (extension == ".xlsx")
            {
                try
                {
                    // Open the Excel file
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        // Get the first worksheet
                        var worksheet = workbook.Worksheet(1);
                        rowCount = worksheet.RowsUsed().Count();
                        columnCount = worksheet.ColumnsUsed().Count();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading Excel file: " + ex.Message);
                }

            }
            else
            {
                Console.WriteLine("Unsupported file format.");
            }
            Totalcolumncount = columnCount;
            Totolrowcount = rowCount;
            return rowCount;
        }
   //


        void Updatethestateoftype()
        {

            typestate = filetype.SelectedValue.ToString().ToLower();
           
        }
        void selectstateupdate()
        {
            if (typestate == "csv")
            {
                filetype.SelectedIndex = 0;
                header.Header = "CSV";
            }
            else
            {
                filetype.SelectedIndex = 1;
                header.Header = "XLSX";
            }
        }
        
        //dataset
        private DataSet ReadCsvFile(string filePath)
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    DataTable dataTable = new DataTable();
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (dataTable.Columns.Count == 0)
                        {
                            // Set columns based on the first row
                            foreach (string field in fields)
                            {
                                dataTable.Columns.Add(new DataColumn(field));
                            }
                        }
                        else
                        {
                            // Add rows
                            dataTable.Rows.Add(fields);
                        }
                    }

                    dataSet.Tables.Add(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading CSV file: " + ex.Message);
            }

            return dataSet;
        }

        private void filetype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Updatethestateoftype();
        }

        private void AutoLoad()
        {

            
            try
            {

           
            if (!string.IsNullOrEmpty(files)) { 
                string extension = System.IO.Path.GetExtension(files).ToLower();
            if (extension == ".csv")
            {//true
             
             
                csvDataSet = ReadCsvFile(files);
                csvDataGrid.ItemsSource = csvDataSet.Tables[0].DefaultView;
            }
            else
            {//false
             // totolfileselected++;
             
                xlsxDataSet = ReadXlsxFile(files);
                        csvDataGrid.ItemsSource = xlsxDataSet.Tables[0].DefaultView;

            }
            }
            }
            catch (Exception ep)
            {
                MessageBox.Show("Error reading Loading the file: " + ep.Message);
            }
        }

        private void CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            if ((bool)autofiletype.IsChecked)
            {
                filetype.IsEnabled = false;
            }
            else
            {
                filetype.IsEnabled = true;
            }
        }

        private DataSet ReadXlsxFile(string filePath)
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        DataTable dataTable = new DataTable(worksheet.Name);
                        foreach (var cell in worksheet.FirstRow().Cells())
                        {
                            dataTable.Columns.Add(cell.Value.ToString());
                        }

                        foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                        {
                            var newRow = dataTable.NewRow();
                            for (int i = 0; i < row.Cells().Count(); i++)
                            {
                                newRow[i] = row.Cell(i + 1).Value.ToString();
                            }
                            dataTable.Rows.Add(newRow);
                        }

                        dataSet.Tables.Add(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading XLSX file: " + ex.Message);
            }

            return dataSet;
        }


        //

    }
}
/*
     // Read data from CSV file and save to DataSet
            DataSet csvDataSet = ReadCsvFile(csvFilePath);

            // Read data from XLSX file and save to DataSet
            DataSet xlsxDataSet = ReadXlsxFile(xlsxFilePath);

            // Bind the data to DataGrids
            csvDataGrid.ItemsSource = csvDataSet.Tables[0].DefaultView;
            xlsxDataGrid.ItemsSource = xlsxDataSet.Tables[0].DefaultView;
 
 
 
 
 
 
 
 
 
 */