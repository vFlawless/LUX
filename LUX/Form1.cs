using Google.Cloud.Firestore;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LUX
{
    public partial class Form1 : Form
    {
        protected FirestoreDb db;
        readonly string regex = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w\.\-\?\&\=\+\%\$\@\#\~\;\,\:\!\*\'\(\)]*)*\/?$";
        readonly string currentVersion = "1.4.5";
        int GreenMulti;
        int BlueMulti;
        int PurpleMulti;
        int OrangeMulti;
        int RedMulti;
        string previousMessage = "";
        bool previously_checked = false;
        bool invalidURL = false;
        List<Payload> planetsList = new();
        //                                        Adding all the Fishes                                               \\
        List<Fish> Fishes = new();

        

        public Form1()
        {
            InitializeComponent();

            WebClient webClient = new WebClient();

            try
            {
                if (webClient.DownloadString("https://pastebin.com/raw/F5sf1aJ7") != currentVersion)
                {
                    if (MessageBox.Show("Looks like there is an update! Do you want to download it?", "Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        using (var client = new WebClient())
                        {
                            Process.Start("Updater.exe");
                            this.Close();
                        }
                }
            }
            catch
            {

            }
            
        }

        //DB: 
        //"URL Domain"        Domain of www.youtube.com == youtube.com
            //Random ID
                //Multipliers 
                //HighestValue
                //.
                //.
            //~Random ID
            //~Random ID

        //After first / we split --> make sub directory

        private Rectangle originalFormSize;
        private Rectangle button1OriginalRectangle;
        private Rectangle button2OriginalRectangle;
        private Rectangle label1OriginalRectangle;
        private Rectangle label2OriginalRectangle;
        private Rectangle FishingTextBoxOriginalRectangle;
        private Rectangle StatsTextBoxOriginalRectangle;
        private Rectangle URLTextBoxOriginalrectangle;
        private Rectangle Checkbox1OriginalRectengle;
        private Rectangle Checkbox2OriginalRectengle;


        private void Form1_Load(object sender, EventArgs e)
        {
            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            button1OriginalRectangle = new Rectangle(button1.Location.X, button1.Location.Y, button1.Width, button1.Height);
            button2OriginalRectangle = new Rectangle(button2.Location.X, button2.Location.Y, button2.Width, button2.Height);
            label1OriginalRectangle = new Rectangle(label1.Location.X, label1.Location.Y, label1.Width, label1.Height);
            label2OriginalRectangle = new Rectangle(label2.Location.X, label2.Location.Y, label2.Width, label2.Height);
            FishingTextBoxOriginalRectangle = new Rectangle(FishingHistoryTextbox.Location.X, FishingHistoryTextbox.Location.Y, FishingHistoryTextbox.Width, FishingHistoryTextbox.Height);
            StatsTextBoxOriginalRectangle = new Rectangle(StatsTextBox.Location.X, StatsTextBox.Location.Y, StatsTextBox.Width, StatsTextBox.Height);
            URLTextBoxOriginalrectangle = new Rectangle(URLTextbox.Location.X, URLTextbox.Location.Y, URLTextbox.Width, URLTextbox.Height);
            Checkbox1OriginalRectengle = new Rectangle(UploadDataCheckbox.Location.X, UploadDataCheckbox.Location.Y, UploadDataCheckbox.Width, UploadDataCheckbox.Height);
            Checkbox2OriginalRectengle = new Rectangle(AllStatsCheckBox.Location.X, AllStatsCheckBox.Location.Y, AllStatsCheckBox.Width, AllStatsCheckBox.Height);

            this.Text = "LUX Fishing Tool V" + currentVersion;

            byte[] resourceBytes = Properties.Resources.cloudfire;

            // Write the resource to a temporary file
            string tempPath = Path.GetTempFileName();
            File.WriteAllBytes(tempPath, resourceBytes);

            // Set the GOOGLE_APPLICATION_CREDENTIALS environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

            // GoogleCredential.FromFile(path);
            db = FirestoreDb.Create("luxfishing-1b3b6");
        }


        private async void button1_Click(object sender, EventArgs e)
        {   
            // Resetting the Multipliers so they don't get stacked
            GreenMulti = 0;
            BlueMulti = 0;
            PurpleMulti = 0;
            OrangeMulti = 0;
            RedMulti = 0;

            //                                        Adding all the Fishes                                               \\
            Fishes = new Fish().GetFishList();

            string temp = URLTextbox.Text.ToLower();

            string Message = FishingHistoryTextbox.Text;
            
            if (CheckTextboxesEmpty(temp, Message))
            {
                return;
            }

            label1.Text = "Enter your fishing history";
            label1.ForeColor = System.Drawing.Color.Black;
            label2.Text = "Enter the URL you were fishing on:";
            label2.ForeColor = System.Drawing.Color.Black;

            // Check if a youtube Video Link contains a channel reference (not reachable links)
            if (temp.Contains("youtube.com/") && temp.Contains("&ab_channel=") && UploadDataCheckbox.Checked)
            {
                MessageBox.Show("Youtube links with channel references ('&ab_channel=') can not be reached in the future, so they will not be uploaded to the datatabse. You can only see your session stats." +
                    "Please do not delete this reference and upload the same stats since the URL changed and therfore a whole new wold is generated.");
                invalidURL = true;
            }
            else
            {
                invalidURL = false;
            }

            if (previousMessage == Message && UploadDataCheckbox.Checked && previously_checked)
            {
                label1.Text = "Do not enter the same Text twice:";
                label1.ForeColor = System.Drawing.Color.Red;
                return;
            }
            previousMessage = Message;
            previously_checked = UploadDataCheckbox.Checked;


            // Get trimmed URL --> Collection and Document 
            var res = TrimmURL(temp);
            string Domain = res.Item1;
            string rest = res.Item2;


            // Split into string arrays after line breaks, also delete Server messages if golden / mythic is caught
            string[] lines = Message.Split(
                new string[] { "\n\n" },
                StringSplitOptions.None
            ).Where(s => !s.StartsWith("[")).ToArray();


            // Get what Planet the current is aswell as reading the formatted input --> returning the Multipliers
            var result = GetPlanet(Fishes, lines);
            string planet = result.Item1;
            Fishes = result.Item2;
            List<float> Multipliers = result.Item3;    // List that contains every multiplier 

            int amount = Multipliers.Count;

            // avrg multiplier 
            float averageMulitplier = (float)(Multipliers.Count > 0 ? Multipliers.Average() : 0.0);
            // highest multiplier
            float highestMultiplier = (float)(Multipliers.Count > 0 ? Multipliers.Max() : 0.0);

            StatsTextBox.Text = await OutputStringAsync(planet, averageMulitplier, highestMultiplier, GreenMulti, BlueMulti, PurpleMulti, OrangeMulti, RedMulti, amount, rest, Domain);
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            // This Button is for showing the best Planets
            FishingHistoryTextbox.Text = "";

            var result = await new BestPlanet().GetBestPlanetsAsync(db);
            AppendTextbox(result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6, result.Item7, result.Item8, result.Item9);
            // Color some words
            ColorWords();
        }

        private async void StatsTextBox_Click(object sender, EventArgs e)
        {
            Point clickPosition = StatsTextBox.PointToClient(Cursor.Position); // get the position of the mouse click in the RichTextBox's client area
            int charIndex = StatsTextBox.GetCharIndexFromPosition(clickPosition); // get the index of the character under the mouse cursor
            int startIndex = charIndex; // set the start index of the word to the index of the clicked character
            while (startIndex > 0 && !char.IsWhiteSpace(StatsTextBox.Text[startIndex - 1]))
            {
                // if the character to the left of the clicked character is not a white space, move the start index to the left
                startIndex--;
            }
            int length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index
            while (charIndex < StatsTextBox.Text.Length && !char.IsWhiteSpace(StatsTextBox.Text[charIndex]))
            {
                // if the character to the right of the clicked character is not a white space, move the end index to the right
                charIndex++;
                length++;
            }
            if (length > 0) // if the length of the word is greater than 0, show a message box
            {
                string word = StatsTextBox.Text.Substring(startIndex, length); // get the word that was clicked
               
                planetsList = new();
                
                if (word == "Yellow")
                {
                    planetsList = await GetAllData("Yellow");
                }
                else if (word == "Orange")
                {
                    planetsList = await GetAllData("Orange");
                }
                else if (word == "Grey")
                {
                    planetsList = await GetAllData("Grey");
                }
                else if (word == "Blue")
                {
                    planetsList = await GetAllData("Blue");
                }
                else if (word == "Red")
                {
                    planetsList = await GetAllData("Red");
                }
                else if (word == "Purple")
                {
                    planetsList = await GetAllData("Purple");
                }
                else if (word == "Ocean")
                {
                    planetsList = await GetAllData("Ocean");
                }
                else if (word == "all")
                {
                    planetsList = await GetAllData("all");
                }

                if (planetsList.Count > 0)
                {
                    LoadPopup popup = new LoadPopup(planetsList);
                    popup.Pupup();
                }
            }
        }


        private void StatsTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point clickPosition = StatsTextBox.PointToClient(Cursor.Position); // get the position of the mouse click in the RichTextBox's client area
            int charIndex = StatsTextBox.GetCharIndexFromPosition(clickPosition); // get the index of the character under the mouse cursor
            int startIndex = charIndex; // set the start index of the word to the index of the clicked character
            while (startIndex > 0 && !char.IsWhiteSpace(StatsTextBox.Text[startIndex - 1]))
            {
                // if the character to the left of the clicked character is not a white space, move the start index to the left
                startIndex--;
            }
            int length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index
            while (charIndex < StatsTextBox.Text.Length && !char.IsWhiteSpace(StatsTextBox.Text[charIndex]))
            {
                // if the character to the right of the clicked character is not a white space, move the end index to the right
                charIndex++;
                length++;
            }
            if (length > 0) // if the length of the word is greater than 0, show a message box
            {
                string word = StatsTextBox.Text.Substring(startIndex, length); // get the word that was clicked
                if (word == "Yellow" || word == "Orange" || word == "Grey" || word == "Blue" || word == "Red" || word == "Purple" || word == "Ocean" || word == "all")
                {
                    this.Cursor = Cursors.Hand;
                }
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            resizeControl(button1OriginalRectangle, button1);
            resizeControl(button2OriginalRectangle, button2);
            resizeControl(label1OriginalRectangle, label1);
            resizeControl(label2OriginalRectangle, label2);
            resizeControl(FishingTextBoxOriginalRectangle, FishingHistoryTextbox);
            resizeControl(StatsTextBoxOriginalRectangle, StatsTextBox);
            resizeControl(URLTextBoxOriginalrectangle, URLTextbox);
            resizeControl(Checkbox1OriginalRectengle, UploadDataCheckbox);
            resizeControl(Checkbox2OriginalRectengle, AllStatsCheckBox);
        }

        private void resizeControl(Rectangle r, Control c)
        {
            float xRatio = (float)(this.Width) / (float)(originalFormSize.Width);
            float yRatio = (float)(this.Height) / (float)(originalFormSize.Height);

            int newX = (int)(r.Location.X * xRatio);
            int newY = (int)(r.Location.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }


        private void StatsTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = e.LinkText, UseShellExecute = true });
        }


        private void ColorWords()
        {
            int index = StatsTextBox.Find("Yellow planets:");
            int index2 = StatsTextBox.Find("Best of "); // Beginning of sentence
            int length = index + "Yellow planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.FromArgb(255, 255, 128);
                StatsTextBox.Select(index, "Yellow".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            index = StatsTextBox.Find("Orange planets:");
            length = index + "Orange planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.Orange;
                StatsTextBox.Select(index, "Orange".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            index = StatsTextBox.Find("Grey planets:");
            length = index + "Grey planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.Gray;
                StatsTextBox.Select(index, "Grey".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            index = StatsTextBox.Find("Blue planets:");
            length = index + "Blue planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.Blue;
                StatsTextBox.Select(index, "Blue".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            index = StatsTextBox.Find("Red planets:");
            length = index + "Red planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.Red;
                StatsTextBox.Select(index, "Red".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            index = StatsTextBox.Find("Purple planets:");
            length = index + "Purple planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.Purple;
                StatsTextBox.Select(index, "Pruple".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            index = StatsTextBox.Find("Ocean planets:");
            length = index + "Ocean planets:".Length - index2;
            if (index >= 0)
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.LightBlue;
                StatsTextBox.Select(index, "Ocean".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }


            if (AllStatsCheckBox.Checked)
            {
                index2 = StatsTextBox.Find("Greens ", index, StatsTextBox.TextLength, RichTextBoxFinds.None) - 7;
                index = StatsTextBox.Find(")", index2, StatsTextBox.TextLength, RichTextBoxFinds.None);
                length = index + 1 - index2;
                if (index >= 0)
                {
                    StatsTextBox.SelectionStart = index2;
                    StatsTextBox.SelectionLength = length;
                    StatsTextBox.SelectionColor = Color.Green;
                }


                index2 = StatsTextBox.Find("Blues ", index2, StatsTextBox.TextLength, RichTextBoxFinds.None) - 7;
                index = StatsTextBox.Find(")", index2, StatsTextBox.TextLength, RichTextBoxFinds.None);
                length = index + 1 - index2;
                if (index >= 0)
                {
                    StatsTextBox.SelectionStart = index2;
                    StatsTextBox.SelectionLength = length;
                    StatsTextBox.SelectionColor = Color.Blue;
                }


                index2 = StatsTextBox.Find("Purples ", index2, StatsTextBox.TextLength, RichTextBoxFinds.None) - 7;
                index = StatsTextBox.Find(")", index2, StatsTextBox.TextLength, RichTextBoxFinds.None);
                length = index + 1 - index2;
                if (index >= 0)
                {
                    StatsTextBox.SelectionStart = index2;
                    StatsTextBox.SelectionLength = length;
                    StatsTextBox.SelectionColor = Color.Purple;
                }


                index2 = StatsTextBox.Find("Oranges ", index2, StatsTextBox.TextLength, RichTextBoxFinds.None) - 7;
                index = StatsTextBox.Find(")", index2, StatsTextBox.TextLength, RichTextBoxFinds.None);
                length = index + 1 - index2;
                if (index >= 0)
                {
                    StatsTextBox.SelectionStart = index2;
                    StatsTextBox.SelectionLength = length;
                    StatsTextBox.SelectionColor = Color.Orange;
                }


                index2 = StatsTextBox.Find("Reds ", index2, StatsTextBox.TextLength, RichTextBoxFinds.None) - 7;
                index = StatsTextBox.Find(")", index2, StatsTextBox.TextLength, RichTextBoxFinds.None);
                length = index + 1 - index2;
                if (index >= 0)
                {
                    StatsTextBox.SelectionStart = index2;
                    StatsTextBox.SelectionLength = length;
                    StatsTextBox.SelectionColor = Color.Red;
                }

                index = StatsTextBox.Find(" all ");
                StatsTextBox.SelectionStart = index + 1;
                StatsTextBox.SelectionLength = 3;
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            index = StatsTextBox.Find("average multiplier: 1"); // Color the average Multipliers 
            while (index >= 0)
            {
                index += "average multiplier: ".Length;
                length = 7;
                string x = "";

                for (int i = 0; i < length; i++)
                {
                    x += StatsTextBox.Text[index + i];
                }

                float number = float.Parse(x);
                if (number >= 100000) number /= 100000;

                // Select the word and make it bold
                StatsTextBox.SelectionStart = index;
                StatsTextBox.SelectionLength = length;
                if (number < 1.003)
                {
                    StatsTextBox.SelectionColor = Color.Red;
                }
                else if (number < 1.007)
                {
                    StatsTextBox.SelectionColor = Color.Orange;
                }

                else if (number < 1.010)
                {
                    StatsTextBox.SelectionColor = Color.YellowGreen;
                }
                else
                {
                    StatsTextBox.SelectionColor = Color.Green;
                }

                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, StatsTextBox.SelectionFont.Style | FontStyle.Bold);

                // Find the next occurrence of the word
                index = StatsTextBox.Find("average multiplier: 1", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
            }

            index = StatsTextBox.Find("BEST:");
            while (index >= 0)
            {
                // Select the word and make it bold
                StatsTextBox.SelectionStart = index;
                StatsTextBox.SelectionLength = "BEST:".Length;
                StatsTextBox.SelectionColor = Color.FromArgb(255, 215, 0);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, StatsTextBox.SelectionFont.Style | FontStyle.Bold);

                // Find the next occurrence of the word
                index = StatsTextBox.Find("BEST:", index + "BEST:".Length, StatsTextBox.TextLength, RichTextBoxFinds.None);
            }

            index = StatsTextBox.Find("SECOND BEST:");
            while (index >= 0)
            {
                // Select the word and make it bold
                StatsTextBox.SelectionStart = index;
                StatsTextBox.SelectionLength = "SECOND BEST:".Length;
                StatsTextBox.SelectionColor = Color.FromArgb(192, 192, 192);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, StatsTextBox.SelectionFont.Style | FontStyle.Bold);

                // Find the next occurrence of the word
                index = StatsTextBox.Find("SECOND BEST:", index + "SECOND BEST:".Length, StatsTextBox.TextLength, RichTextBoxFinds.None);
            }

            index = StatsTextBox.Find("THIRD BEST:");
            while (index >= 0)
            {
                // Select the word and make it bold
                StatsTextBox.SelectionStart = index;
                StatsTextBox.SelectionLength = "THIRD BEST:".Length;
                StatsTextBox.SelectionColor = Color.FromArgb(205, 127, 50);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, StatsTextBox.SelectionFont.Style | FontStyle.Bold);

                // Find the next occurrence of the word
                index = StatsTextBox.Find("THIRD BEST:", index + "THIRD BEST:".Length, StatsTextBox.TextLength, RichTextBoxFinds.None);
            }
        }

        private bool CheckTextboxesEmpty(string temp, string Message)
        {
            // Check if Text boxes are empty \\
            if (UploadDataCheckbox.Checked)
            {
                if (temp.Length == 0)
                {
                    label2.Text = "Please enter the URL you were fishing on:";
                    label2.ForeColor = System.Drawing.Color.Red;
                    return true;
                }

                if (!Regex.IsMatch(temp, regex)) // Check for valid URL 
                {
                    label2.Text = "Please enter a valid URL:";
                    label2.ForeColor = System.Drawing.Color.Red;
                    return true;
                }
            }


            if (Message.Length == 0)
            {
                label1.Text = "Please enter your fishing history:";
                label1.ForeColor = System.Drawing.Color.Red;
                return true;
            }


            return false;
        }


        public static (string, string) TrimmURL(string Url)  // Split up the URL so that we can make a collection with the same domain and documents that contain rest of the URL
        {
            string rest = "";

            string[] Pre = { "https://", "https:/", "https:", "http://", "http:/", "http:" };

            // Check if URL starts with any of the patterns (Format Domain of URL)

            for (int i = 0; i < Pre.Length; i++)
            {
                if (Url.StartsWith(Pre[i]))
                {
                    Url = Url.Replace(Pre[i], "");
                }
            }

            // Only start of Link == collection, document (with random ID) contains rest of link:
            for (int i = 0; i < Url.Length; i++)
            {
                if (Url[i] == '/')
                {
                    rest = Url[(i + 1)..];
                    Url = Url.Remove(i, Url.Length - i);
                    break;
                }
            }

            return (Url, rest);
        }



        private async Task<string> OutputStringAsync(string planet, float averageMulitplier, float highestMultiplier,
                                    int GreenMulti, int BlueMulti, int PurpleMulti, int OrangeMulti, int RedMulti,
                                    int amount, string rest, string Domain
                                    )
        {
            string x = "";

            Payload pl = new Payload
            {
                amount = amount,
                amountGreen = GreenMulti,
                amountBlue = BlueMulti,
                amountPurple = PurpleMulti,
                amountOrange = OrangeMulti,
                amountRed = RedMulti,
                avrgMultiplier = averageMulitplier,
                highestMultiplier = highestMultiplier,
                URLSubstring = rest,
                planet = planet
            };


            x += $"Session stats ({planet}): \n";
            x = AppendX(x, pl.avrgMultiplier, pl.highestMultiplier, pl, pl.amount);
            x += $"{$"Out of {pl.amount:n0} {(pl.amount >= 0 ? "fishes" : "fish")}",-30} {pl.amountGreen * 100 + pl.amountBlue * 200 + pl.amountPurple * 1000 + pl.amountOrange * 20000 + pl.amountRed * 1000000:n0} Credits\n";

            if (UploadDataCheckbox.Checked && !invalidURL)
            {
                pl = await DataAsync(pl, Domain, amount);

                x += "------------------------------------------------------------------\n";
                x += $"Alltime stats for https://{Domain}{(rest.Length > 0 ? $"/{rest}" : rest)}: \n\n";
                x = AppendX(x, pl.avrgMultiplier, pl.highestMultiplier, pl, pl.amount);
                x = AppendX(x, pl.avrgMultiplier, pl.highestMultiplier, pl, pl.amount);
                x += $"Out of {pl.amount:n0} {(pl.amount >= 0 ? "fishes" : "fish")}\n";
            }

            return x;
        }

        private static string AppendX(string x, double averageMulitplier, double highestMultiplier, Payload pl, int amount)
        {
            x += $"Your average multiplier is: {averageMulitplier:0.00000}\n";
            x += $"Your highest multiplier is: {highestMultiplier:0.00000}\n";

            x += "\n";

            x += $"You've gotten an average of:\n";

            x += $"{(float)pl.amountGreen / amount * 100:00.00}% Greens\n";
            x += $"{(float)pl.amountBlue / amount * 100:00.00}% Blues\n";
            x += $"{(float)pl.amountPurple / amount * 100:00.00}% Purples\n";
            x += $"{(float)pl.amountOrange / amount * 100:00.00}% Oranges\n";
            x += $"{(float)pl.amountRed / amount * 100:00.00}% Reds\n";

            x += "\n";

            return x;
        }




        private void AppendTextbox(Dictionary<string, BestPlanet[]> bestPlanets, int[] amountPlanets, int amountGreen, int amountBlue, int amountPurple, int amountOrange, int amountRed, float avrgMultiplier, float highestMultiplier)
        {
            StatsTextBox.Text = "";
            for (int i = 0; i < bestPlanets.Count; i++)
            {
                var item = bestPlanets.ElementAt(i);
                var planettype = item.Key;
                StatsTextBox.Text += $"Best of {amountPlanets[i]:n0} {planettype} {(amountPlanets[i] == 1 ? "planet" : "planets")}:\n\n";
                var planets = item.Value;
                for (int j = 0; j < planets.Length; j++)
                {
                    if (j == 0)
                    {
                        StatsTextBox.Text += $"BEST:".PadRight(20);
                    }
                    else if (j == 1)
                    {
                        StatsTextBox.Text += $"SECOND BEST:".PadRight(20);
                    }
                    else
                    {
                        StatsTextBox.Text += $"THIRD BEST:".PadRight(20);
                    }

                    if (planets[j].HighestMultiplier == 0)
                    {
                        StatsTextBox.Text += $"Currently there is no more {planettype} planet.\n\n";
                    }
                    else
                    {
                        StatsTextBox.Text +=
                        $"{planets[j].PlanetName}" +
                        $"\n\n" +
                        $"{planets[j].Amount:n0} {(planets[j].Amount == 1 ? "fish" : "fishes")} caught" +
                        $"\naverage multiplier: {planets[j].AvrgMultiplier:0.00000}" +
                        $"\nhighest multiplier: {planets[j].HighestMultiplier:0.00000} \n\n\n";
                        ;
                    }
                }
                StatsTextBox.Text += $"------------------------------------------------------------------\n\n\n";
            }

            if (AllStatsCheckBox.Checked)
            {
                int amount = amountGreen + amountBlue + amountPurple + amountOrange + amountRed;
                // Show stats of all planets
                StatsTextBox.Text +=
                        $"------------------------------------------------------------------\n\n\n" +
                        $"Stats for all {amountPlanets.Sum()} planets" + $"\n" +
                        $"\n" +
                        $"average multiplier: {avrgMultiplier:0.00000}" +
                        $"\n" +
                        $"highest multiplier: {highestMultiplier:0.00000}" +
                        $"\n" +
                        $"{(float)amountGreen / amount * 100:00.00}% Greens".PadRight(15) +
                        $"({amountGreen:n0} fishes)\n" +
                        $"{(float)amountBlue / amount * 100:00.00}% Blues".PadRight(15) +
                        $"({amountBlue:n0} fishes)\n" +
                        $"{(float)amountPurple / amount * 100:00.00}% Purples".PadRight(15) +
                        $"({amountPurple:n0} fishes)\n" +
                        $"{(float)amountOrange / amount * 100:00.00}% Oranges".PadRight(15) +
                        $"({amountOrange:n0} fishes)\n" +
                        $"{(float)amountRed / amount * 100:00.00}% Reds".PadRight(15) +
                        $"({amountRed:n0} fishes)\n" +
                        $"{amount:n0} Fish caught.        {amountGreen * 100 + amountBlue * 200 + amountPurple * 1000 + amountOrange * 20000 + amountRed * 1000000:n0} Credits"
                        ;
            }
        }






        async Task<Payload> DataAsync(Payload payload, string url, int amount)
        {
            Query capitalQuery = db.Collection(url).WhereEqualTo("URLSubstring", payload.URLSubstring);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();

            if (capitalQuerySnapshot.Count == 0)
            {
                // If it's a new URL
                Add_Document(url, payload);
            }
            else
            {
                // If URL already exists (previously fished on)
                string ID = capitalQuerySnapshot[0].Id; // Get ID of Document (for changing later on)

                Payload pl = capitalQuerySnapshot[0].ConvertTo<Payload>();
                payload.amount += pl.amount;
                payload.amountGreen += pl.amountGreen;
                payload.amountBlue += pl.amountBlue;
                payload.amountPurple += pl.amountPurple;
                payload.amountOrange += pl.amountOrange;
                payload.amountRed += pl.amountRed;
                payload.highestMultiplier = pl.highestMultiplier > payload.highestMultiplier ? pl.highestMultiplier : payload.highestMultiplier;

                float average = payload.avrgMultiplier * amount;     //Calculate the average Multiplier
                average += pl.avrgMultiplier * pl.amount;
                average /= payload.amount;

                payload.avrgMultiplier = average;

                if (payload.planet == "Null" || amount <= 4) // Gotta check this
                {
                    payload.planet = pl.planet;
                }

                // Update Values for URL:
                DocumentReference document = db.Collection(url).Document(ID);
                await document.SetAsync(payload);
            }

            return payload;
        }



        private (string, List<Fish>, List<float>) GetPlanet(List<Fish> Fishes, string[] lines)
        {
            int j;
            string name;
            string number;
            Planet planettemp = Planet.Null;
            List<float> Multipliers = new();

            for (int i = 0; i < lines.Length; i++)
            {
                j = 14;
                name = "";
                number = "";

                //Find Name
                for (int k = j; k < lines[i].Length; k++)
                {
                    if (lines[i][k] != ']')
                    {
                        name += lines[i][k];
                    }
                    else
                    {
                        j = k;
                        break;
                    }
                }

                // Find Weight 
                for (int k = j; k < lines[i].Length; k++)
                {
                    if (Char.IsDigit(lines[i][k]))
                    {
                        number += lines[i][k];
                    }
                    else if (lines[i][k] == '.')
                    {
                        number += ',';
                    }
                    else if (number.Length != 0)
                    {
                        break;
                    }
                }

                // Add Multiplier to list
                for (int k = 0; k < Fishes.Count; k++)
                {
                    if (name == Fishes[k].name)
                    {
                        switch (Fishes[k].rarity)
                        {
                            case Rarity.Green:
                                GreenMulti++;
                                break;
                            case Rarity.Blue:
                                BlueMulti++;
                                break;
                            case Rarity.Purple:
                                PurpleMulti++;
                                break;
                            case Rarity.Orange:
                                OrangeMulti++;
                                break;
                            case Rarity.Red:
                                RedMulti++;
                                break;
                        }

                        if (float.Parse(number) / Fishes[k].minWeigth < 1000) // There was a problem when people are coming from europe / america because of the decimal '.' / ',' 
                        {
                            Multipliers.Add(float.Parse(number) / Fishes[k].minWeigth);
                        }
                        else
                        {
                            Multipliers.Add((float.Parse(number) / Fishes[k].minWeigth) / 1000);
                        }

                        if (planettemp == Planet.Null)
                        {
                            planettemp = Fishes[k].planet;
                        }
                        else if (planettemp != Fishes[k].planet)
                        {
                            planettemp = Planet.Ocean;
                        }

                        break;  // If Fish was found we can stop looping threw the rest 
                    }
                    else if (k == Fishes.Count - 1)
                    {
                        Console.WriteLine(name);
                    }
                }
            }

            string planet = "";

            switch (planettemp)     // Planet type 
            {
                case Planet.Ocean:
                    planet = "Ocean";
                    break;
                case Planet.Orange:
                    planet = "Orange";
                    break;
                case Planet.Red:
                    planet = "Red";
                    break;
                case Planet.Yellow:
                    planet = "Yellow";
                    break;
                case Planet.Blue:
                    planet = "Blue";
                    break;
                case Planet.Grey:
                    planet = "Grey";
                    break;
                case Planet.Purple:
                    planet = "Purple";
                    break;
                case Planet.Null:
                    planet = "Null";
                    break;
            }

            return (planet, Fishes, Multipliers);
        }





        async void Add_Document(string URL, Payload payload)
        {
            CollectionReference collection = db.Collection(URL);

            DocumentReference doc = db.Collection("URLS").Document("Domain"); // Collection with the names of the other collections (bases for leaderboards)
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();

            Payload2 pl = snapshot.ConvertTo<Payload2>();
            for (int i = 0; i < pl.Domains.Count; i++)
            {
                if (pl.Domains[i] == URL)
                {
                    break;
                }
                else if (i == pl.Domains.Count - 1)
                {
                    pl.Domains.Add(URL);
                    await doc.UpdateAsync("Domains", pl.Domains);
                }
            }
            await collection.AddAsync(payload);
        }


        private async Task<List<Payload>> GetAllData(string planet)
        {
            List<Payload> pl = new List<Payload>();
            DocumentReference doc = db.Collection("URLS").Document("Domain"); // Collection with the names of the other collections (bases for leaderboards)
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();

            Payload2 pl2 = snapshot.ConvertTo<Payload2>();

            if(planet == "all")
            {
                for (int i = 0; i < pl2.Domains.Count; i++)     // For every collection (domain of URL)
                {
                    Query capitalQuery = db.Collection(pl2.Domains[i]); // Get Every Planet that is the right PlanetType
                    QuerySnapshot snapshots = await capitalQuery.GetSnapshotAsync();

                    for (int k = 0; k < snapshots.Count; k++)    // Every Planet with that type 
                    {
                        // Safe data of all planets (for when checkbox is checked):
                        Payload Planet = snapshots[k].ConvertTo<Payload>();
                        pl.Add(new Payload
                        {
                            amount = Planet.amount,
                            amountBlue = Planet.amountBlue,
                            amountGreen = Planet.amountGreen,
                            amountRed = Planet.amountRed,
                            amountOrange = Planet.amountOrange,
                            amountPurple = Planet.amountPurple,
                            avrgMultiplier = Planet.avrgMultiplier,
                            highestMultiplier = Planet.highestMultiplier,
                            planet = Planet.planet,
                            URLSubstring = "https://" + pl2.Domains[i] + "/" + Planet.URLSubstring
                        });
                    }
                    pl.Sort((x, y) => y.avrgMultiplier.CompareTo(x.avrgMultiplier));
                }
                return pl;
            }

            for (int i = 0; i < pl2.Domains.Count; i++)     // For every collection (domain of URL)
            {
                Query capitalQuery = db.Collection(pl2.Domains[i]).WhereEqualTo("planet", planet); // Get Every Planet that is the right PlanetType
                QuerySnapshot snapshots = await capitalQuery.GetSnapshotAsync();

                for (int k = 0; k < snapshots.Count; k++)    // Every Planet with that type 
                {
                    // Safe data of all planets (for when checkbox is checked):
                    Payload Planet = snapshots[k].ConvertTo<Payload>();
                    pl.Add(new Payload
                    {
                        amount = Planet.amount,
                        amountBlue = Planet.amountBlue,
                        amountGreen = Planet.amountGreen,
                        amountRed = Planet.amountRed,
                        amountOrange = Planet.amountOrange,
                        amountPurple = Planet.amountPurple,
                        avrgMultiplier = Planet.avrgMultiplier,
                        highestMultiplier = Planet.highestMultiplier,
                        planet = Planet.planet,
                        URLSubstring = "https://" + pl2.Domains[i] + "/" + Planet.URLSubstring
                    });
                }
                pl.Sort((x, y) => y.avrgMultiplier.CompareTo(x.avrgMultiplier));
            }
            return pl;
        }

    }


    public enum Rarity
    {
        Green,
        Blue,
        Purple,
        Orange,
        Red
    }


    public enum Planet
    {
        Ocean,
        Orange,
        Yellow,
        Blue,
        Red,
        Grey,
        Purple,
        Null
    }

}