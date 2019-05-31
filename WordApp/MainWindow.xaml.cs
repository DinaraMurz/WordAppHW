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

namespace WordApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //foreach (var fontFamilyName in FontFamily.FamilyNames)
            //{
            //    fontFamilyListBox.Items.Add(fontFamilyName.ToString());
            //}


            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                fontFamilyListBox.Items.Add(fontFamily.Source);
            }
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

        private void SaveFile(string path = null)
        {
            if (path == null)
            {
                path = GetPathOfFile();
            }

            try
            {
                textBoxForUser.SelectAll();

                using (var ws = new StreamWriter(path))
                {
                    ws.WriteLine(textBoxForUser.Selection.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenFile(string path = null)
        {
            ////////
            if (path == null)
            {
                path = GetPathOfFile();
            }

            try
            {
                textBoxForUser.SelectAll();

                textBoxForUser.Document.Blocks.Add(new Paragraph(new Run(System.IO.File.ReadAllText(path))));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FontFamilyItemClick(object sender, RoutedEventArgs e)
        {
            textBoxForUser.FontFamily = new FontFamily(fontFamilyListBox.SelectedValue.ToString());
        }

        private void OpenMenuClick(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void SaveMenuClick(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        

        private void AutoSave()
        {
            ////////////
            string path = "";
            try
            {
                textBoxForUser.SelectAll();

                using (FileStream fs = File.Create(path))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes("AutoSave");
                    fs.Write(title, 0, title.Length);
                    byte[] author = new UTF8Encoding(true).GetBytes(textBoxForUser.Selection.Text);
                    fs.Write(author, 0, author.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Assembly.GetExecutingAssembly().Location;
        }

        ///////быстрый доступ/ горячие клавиши/ потоки
    }
}
