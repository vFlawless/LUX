using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace Updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Create the WebClient and ProgressBar
            WebClient webClient = new WebClient();
            ProgressBar progressBar = new ProgressBar();
            // Set the ProgressBar properties
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Step = 1;
            progressBar.Value = 0;
            // Set the ProgressBar location
            progressBar.Location = new Point(50, 50);
            // Add the ProgressBar to the form
            Controls.Add(progressBar);

            // Create the Label
            Label label = new Label();
            // Set the Label properties
            label.AutoSize = true;
            label.Text = "Downloading update...";
            // Set the Label location
            label.Location = new Point(50, 100);
            // Add the Label to the form
            Controls.Add(label);

            try
            {
                // Sleep for a short time to allow the controls to be added to the form
                System.Threading.Thread.Sleep(3000);
                // Download the file and update the progress
                webClient.DownloadFileCompleted += (s, e) =>
                {
                    // Update the Label text
                    label.Text = "Update downloaded, extracting files...";
                    // Extract the zip file
                    string zipPath = @".\LUX.zip";
                    string extractPath = @".\";
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                    // Update the Label text
                    label.Text = "Files extracted, cleaning up...";
                    // Delete the zip file
                    File.Delete(@".\LUX.zip");
                    // Update the Label text
                    label.Text = "Update complete, launching LUX...";
                    // Start the LUX.exe file
                    Process.Start(@".\LUX.exe");
                    // Close the form
                    Close();
                };
                webClient.DownloadProgressChanged += (s, e) =>
                {
                    // Update the ProgressBar value
                    progressBar.Value = e.ProgressPercentage;
                };
                // Delete the old LUX.exe file
                File.Delete(@".\LUX.exe");
                // Download the new LUX.zip file
                webClient.DownloadFileAsync(new Uri("https://lux-fishing.000webhostapp.com/update/LUX.zip"), @"LUX.zip");
            }
            catch
            {
                // Start the LUX.exe file
                Process.Start("LUX.exe");
                // Close the form
                Close();
            }

        }
    }
}