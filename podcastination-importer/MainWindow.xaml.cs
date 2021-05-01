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

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            // Create Object
            object ImportTaskDetails = new
            {
                PodcastKey = TB_podcatsKey.Text,
                SeasonKey = TB_seasonKey.Text,
                Title = TB_title.Text,
                Subtitle = TB_subtitle,
                Date = DateTime.Now,
                Author = TB_author,
                Description = TB_description,
                MP3FileName = TB_mp3FileName,
                ImageFileName = TB_imageFileName,
                YouTubeURL = TB_youTubeURL
            };

            // Transform ImportTaskDetails to Json Object
            var jsonImportTaskDetails = JsonConvert.SerializeObject(ImportTaskDetails);
            
            Console.WriteLine(jsonImportTaskDetails);
        }
    }
}
