using System.Diagnostics;
using System.IO;
using System.Windows;

namespace DemucsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string filename;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseAction(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension
                DefaultExt = ".mp3",
                Filter = "MP3 Files (*.mp3)|*.mp3|WAV Files (*.wav)|*.wav"
            };

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
                BrowseText.Text = filename;
            }
        }

        private void StartAction(object sender, RoutedEventArgs e)
        {
            if (filename == null)
            {
                BrowseText.Text = "No File Selected!";
            }

            string StemSelect = StemSelectSwitch(TwoStems.Text);
            string QualitySelect = QualitySelectSwitch(OutputQuality.Text);

            string tempfilename = string.Concat(" ", '"', Path.GetFileName(filename), '"');

            string command = string.Concat("demucs", StemSelect, QualitySelect, tempfilename);

            BrowseText.Text = command;

            Process cmd = new Process //Generate the XML report we'll be grabbing the data from
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = Path.GetDirectoryName(filename),
                    Arguments = string.Concat(@"/C ", command),
                }
            };
            cmd.Start();
        }

        public string QualitySelectSwitch(string selection)
        {
            string final;

            switch (selection)
            {
                case "WAV - Int24":
                    final = " --int24";
                    break;

                case "WAV - Float32":
                    final = " --float32";
                    break;

                case "MP3 - 320kbps":
                    final = " --mp3";
                    break;

                default:
                    final = "";
                    break;
            }

            return final;
        }

        public string StemSelectSwitch(string selection)
        {
            string final;

            if (selection == "None")
                return "";

            final = string.Concat(" --two-stems=", selection.ToLower());

            return final;
        }
    }
}