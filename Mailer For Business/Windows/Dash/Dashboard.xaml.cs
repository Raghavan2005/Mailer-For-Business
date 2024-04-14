﻿using ClosedXML.Excel;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


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
        String typestate = "csv";
        public Dashboard()
        {
            InitializeComponent();
            filetype.SelectedIndex = 0;
            if ((bool)autofiletype.IsChecked)
            {
                filetype.IsEnabled = false;
            }

            parametercombox.IsEnabled = false;
            cxmTextBox.IsEnabled = false;
            logobox.IsEnabled = false;

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
            else
            {
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

        //
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            // Handle file drop
            try
            {
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

            }
            catch
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
            parametercombox.IsEnabled = false;
            cxmTextBox.IsEnabled = false;
            logobox.IsEnabled = false;
            //statustext.Text = "Please Select File Format : (.csv //.xlsx)";
            csvDataGrid.ItemsSource = " ";
            textupdateui();
            files = "";
            filetypetxt.Content = "Select The File";
        }

        void textupdateui()
        {
            if (!string.IsNullOrEmpty(files))
            {
                string filenames = System.IO.Path.GetFileNameWithoutExtension(files).ToLower();
                filetypetxt.Content = filenames.ToString() + "." + typestate.ToLower();
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

        //
        void Updatethestateoftype()
        {

            typestate = (filetype.SelectedIndex == 0) ? "csv" : "xlsx";


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

        void addparametervalues()
        {
            cxmTextBox.IsEnabled = true;
            logobox.IsEnabled = true;
            parametercombox.IsEnabled = true;
            parametercombox.ItemsSource = null;

            if (typestate == "csv")
            {
                parametercombox.ItemsSource = GetColumnNames(csvDataSet);
                parametercombox.SelectedIndex = 0;
                return;
            }
            else
            {
                parametercombox.ItemsSource = GetColumnNames(xlsxDataSet);
                parametercombox.SelectedIndex = 0;
                return;
            }
        }

        private void AutoLoad()
        {


            try
            {


                if (!string.IsNullOrEmpty(files))
                {
                    string extension = System.IO.Path.GetExtension(files).ToLower();
                    if (extension == ".csv")
                    {//true


                        csvDataSet = ReadCsvFile(files);
                        csvDataGrid.ItemsSource = csvDataSet.Tables[0].DefaultView;
                        addparametervalues();
                    }
                    else
                    {//false
                     // totolfileselected++;

                        xlsxDataSet = ReadXlsxFile(files);
                        csvDataGrid.ItemsSource = xlsxDataSet.Tables[0].DefaultView;
                        addparametervalues();

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

        //textbox
        private void MenuChange(Object sender, RoutedEventArgs ags)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null || cxm == null) return;

            switch (rb.Name)
            {
                case "rbCustom":
                    cxmTextBox.ContextMenu = cxm;
                    break;
                case "rbDefault":
                    // Clearing the value of the ContextMenu property
                    // restores the default TextBox context menu.
                    cxmTextBox.ClearValue(ContextMenuProperty);
                    break;
                case "rbDisabled":
                    // Setting the ContextMenu propety to
                    // null disables the context menu.
                    cxmTextBox.ContextMenu = null;
                    break;
                default:
                    break;
            }
        }

        void ClickPaste(Object sender, RoutedEventArgs args) { cxmTextBox.Paste(); }
        void ClickCopy(Object sender, RoutedEventArgs args) { cxmTextBox.Copy(); }
        void ClickCut(Object sender, RoutedEventArgs args) { cxmTextBox.Cut(); }
        void ClickUndo(Object sender, RoutedEventArgs args) { cxmTextBox.Undo(); }
        void ClickRedo(Object sender, RoutedEventArgs args) { cxmTextBox.Redo(); }

        void ClickSelectLine(Object sender, RoutedEventArgs args)
        {
            int lineIndex = cxmTextBox.GetLineIndexFromCharacterIndex(cxmTextBox.CaretIndex);
            int lineStartingCharIndex = cxmTextBox.GetCharacterIndexFromLineIndex(lineIndex);
            int lineLength = cxmTextBox.GetLineLength(lineIndex);
            cxmTextBox.Select(lineStartingCharIndex, lineLength);
        }

        void AddNewItem(object sender, RoutedEventArgs e)
        {
            // Get the current cursor position
            int cursorPosition = cxmTextBox.CaretIndex;

            // Get the text to insert
            string newText = "((%" + parametercombox.SelectedValue.ToString() + "%))";

            // Insert the text at the cursor position
            cxmTextBox.Text = cxmTextBox.Text.Insert(cursorPosition, newText);

            // Update the cursor position to the end of the inserted text
            cxmTextBox.CaretIndex = cursorPosition + newText.Length;
        }
        public string ReplaceSpaceWithUnderscore(string input)
        {
            // Replace space with underscore
            string output = input.Replace(" ", "_");
            return output;
        }


        void CxmOpened(Object sender, RoutedEventArgs args)
        {
            // Only allow copy/cut if something is selected to copy/cut.
            if (cxmTextBox.SelectedText == "")
                cxmItemCopy.IsEnabled = cxmItemCut.IsEnabled = false;
            else
                cxmItemCopy.IsEnabled = cxmItemCut.IsEnabled = true;

            // Only allow paste if there is text on the clipboard to paste.
            if (Clipboard.ContainsText())
                cxmItemPaste.IsEnabled = true;
            else
                cxmItemPaste.IsEnabled = false;
        }

        private void send_clickbtn(object sender, RoutedEventArgs e)
        {

        }

        private void cxmTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string textBoxValue = cxmTextBox.Text;





            // testtxt.Content = ReplacePatterns( textBoxValue);
        }
        //replay

        string ReplacePatterns(string input)
        {
            // Define the pattern to match ((%name%)) and ((%mail%))
            string pattern = @"\(\(%(.*?)%\)\)";

            // Use regular expression to find patterns and prompt user for replacement text
            string replacedText = Regex.Replace(input, pattern, match =>
            {
                // Extract the key from the match
                string key = match.Groups[1].Value;
                string extension = System.IO.Path.GetExtension(files).ToLower();
                string replacement;
                if (extension == ".csv")
                {
                    replacement = GetValueFromColumn(csvDataSet, key, 0);
                }
                else
                {
                    replacement = GetValueFromColumn(xlsxDataSet, key, 0);
                }

                // Prompt the user for replacement text

                // Return the replacement value
                return replacement;
            });

            return replacedText;
        }

        //
        // Function to get the value of a column from a DataSet using column name and row index
        String GetValueFromColumn(DataSet dataSet, string columnName, int rowIndex)
        {
            // Check if the DataSet is valid and contains tables
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // Get the first DataTable from the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Check if the row index is valid
                if (rowIndex >= 0 && rowIndex < dataTable.Rows.Count)
                {
                    // Get the DataRow at the specified index
                    DataRow targetRow = dataTable.Rows[rowIndex];

                    // Check if the column exists in the DataTable
                    if (dataTable.Columns.Contains(columnName))
                    {
                        // Retrieve the value of the specified column from the DataRow
                        return targetRow[columnName].ToString();
                    }
                    else
                    {
                        // Column does not exist in the DataTable
                        Console.WriteLine($"Column '{columnName}' does not exist in the DataTable.");
                        return "null";
                    }
                }
                else
                {
                    // Invalid row index
                    Console.WriteLine("Invalid row index.");
                    return "null";
                }
            }
            else
            {
                // Invalid DataSet or no tables in the DataSet
                Console.WriteLine("Invalid DataSet or no tables found.");
                return "null";
            }
        }

        private void cxmTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl + B is pressed
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.B)
            {
                // Get the selected text
                string selectedText = cxmTextBox.SelectedText;

                // Check if any text is selected
                if (!string.IsNullOrEmpty(selectedText))
                {
                    // Apply formatting to the selected text
                    string formattedText = $"<br>{selectedText}</br>";

                    // Get the start and end indices of the selection
                    int selectionStart = cxmTextBox.SelectionStart;
                    int selectionLength = cxmTextBox.SelectionLength;

                    // Replace the selected text with the formatted text
                    cxmTextBox.Text = cxmTextBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, formattedText);

                    // Update the selection indices to reflect the formatted text
                    cxmTextBox.Select(selectionStart, formattedText.Length);

                    // Mark the event as handled
                    e.Handled = true;
                }
            }
            // Check if Ctrl + U is pressed
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                // Get the selected text
                string selectedText = cxmTextBox.SelectedText;

                // Check if any text is selected
                if (!string.IsNullOrEmpty(selectedText))
                {
                    // Apply formatting to the selected text
                    string formattedText = $"<u>{selectedText}</u>";

                    // Get the start and end indices of the selection
                    int selectionStart = cxmTextBox.SelectionStart;
                    int selectionLength = cxmTextBox.SelectionLength;

                    // Replace the selected text with the formatted text
                    cxmTextBox.Text = cxmTextBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, formattedText);

                    // Update the selection indices to reflect the formatted text
                    cxmTextBox.Select(selectionStart, formattedText.Length);

                    // Mark the event as handled
                    e.Handled = true;
                }
            }
            // Check if Ctrl + I is pressed
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.I)
            {
                // Get the selected text
                string selectedText = cxmTextBox.SelectedText;

                // Check if any text is selected
                if (!string.IsNullOrEmpty(selectedText))
                {
                    // Apply formatting to the selected text
                    string formattedText = $"<i>{selectedText}</i>";

                    // Get the start and end indices of the selection
                    int selectionStart = cxmTextBox.SelectionStart;
                    int selectionLength = cxmTextBox.SelectionLength;

                    // Replace the selected text with the formatted text
                    cxmTextBox.Text = cxmTextBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, formattedText);

                    // Update the selection indices to reflect the formatted text
                    cxmTextBox.Select(selectionStart, formattedText.Length);

                    // Mark the event as handled
                    e.Handled = true;
                }
            }


        }

        private void logoinsertclick(object sender, RoutedEventArgs e)
        {
            // Open the UrlInputWindow to get the URL
            UrlInputWindow urlInputWindow = new UrlInputWindow();
            if (urlInputWindow.ShowDialog() == true) // If user clicked OK
            {
                string enteredUrl = urlInputWindow.EnteredUrl;

                // Create a BitmapImage object from the URL
                BitmapImage bitmapImage = new BitmapImage(new Uri(enteredUrl));

                // Set the source of the Image control to the BitmapImage
                logoimagebox.Source = bitmapImage;

                double imageWidth = logoimagebox.ActualWidth;
                double imageHeight = logoimagebox.ActualHeight;
                // testtxt.Content= ((int)imageWidth).ToString()+"X"+((int)imageHeight).ToString();
            }
            else
            {
                urlInputWindow.Close();
            }
        }





        //

        List<string> GetColumnNames(DataSet dataSet)
        {
            List<string> columnNames = new List<string>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[0]; // Assuming you're working with the first DataTable in the DataSet
                DataColumnCollection columns = dataTable.Columns;

                // Iterate over the columns and add their names to the list
                foreach (DataColumn column in columns)
                {
                    columnNames.Add(column.ColumnName);
                }
            }

            return columnNames;
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