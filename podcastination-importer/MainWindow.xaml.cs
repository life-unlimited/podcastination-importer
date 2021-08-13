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
using System.ComponentModel;
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
            public string time { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string mp3_file_location { get; set; }
            public string image_file_location { get; set; }
            public string pdf_file_location { get; set; }
            public string youtube_url { get; set; }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {

            string mp3Path = TB_mp3FileLocation.Text;
            string imagePath = TB_imageFileLocation.Text;
            string pdfPath = TB_pdfFileLocation.Text;
            string jsonPath = "../task.json";
            DateTime selectedDate = DateTimePicker.SelectedDate.Value;

            // Create Object
            ImportTaskDetails importTaskDetails = new ImportTaskDetails()
            {
                podcast_key = TB_podcatsKey.Text,
                season_key = TB_seasonKey.Text,
                title = TB_title.Text,
                subtitle = TB_subtitle.Text,
                date = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day),
                time = TB_Time.Text,
                author = TB_author.Text,
                description = TB_description.Text,
                mp3_file_location = TB_mp3FileLocation.Text,
                image_file_location = TB_imageFileLocation.Text,
                pdf_file_location = TB_pdfFileLocation.Text,
                youtube_url = TB_youTubeURL.Text
            };

            createJsonFile(importTaskDetails, jsonPath);
            moveFiles(mp3Path, imagePath, pdfPath, jsonPath);
            MessageBox.Show("Success!");
        }

        public void createJsonFile(object jsonObject, string path)
        {
            if(path == "")
            {
                string directory = @"..\presets\";

                // Path to directory before the instaltion directory
                string combinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), directory);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = System.IO.Path.GetFullPath(combinedPath);
                saveFileDialog.Filter = "json files (*.json)|*.json";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == true)
                {
                   path = saveFileDialog.FileName;
                }
            }

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;

                JsonSerializer serializer = JsonSerializer.Create(settings);
                serializer.Serialize(file, jsonObject);
            }
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
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: \n{ex}");
            }
        }

        private void Btn_ImageFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "png files (*.png)|*.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            try
            {
                openFileDialog1.ShowDialog();

                string fileSelected = openFileDialog1.FileName;
                TB_imageFileLocation.Text = fileSelected;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: \n{ex}");
            }
        }

        private void Btn_PdfFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "pdf files (*.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            try
            {
                openFileDialog1.ShowDialog();

                string fileSelected = openFileDialog1.FileName;
                TB_pdfFileLocation.Text = fileSelected;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: \n{ex}");
            }
        }

        public void moveFiles(string mp3File, string imageFile, string pdfFile, string jsonPath)
        {
            DateTime thisDay = DateTime.Today;
            string thisDayConverted = thisDay.ToString("d");

            try
            {
                // files will get stored in a dircetory in the same path as the program is installed 
                // The directory has the current date as a name

                DirectoryInfo di = Directory.CreateDirectory(@$".\{thisDayConverted}");

                File.Move(mp3File, di.FullName + @"\predigt.mp3");

                if (imageFile != "")
                {
                    File.Move(imageFile, di.FullName + @"\thumb.png");
                }

                if (pdfFile != "")
                {
                    File.Move(pdfFile, di.FullName + @"\pdfPredipt.pdf");
                }

                File.Move(jsonPath, di.FullName + @"\task.json");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong while moving the files: \n{ex}");
            }
        }

        private void Btn_saveAsPreset_Click(object sender, RoutedEventArgs e)
        {
            string directory = @"..\presets\";
            string path = "";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create Object
            ImportTaskDetails importTaskDetails = new ImportTaskDetails()
            {
                podcast_key = TB_podcatsKey.Text,
                season_key = TB_seasonKey.Text,
                title = TB_title.Text,
                subtitle = TB_subtitle.Text,
                time = TB_Time.Text,
                author = TB_author.Text,
                description = TB_description.Text,
                mp3_file_location = TB_mp3FileLocation.Text,
                image_file_location = TB_imageFileLocation.Text,
                pdf_file_location = TB_pdfFileLocation.Text,
                youtube_url = TB_youTubeURL.Text
            };

            createJsonFile(importTaskDetails, path);
        }

        private void Btn_openPreset_Click(object sender, RoutedEventArgs e)
        {
            string filePath;
            string directory = @"..\presets\";

            // Path to directory before the instaltion directory
            string combinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), directory); 

            if (!Directory.Exists(directory))
            {
                MessageBox.Show("Es gibt noch keine Presets!");
            }

            // Start file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(combinedPath);
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                
                if (openFileDialog.ShowDialog() == true)
                {

                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    using (StreamReader file = File.OpenText(filePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        ImportTaskDetails importTaskDetails = (ImportTaskDetails)serializer.Deserialize(file, typeof(ImportTaskDetails));

                        setPresetText(importTaskDetails);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: \n{ex}");
            }
        }

        public void setPresetText(ImportTaskDetails importTaskDetails)
        {
            TB_podcatsKey.Text = importTaskDetails.podcast_key;
            TB_seasonKey.Text = importTaskDetails.season_key;
            TB_title.Text = importTaskDetails.title;
            TB_Time.Text = importTaskDetails.time;
            TB_author.Text = importTaskDetails.author;
            TB_description.Text = importTaskDetails.description;
            TB_mp3FileLocation.Text = importTaskDetails.mp3_file_location;
            TB_imageFileLocation.Text = importTaskDetails.image_file_location;
            TB_pdfFileLocation.Text = importTaskDetails.pdf_file_location;
            TB_youTubeURL.Text = importTaskDetails.youtube_url;
        }

        private void TB_mp3FileLocation_TextChanged(object sender, TextChangedEventArgs e)
        {

            Btn_start.IsEnabled = TB_mp3FileLocation.Text != "";
        }
    }
}
