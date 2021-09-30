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
using System.Configuration;
using System.Globalization;
using System.Collections;

namespace podcastination_importer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string Mp3OutputName = "sermon.mp3";
        const string ThumbOutputName = "thumb.png";
        const string PdfOutputName = "presentation.pdf";

        public MainWindow()
        {
            InitializeComponent();
            DateTimePicker.SelectedDate = DateTime.Today;
            refreshFormStates();
        }

        public class ImportTaskDetails
        {
            public string podcast_key { get; set; }
            public string season_key { get; set; }
            public string title { get; set; }
            public string subtitle { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string mp3_file { get; set; }
            public string image_file { get; set; }
            public string pdf_file { get; set; }
            public string yt_url { get; set; }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {

            string mp3Path = TB_mp3FileLocation.Text;
            string imagePath = TB_imageFileLocation.Text;
            string pdfPath = TB_pdfFileLocation.Text;
            string jsonPath = ConfigurationManager.AppSettings["taskJsonPath"];
            DateTime selectedDate = DateTimePicker.SelectedDate.Value;
            DateTime dateWithTime = addTimeToDate(selectedDate);

            // Create Object
            ImportTaskDetails importTaskDetails = new ImportTaskDetails()
            {
                podcast_key = TB_podcastKey.Text,
                season_key = TB_seasonKey.Text,
                title = TB_title.Text,
                subtitle = TB_subtitle.Text,
                date = dateWithTime.ToString("yyyy-MM-dd'T'HH:mm:ss.ffZ"),
                author = TB_author.Text,
                description = TB_description.Text,
                mp3_file = Mp3OutputName,
                image_file = imagePath == "" ? "" : ThumbOutputName,
                pdf_file = pdfPath == "" ? "" : PdfOutputName,
                yt_url = TB_youTubeURL.Text
            };

            createJsonFile(importTaskDetails, jsonPath);
            moveFiles(mp3Path, imagePath, pdfPath, jsonPath);
        }

        public void createJsonFile(object jsonObject, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter file = File.CreateText(path + @"\task.json"))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Formatting = Formatting.Indented;

                    JsonSerializer serializer = JsonSerializer.Create(settings);
                    serializer.Serialize(file, jsonObject);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            };

        }

        public DateTime addTimeToDate(DateTime selectedDate)
        {
            DateTime dateWithTime;
            string timeValue = TB_Time.Text;

            string[] timeValueArray = timeValue.Split(":");
            if (timeValueArray.Length != 2)
            {
                throw new Exception("Wrong time format. Expected: 'hh:mm'");
            }
            selectedDate = selectedDate.AddHours(Convert.ToDouble(timeValueArray[0]));
            selectedDate = selectedDate.AddMinutes(Convert.ToDouble(timeValueArray[1]));
            dateWithTime = selectedDate;

            return dateWithTime;
        }

        private void Btn_Mp3File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            try
            {
                if ((bool)!openFileDialog.ShowDialog())
                {
                    return;
                };

                string fileSelected = openFileDialog.FileName;
                TB_mp3FileLocation.Text = fileSelected;

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, while getting the mp3. Try agian");
            }
        }

        private void Btn_ImageFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "png files (*.png)|*.png";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            try
            {
                if ((bool)!openFileDialog.ShowDialog())
                {
                    return;
                };

                string fileSelected = openFileDialog.FileName;
                TB_imageFileLocation.Text = fileSelected;

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, while getting the image. Try agian");
            }
        }

        private void Btn_PdfFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            try
            {
                if ((bool)!openFileDialog.ShowDialog())
                {
                    return;
                };

                string fileSelected = openFileDialog.FileName;
                TB_pdfFileLocation.Text = fileSelected;

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, while getting the pdf. Try agian");
            }
        }

        public void moveFiles(string mp3File, string imageFile, string pdfFile, string jsonPath)
        {
            DateTime thisDay = DateTime.Now;
            string thisDayConverted = $"{thisDay.Year}-{thisDay.Month}-{thisDay.Day}-{thisDay.Hour}-{thisDay.Minute}-{thisDay.Second}";
            string fullPath = System.IO.Path.Combine(ConfigurationManager.AppSettings["destination"] + thisDayConverted);
            try
            {
                // The directory has the current date as a name

                DirectoryInfo di = Directory.CreateDirectory(fullPath);

                File.Copy(mp3File, System.IO.Path.Join(di.FullName, Mp3OutputName));

                if (imageFile != "")
                {
                    File.Copy(imageFile, System.IO.Path.Join(di.FullName, ThumbOutputName));
                }

                if (pdfFile != "")
                {
                    File.Copy(pdfFile, System.IO.Path.Join(di.FullName, PdfOutputName));
                }

                File.Move(jsonPath + @"\task.json", di.FullName + @"\task.json");

                MessageBox.Show($"Files have been successfuly moved to {fullPath}");

                System.Windows.Application.Current.Shutdown();

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong while moving the files: \n{e}");
            }
        }

        private void Btn_saveAsPreset_Click(object sender, RoutedEventArgs e)
        {
            string jsonPath = String.Empty;
            string filepath = ConfigurationManager.AppSettings["presetLocation"];
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = filepath;
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.AddExtension = true;
            if ((bool)!saveFileDialog.ShowDialog())
            {
                return;
            };
            jsonPath = saveFileDialog.FileName;


            // Create Object
            ImportTaskDetails importTaskDetails = new ImportTaskDetails()
            {
                podcast_key = TB_podcastKey.Text,
                season_key = TB_seasonKey.Text,
                title = TB_title.Text,
                subtitle = TB_subtitle.Text,
                time = TB_Time.Text,
                author = TB_author.Text,
                description = TB_description.Text,
                mp3_file = TB_mp3FileLocation.Text,
                image_file = TB_imageFileLocation.Text,
                pdf_file = TB_pdfFileLocation.Text,
                yt_url = TB_youTubeURL.Text
            };

            createJsonFile(importTaskDetails, jsonPath);
        }

        private void Btn_openPreset_Click(object sender, RoutedEventArgs e)
        {
            string filePath;

            string directory = ConfigurationManager.AppSettings["presetLocation"];
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if ((bool)!openFileDialog.ShowDialog())
                {
                    return;
                };

                //Get the path of specified file
                filePath = openFileDialog.FileName;

                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    ImportTaskDetails importTaskDetails = (ImportTaskDetails)serializer.Deserialize(file, typeof(ImportTaskDetails));

                    setPresetText(importTaskDetails);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }


        }

        public void setPresetText(ImportTaskDetails importTaskDetails)
        {
            TB_podcastKey.Text = importTaskDetails.podcast_key;
            TB_seasonKey.Text = importTaskDetails.season_key;
            TB_title.Text = importTaskDetails.title;
            TB_Time.Text = importTaskDetails.time;
            TB_author.Text = importTaskDetails.author;
            TB_description.Text = importTaskDetails.description;
            TB_mp3FileLocation.Text = importTaskDetails.mp3_file;
            TB_imageFileLocation.Text = importTaskDetails.image_file;
            TB_pdfFileLocation.Text = importTaskDetails.pdf_file;
            TB_youTubeURL.Text = importTaskDetails.yt_url;
        }

        private List<string> validateForm()
        {
            List<string> errorList = new List<string>();

            if (TB_podcastKey.Text == "")
                errorList.Add("Missing PodcastKey.");

            if (TB_seasonKey.Text == "")
                errorList.Add("Missing SeasonKey.");

            if (TB_title.Text == "")
                errorList.Add("Missing Title.");

            if (TB_Time.Text == "")
                errorList.Add("Missing Time.");

            if (TB_author.Text == "")
                errorList.Add("Missing Author.");

            if (TB_mp3FileLocation.Text == "")
                errorList.Add("Missing MP3 file.");

            if (DateTimePicker.SelectedDate == null)
                errorList.Add("Missing Date.");

            return errorList;
        }

        public void refreshFormStates()
        {
            List<string> errorList = validateForm();
            if(errorList.Count == 0)
            {
                TB_validStates.Text = "No errors.";
                TB_validStates.Foreground = Brushes.White;
                Btn_start.IsEnabled = true;
            }
            else
            {
                TB_validStates.Text = String.Join("\n", errorList);
                TB_validStates.Foreground = Brushes.Red;
                Btn_start.IsEnabled = false;
            }
        }

        private void refreshFormStates(object sender, TextChangedEventArgs e)
        {
            refreshFormStates();
        }

        private void refreshFormStates(object sender, SelectionChangedEventArgs e)
        {
            refreshFormStates();
        }
    }
}
