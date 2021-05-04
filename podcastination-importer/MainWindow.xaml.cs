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
            public string PodcastKey { get; set; }
            public string SeasonKey { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public System.DateTime Date { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string MP3FileName { get; set; }
            public string ImageFileName { get; set; }
            public string YouTubeURL { get; set; }


        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            // Create Object
            ImportTaskDetails myobj = new ImportTaskDetails()
            {
                PodcastKey = TB_podcatsKey.Text,
                SeasonKey = TB_seasonKey.Text,
                Title = TB_title.Text,
                Subtitle = TB_subtitle.Text,
                Date = DateTime.Now,
                Author = TB_author.Text,
                Description = TB_description.Text,
                MP3FileName = TB_mp3FileName.Text,
                ImageFileName = TB_imageFileName.Text,
                YouTubeURL = TB_youTubeURL.Text
            };

            // Transform ImportTaskDetails to Json Object
            var jsonImportTaskDetails = JsonConvert.SerializeObject(myobj);

            Console.WriteLine(jsonImportTaskDetails);
        }
    }
}
