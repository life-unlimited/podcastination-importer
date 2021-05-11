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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace podcastination_importer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

        }

        public class ImportTaskDetails
        {
            public string podcast_key { get; set; }
            public string season_key { get; set; }
            public string title { get; set; }
            public string subtitle { get; set; }
            public DateTime date { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string mp3_file_location { get; set; }
            public string image_file_location { get; set; }
            public string pdf_file_location { get; set; }
            public string youtube_url { get; set; }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = DateTimePicker.SelectedDate.Value;

            // Create Object
            ImportTaskDetails importTaskDetails = new ImportTaskDetails()
            {
                podcast_key = TB_podcatsKey.Text,
                season_key = TB_seasonKey.Text,
                title = TB_title.Text,
                subtitle = TB_subtitle.Text,
                date = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day),
                author = TB_author.Text,
                description = TB_description.Text,
                mp3_file_location = TB_mp3FileLocation.Text,
                image_file_location = TB_imageFileLocation.Text,
                pdf_file_location = TB_pdfFileLocation.Text,
                youtube_url = TB_youTubeURL.Text
            };

            createJsonFile(importTaskDetails, "../task.json");
        }

        public void createJsonFile(object jsonObject, string path)
        {
            
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;

                JsonSerializer serializer = JsonSerializer.Create(settings);
                serializer.Serialize(file, jsonObject);
            }
            MessageBox.Show("Process sucessfull!");
        }

        private void Btn_Mp3File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "mp3 files (*.mp3)|*.mp3";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            try
            {
                openFileDialog1.ShowDialog();

                    string fileSelected = openFileDialog1.FileName;
                    TB_mp3FileLocation.Text = fileSelected;
                
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, while getting the file. Try agian");
            }
        }

        private void Btn_ImageFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "mp3 files (*.mp3)|*.mp3";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            try
            {
                openFileDialog1.ShowDialog();

                string fileSelected = openFileDialog1.FileName;
                TB_imageFileLocation.Text = fileSelected;

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, while getting the file. Try agian");
            }
        }

        private void Btn_PdfFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "mp3 files (*.mp3)|*.mp3";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            try
            {
                openFileDialog1.ShowDialog();

                string fileSelected = openFileDialog1.FileName;
                TB_pdfFileLocation.Text = fileSelected;

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, while getting the file. Try agian");
            }
        }
    }
}
