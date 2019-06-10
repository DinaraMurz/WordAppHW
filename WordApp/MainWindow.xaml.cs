using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace WordApp
{
    public partial class MainWindow : Window
    {
        private string _saveUri;
        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                fontFamilyListBox.Items.Add(fontFamily.Source);
            }
            wordWrapCheckBox.IsChecked = true;
            

            SaveHotkey.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            OpenHotkey.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            CloseHotkey.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            PrintHotkey.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
            DateAndTimeHotkey.InputGestures.Add(new KeyGesture(Key.F5));
            SelectAllHotkey.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            _saveUri = "NewFile_" + Guid.NewGuid().ToString() + ".txt";
            timer = new Timer(SaveFile, _saveUri, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60));
        }

        private string GetPathOfFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string fileName = "";

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            return fileName;
        }

        private void SaveFile(object pathObj = null)
        {
            var path = pathObj as string;
            if (path == null)
            {
                path = GetPathOfFile();
            }

            try
            {
                UserRichTextBox.SelectAll();
                
                using (var ws = new StreamWriter(path))
                {
                    ws.WriteLine(UserRichTextBox.Selection.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenFile(string path = null)
        {
            if (path == null)
            {
                path = GetPathOfFile();
            }

            try
            {
                UserRichTextBox.SelectAll();

                UserRichTextBox.Document.Blocks.Add(new Paragraph(new Run(System.IO.File.ReadAllText(path))));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FontFamilyItemClick(object sender, RoutedEventArgs e)
        {
            UserRichTextBox.FontFamily = new FontFamily(fontFamilyListBox.SelectedValue.ToString());
        }

        private void OpenMenuClick(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void SaveMenuClick(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        //private void AutoSave()
        //{
        //    string path = "";
        //    try
        //    {
        //        UserRichTextBox.SelectAll();

        //        using (FileStream fs = File.Create(path))
        //        {
        //            Byte[] title = new UTF8Encoding(true).GetBytes("AutoSave");
        //            fs.Write(title, 0, title.Length);
        //            byte[] author = new UTF8Encoding(true).GetBytes(UserRichTextBox.Selection.Text);
        //            fs.Write(author, 0, author.Length);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    //Assembly.GetExecutingAssembly().Location;
        //}

        private void wordWrapCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            UserRichTextBox.Width = grid.Width;
        }

        private void wordWrapCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            UserRichTextBox.Width = int.MaxValue;
        }
        
        public static RoutedCommand SaveHotkey = new RoutedCommand();
        public static RoutedCommand OpenHotkey = new RoutedCommand();
        public static RoutedCommand CloseHotkey = new RoutedCommand();
        public static RoutedCommand PrintHotkey = new RoutedCommand();
        public static RoutedCommand DateAndTimeHotkey = new RoutedCommand();
        public static RoutedCommand SelectAllHotkey = new RoutedCommand();

        private void ExitMenuClick(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void PrintMenuClick(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(UserRichTextBox, "Распечатываем Документ");
            }
        }

        private void ReferenceClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.bing.com/search?q=get+help+with+notepad+in+windows+10&filters=guid:%224466414-en-dia%22%20lang:%22en%22&form=T00032&ocid=HelpPane-BingIA");
        }

        private void DateAndTimeClick(object sender, RoutedEventArgs e)
        {
            UserRichTextBox.SelectAll();
            UserRichTextBox.Document.Blocks.Add(new Paragraph(new Run(DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString())));
        }

        private void SelectAllClick(object sender, RoutedEventArgs e)
        {
            UserRichTextBox.SelectAll();
        }
    }
}
