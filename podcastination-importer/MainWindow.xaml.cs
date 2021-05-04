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
            public System.DateTime date { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string mp3_file_name { get; set; }
            public string image_file_name { get; set; }
            public string youtube_url { get; set; }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            // Create Object
            ImportTaskDetails importTaskDetails = new ImportTaskDetails()
            {
                podcast_key = TB_podcatsKey.Text,
                season_key = TB_seasonKey.Text,
                title = TB_title.Text,
                subtitle = TB_subtitle.Text,
                date = DateTime.Now,
                author = TB_author.Text,
                description = TB_description.Text,
                mp3_file_name = TB_mp3FileName.Text,
                image_file_name = TB_imageFileName.Text,
                youtube_url = TB_youTubeURL.Text
            };

            // Transform ImportTaskDetails to Json Object
            string jsonImportTaskDetails = JsonConvert.SerializeObject(importTaskDetails);

            createJsonFile(jsonImportTaskDetails, "../movie.json");
        }

        public void createJsonFile(string jsonObject, string path)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, jsonObject);
            }
        }
    }
}
