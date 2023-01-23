using Google.Api;
using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LUX
{
    public partial class Form1 : Form
    {
        readonly ToolTip toolTip = new();
        
        protected FirestoreDb db;
        readonly string regex = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w\.\-\?\&\=\+\%\$\@\#\~\;\,\:\!\*\'\(\)]*)*\/?$";
        readonly string currentVersion = "1.6.1";
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
        List<Fish> Fishes = new Fish().GetFishList();
        float[] top3Multis = new float[3];
        string[] top3Names = new string[3];
        List<List<string>> tooltipWords = new();
        List<string> hoverMultiWords = new();
        int amountToolBox = 0;

        bool isLoad = false;

        public Form1()
        {
            InitializeComponent();
            WebClient webClient = new();

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
        private Rectangle label3OriginalRectangle;
        private Rectangle label4OriginalRectangle;
        private Rectangle FishingTextBoxOriginalRectangle;
        private Rectangle StatsTextBoxOriginalRectangle;
        private Rectangle URLTextBoxOriginalrectangle;
        private Rectangle Checkbox1OriginalRectengle;
        private Rectangle Checkbox2OriginalRectengle;
        private Rectangle whiteListWord;
        private Rectangle whiteListNumber;

        private void Form1_Load(object sender, EventArgs e)
        {
            isLoad = true;

            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            button1OriginalRectangle = new Rectangle(button1.Location.X, button1.Location.Y, button1.Width, button1.Height);
            button2OriginalRectangle = new Rectangle(button2.Location.X, button2.Location.Y, button2.Width, button2.Height);
            label1OriginalRectangle = new Rectangle(label1.Location.X, label1.Location.Y, label1.Width, label1.Height);
            label2OriginalRectangle = new Rectangle(label2.Location.X, label2.Location.Y, label2.Width, label2.Height);
            label3OriginalRectangle = new Rectangle(label3.Location.X, label3.Location.Y, label3.Width, label3.Height);
            label4OriginalRectangle = new Rectangle(label4.Location.X, label4.Location.Y, label4.Width, label4.Height);
            FishingTextBoxOriginalRectangle = new Rectangle(FishingHistoryTextbox.Location.X, FishingHistoryTextbox.Location.Y, FishingHistoryTextbox.Width, FishingHistoryTextbox.Height);
            StatsTextBoxOriginalRectangle = new Rectangle(StatsTextBox.Location.X, StatsTextBox.Location.Y, StatsTextBox.Width, StatsTextBox.Height);
            URLTextBoxOriginalrectangle = new Rectangle(URLTextbox.Location.X, URLTextbox.Location.Y, URLTextbox.Width, URLTextbox.Height);
            Checkbox1OriginalRectengle = new Rectangle(UploadDataCheckbox.Location.X, UploadDataCheckbox.Location.Y, UploadDataCheckbox.Width, UploadDataCheckbox.Height);
            Checkbox2OriginalRectengle = new Rectangle(AllStatsCheckBox.Location.X, AllStatsCheckBox.Location.Y, AllStatsCheckBox.Width, AllStatsCheckBox.Height);
            whiteListNumber = new Rectangle(whitelistmultiplier.Location.X, whitelistmultiplier.Location.Y, whitelistmultiplier.Width, whitelistmultiplier.Height);
            whiteListWord = new Rectangle(whitelistFish.Location.X, whitelistFish.Location.Y, whitelistFish.Width, whitelistFish.Height);


            this.Text = "LUX Fishing Tool V" + currentVersion;

            Fishes.Sort((x, y) => x.name.CompareTo(y.name));
            whitelistFish.Items.Add("All Fish");
            whitelistFish.Items.Add("Common");
            whitelistFish.Items.Add("Rare");
            whitelistFish.Items.Add("Epic");
            whitelistFish.Items.Add("Legendary");
            whitelistFish.Items.Add("Mythical");
            whitelistFish.Items.Add("");
            for (int i = 0; i < Fishes.Count; i++)
            {
                whitelistFish.Items.Add(Fishes[i].name);
            }

            byte[] resourceBytes = Properties.Resources.cloudfire;

            // Write the resource to a temporary file
            string tempPath = Path.GetTempFileName();
            File.WriteAllBytes(tempPath, resourceBytes);

            // Set the GOOGLE_APPLICATION_CREDENTIALS environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

            // GoogleCredential.FromFile(path);
            db = FirestoreDb.Create("luxfishing-1b3b6");
        }


        private async void Button1_Click(object sender, EventArgs e)
        {
            // Resetting
            top3Multis = new float[3];
            top3Names = new string[3];
            tooltipWords = new List<List<string>>();
            hoverMultiWords = new List<string>();
            amountToolBox = 0;

            // Resetting the Multipliers so they don't get stacked
            GreenMulti = 0;
            BlueMulti = 0;
            PurpleMulti = 0;
            OrangeMulti = 0;
            RedMulti = 0;

            //                                        Adding all the Fishes                                               \\
            Fishes = new Fish().GetFishList();

            string temp = URLTextbox.Text;

            string Message = FishingHistoryTextbox.Text;

            // If you want to watch a fish
            string selectedFish = whitelistFish.SelectedItem?.ToString() ?? "";
            string selectedMultiTemp = whitelistmultiplier.Text.Replace(" ", "0");
            while (selectedMultiTemp.Length < 5)
            {
                selectedMultiTemp += "0";
            }

            float selectedMultiplier = float.Parse(selectedMultiTemp);

            if(selectedMultiplier >= 1000)
            {
                selectedMultiplier /= 1000;
            }

            if (CheckTextboxesEmpty(temp, Message))
            {
                return;
            }

            label1.Text = "Enter your fishing history";
            label1.ForeColor = System.Drawing.Color.Black;
            label2.Text = "Enter the URL you were fishing on:";
            label2.ForeColor = System.Drawing.Color.Black;

            // Check if a youtube Video Link contains a channel reference (not reachable links)
            if (temp.ToLower().Contains("youtube.com/") && temp.ToLower().Contains("&ab_channel=") && UploadDataCheckbox.Checked)
            {
                MessageBox.Show("Youtube links with channel references ('&ab_channel=') can not be reached in the future, so they will not be uploaded to the datatabse. You can only see your session stats." +
                    "Please do not delete this reference and upload the same stats since the URL changed and therfore a whole new wold is generated.");
                invalidURL = true;
            }
            else
            {
                invalidURL = false;
            }


            if (previousMessage.Length != 0 && Message.Contains(previousMessage) && (previously_checked && UploadDataCheckbox.Checked))      // Check if Text was placed in there double 
            {
                string temp2 = Message;
                Message = Message.Replace(previousMessage, "");
                previousMessage = temp2;
                Message = Message.Replace("\n", "");
            }
            else if (UploadDataCheckbox.Checked)
            {
                previousMessage = Message;
            }

            if(Message == "")
            {
                label1.Text = "Enter a different text then before:";
                label1.ForeColor = System.Drawing.Color.Red;
                return;
            }

            previously_checked = UploadDataCheckbox.Checked;


            // Get trimmed URL --> Collection and Document 
            var res = TrimmURL(temp);
            string Domain = res.Item1;
            string rest = res.Item2;


            // Split into string arrays after line breaks, also delete Server messages if golden / mythic is caught
            string[] lines = Message.Split(
                new string[] { "\n\n" },
                StringSplitOptions.None
            ).ToArray();


            // Get what Planet the current is aswell as reading the formatted input --> returning the Multipliers
            var result = GetPlanet(Fishes, lines, selectedFish, selectedMultiplier);
            string planet = result.Item1;
            Fishes = result.Item2;
            List<float> Multipliers = result.Item3;    // List that contains every multiplier 
            List<float> selectedFishMultipliers = result.Item4;

            if (Multipliers.Any(x => x > 2)) // Check if a multiplier > 2 is contained --> if multipliers were manipulated
            {
                if (UploadDataCheckbox.Checked)
                {
                    MessageBox.Show("Please do not manipulate multipliers! These stats will not be uploaded to the database!");
                    return;
                }
                MessageBox.Show("Please do not manipulate multipliers!");
            }
            
            int amount = Multipliers.Count;

            // avrg multiplier 
            float averageMulitplier = (float)(Multipliers.Count > 0 ? Multipliers.Average() : 0.0);
         
            
            StatsTextBox.Text = await OutputStringAsync(planet, averageMulitplier, top3Multis.ToList(), GreenMulti, BlueMulti, PurpleMulti, OrangeMulti, RedMulti, amount, rest, Domain, selectedFish, selectedFishMultipliers, selectedMultiplier);
            //Color Output
            ColorWords2();
        }

        private async void Button2_Click(object sender, EventArgs e)
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
                else if (word == "Gray")
                {
                    planetsList = await GetAllData("Gray");
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
                    LoadPopup popup = new(planetsList);
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

                if (word == "Yellow" || word == "Orange" || word == "Gray" || word == "Blue" || word == "Red" || word == "Purple" || word == "Ocean" || word == "all")
                {
                    this.Cursor = Cursors.Hand;
                }

                if(startIndex < 150)
                {
                    if (float.TryParse(word, out float result) && top3Multis.Contains(result))
                    {
                        this.Cursor = Cursors.Hand;

                        int index = Array.IndexOf(top3Multis, float.Parse(word));

                        string temp = top3Names[index];

                        if (temp != toolTip.GetToolTip(StatsTextBox))
                        {
                            toolTip.Show(temp, StatsTextBox, clickPosition.X + 15, clickPosition.Y + 15);
                        }
                    }
                }

                else if (hoverMultiWords.Contains(word))
                {
                    this.Cursor = Cursors.Hand;

                    int index = hoverMultiWords.IndexOf(word);
                    string temp = tooltipWords[index][0];
                    for(int i = 1; i < tooltipWords[index].Count; i++)
                    {
                        if (i % 5 == 0)
                        {
                            temp += "\n";
                            temp += tooltipWords[index][i];
                        }
                        else
                        {
                            temp += ", " + tooltipWords[index][i];
                        }
                    }
                    
                    if(temp != toolTip.GetToolTip(StatsTextBox))
                    {
                        toolTip.Show(temp, StatsTextBox, clickPosition.X + 15, clickPosition.Y + 15);
                    }
                }

                else
                {
                    toolTip.Hide(StatsTextBox);
                }

            }
        }

        static void SortLists(List<string> weights, List<List<string>> names, out List<string> sortedWeights, out List<List<string>> sortedNames)
        {
            var combined = weights.Zip(names, (w, n) => new { Weight = w, Names = n })
                                 .OrderByDescending(x => x.Weight);

            sortedWeights = combined.Select(x => x.Weight).ToList();
            sortedNames = combined.Select(x => x.Names).ToList();
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (isLoad)
            {
                ResizeControl(button1OriginalRectangle, button1);
                ResizeControl(button2OriginalRectangle, button2);
                ResizeControl(label1OriginalRectangle, label1);
                ResizeControl(label2OriginalRectangle, label2);
                ResizeControl(label3OriginalRectangle, label3);
                ResizeControl(label4OriginalRectangle, label4);
                ResizeControl(FishingTextBoxOriginalRectangle, FishingHistoryTextbox);
                ResizeControl(StatsTextBoxOriginalRectangle, StatsTextBox);
                ResizeControl(URLTextBoxOriginalrectangle, URLTextbox);
                ResizeControl(Checkbox1OriginalRectengle, UploadDataCheckbox);
                ResizeControl(Checkbox2OriginalRectengle, AllStatsCheckBox);
                ResizeControl(whiteListNumber, whitelistmultiplier);
                ResizeControl(whiteListWord, whitelistFish);
            }
        }

        private void ResizeControl(Rectangle r, System.Windows.Forms.Control c)
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
            int index;
            int length;

            List<Tuple<string, Color>> colorWords = new()
            {
                StatsTextBox.SelectionStart = index2;
                StatsTextBox.SelectionLength = length;
                StatsTextBox.SelectionColor = Color.Orange;
                StatsTextBox.Select(index, "Orange".Length);
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
            }

            int index2 = StatsTextBox.Find("Best of "); // Beginning of sentence

            foreach (Tuple<string, Color> colorWord in colorWords)
            {
                index = StatsTextBox.Find(colorWord.Item1);
                if (index >= 0)
                {
                    length = index + colorWord.Item1.Length - index2;
                    StatsTextBox.SelectionStart = index2;
                    StatsTextBox.SelectionLength = length;
                    StatsTextBox.SelectionColor = colorWord.Item2;
                    StatsTextBox.Select(index, colorWord.Item1.Replace(" planets:", "").Length);
                    StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);
                }
                index2 = StatsTextBox.Find("Best of ", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
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
                colorWords = new List<Tuple<string, Color>>()
                {
                    Tuple.Create("Greens ", Color.Green),
                    Tuple.Create("Blues ", Color.Blue),
                    Tuple.Create("Purples ", Color.Purple),
                    Tuple.Create("Oranges ", Color.Orange),
                    Tuple.Create("Reds ", Color.Red)
                };

                index2 = 0;

                foreach (Tuple<string, Color> colorWord in colorWords)
                {
                    index2 = StatsTextBox.Find(colorWord.Item1, index2, StatsTextBox.TextLength, RichTextBoxFinds.None) - 7;
                    index = StatsTextBox.Find(")", index2, StatsTextBox.TextLength, RichTextBoxFinds.None);
                    length = index + 1 - index2;
                    if (index >= 0)
                    {
                        StatsTextBox.SelectionStart = index2;
                        StatsTextBox.SelectionLength = length;
                        StatsTextBox.SelectionColor = colorWord.Item2;
                    }
                }
                // Make " all " bold and underline
                index = StatsTextBox.Find(" all ");
                StatsTextBox.SelectionStart = index + 1;
                StatsTextBox.SelectionLength = 3;
                StatsTextBox.SelectionFont = new Font(StatsTextBox.SelectionFont, FontStyle.Underline | FontStyle.Bold);

            }

            // Color the average Multipliers 
            index = StatsTextBox.Find("average multiplier: ");
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

                index = StatsTextBox.Find("average multiplier: ", index + length, StatsTextBox.TextLength, RichTextBoxFinds.None);
            }



            List<Tuple<string, Color>> bestWords = new()
            {
                Tuple.Create("BEST:", Color.FromArgb(255, 215, 0)),
                Tuple.Create("SECOND BEST:", Color.FromArgb(192, 192, 192)),
                Tuple.Create("THIRD BEST:", Color.FromArgb(205, 127, 50))
            };


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

        private void ColorWords2()
        {
            int length;
            int index = StatsTextBox.Find("average multiplier"); // Color the average Multipliers 
            while (index >= 0)
            {
                index += "average multiplier is: ".Length;
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
                index = StatsTextBox.Find("average multiplier", index, StatsTextBox.TextLength, RichTextBoxFinds.None);
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

            return (Url.ToLower(), rest);
        }



        private async Task<string> OutputStringAsync(string planet, float averageMulitplier, List<float> highestMultiplier,
                                    int GreenMulti, int BlueMulti, int PurpleMulti, int OrangeMulti, int RedMulti,
                                    int amount, string rest, string Domain, string selectedFish, List<float> selectedFishMultipliers, float selectedMultiplier
                                    )
        {
            string x = "";

            Payload pl = new()
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
            
            if(!string.IsNullOrEmpty(selectedFish))
            {
                if (selectedFish != "All Fish" && selectedFish != "Common" && selectedFish != "Rare" && selectedFish != "Epic" && selectedFish != "Legendary" && selectedFish != "Mythical")
                {
                    if (selectedFishMultipliers.Count != 0)
                    {
                        selectedFishMultipliers.Sort((x, y) => y.CompareTo(x));
                        x += $"\n{selectedFish}: \n";
                        for (int i = 0; i < selectedFishMultipliers.Count; i++)
                        { 
                            x += $"{selectedFishMultipliers[i]}{(i != selectedFishMultipliers.Count - 1 ? " |" : "")} ";
                        }
                    }
                    else
                    {
                        x += $"\nNo {selectedFish} caught";
                    }
                }
                else
                {
                    if (hoverMultiWords.Count != 0)
                    {
                        SortLists(hoverMultiWords, tooltipWords, out hoverMultiWords, out tooltipWords);
                        for(int i = 0; i < tooltipWords.Count; i++)
                        {
                            tooltipWords[i].Sort((x, y) => {
                                int i = 0;
                                while (i < x.Length && Char.IsDigit(x[i])) i++;
                                int j = 0;
                                while (j < y.Length && Char.IsDigit(y[j])) j++;
                                return int.Parse(y[..j]).CompareTo(int.Parse(x[..i]));
                            });
                        }
                        

                        
                        if(selectedFish == "All Fish")
                        {
                            x += $"\n{amountToolBox:n0} out of {pl.amount:n0} ({(float)amountToolBox / pl.amount * 100:00.00}%) fishes caught ";
                        }
                        else
                        {
                            x += $"\n{amountToolBox:n0} out of {pl.amount:n0} ({(float)amountToolBox / pl.amount * 100:00.00}%) {selectedFish}s caught ";
                        }

                        x += $"with a multiplier >= {selectedMultiplier}:\n";
                        for (int i = 0; i < hoverMultiWords.Count; i++)
                        {
                            x += hoverMultiWords[i] + " | ";
                        }
                    }
                    else
                    {
                        if(selectedFish == "All Fish")
                        {
                            x += $"\nNo fishes with a multiplier above {selectedMultiplier} caught";
                        }
                        else
                        {
                            x += $"\nNo {selectedFish}s with a multiplier above {selectedMultiplier} caught";
                        }
                    }
                }
                x += $"\n";
            }
            
            if (UploadDataCheckbox.Checked && !invalidURL)
            {
                pl = await DataAsync(pl, Domain, amount);

                x += "------------------------------------------------------------------\n";
                x += $"Alltime stats for https://{Domain}{(rest.Length > 0 ? $"/{rest}" : rest)}: \n\n";
                x = AppendX(x, pl.avrgMultiplier, pl.highestMultiplier, pl, pl.amount);
                x += $"Out of {pl.amount:n0} {(pl.amount >= 0 ? "fishes" : "fish")}\n";
            }

            return x;
        }

        private static string AppendX(string x, float averageMulitplier, List<float> highestMultiplier, Payload pl, int amount)
        {
            x += $"Your average multiplier is: {averageMulitplier:0.00000}\n";
            x += $"Your highest multipliers are: ";
            
            // Add the Top 3 highest multipliers
            for(int i = 0; i < highestMultiplier.Count; i++)
            {
                if(highestMultiplier[i] != 0)
                {
                    if(highestMultiplier[i] == 1)
                    {
                        x += highestMultiplier[i].ToString() + "   ";
                    }
                    else
                    {
                        x += highestMultiplier[i].ToString("F7").TrimEnd('0') + "   ";
                    }
                }
            }

            x += "\n\n";

            x += $"You've gotten an average of:\n";

            x += $"{(float)pl.amountGreen / amount * 100:00.00}% Commons\n";
            x += $"{(float)pl.amountBlue / amount * 100:00.00}% Rares\n";
            x += $"{(float)pl.amountPurple / amount * 100:00.00}% Epics\n";
            x += $"{(float)pl.amountOrange / amount * 100:00.00}% Legendarys\n";
            x += $"{(float)pl.amountRed / amount * 100:00.00}% Mythicals\n";

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
                StatsTextBox.Text += $"Best of {amountPlanets[i]:n0} {planettype} {"planets"}:\n\n";
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

                    if (planets[j].AvrgMultiplier == 0)
                    {
                        StatsTextBox.Text += $"Currently there is no more {planettype} planet.\n\n";
                    }
                    else
                    {
                        StatsTextBox.Text +=
                        $"{planets[j].PlanetName}" +
                        $"\n\n" +
                        $"{planets[j].Amount:n0} {(planets[j].Amount == 1 ? "fish" : "fishes")} caught" +
                        $"\naverage multiplier: {planets[j].AvrgMultiplier:0.00000}";
                        StatsTextBox.Text += $"\nhighest multiplier: ";
                        for(int k = 0; k < planets[j].HighestMultiplier.Count; k++)
                        {
                            StatsTextBox.Text += $"{planets[j].HighestMultiplier[k]:0.0000}   ";
                        }
                        StatsTextBox.Text += "\n\n\n";
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
                payload.highestMultiplier = GetTop3Highest(pl.highestMultiplier, payload.highestMultiplier);
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

        private static List<float> GetTop3Highest(List<float> list1, List<float> list2)
        {
            // Combine the two lists into one
            var combinedList = list1.Concat(list2);
            // Order the combined list in descending order by value
            var top3 = combinedList.OrderByDescending(f => f).Take(3);
            // return the top 3 highest floats
            return top3.ToList();
        }

        private (string, List<Fish>, List<float>, List<float>) GetPlanet(List<Fish> Fishes, string[] lines, string selectedFish, float selectedMulti)
        {
            Planet planettemp = Planet.Null;
            List<float> Multipliers = new();
            List<float> selectedFishMultipliers = new();
            Regex nameRegex = new(@"(?<=\[).*?(?=\])");
            Regex weightRegex = new(@"(?<=weighs ).*?(?= pounds)");

            foreach (string line in lines)
            {
                string nameMatch = nameRegex.Match(line).ToString();
                string weightMatch = weightRegex.Match(line).ToString();

            for (int i = 0; i < lines.Length; i++)
            {
                j = 14;
                name = "";
                number = "";

                        if (selectedFish == "All Fish" || nameMatch == selectedFish)
                        {
                            if (multi >= selectedMulti)
                            {
                                string temp = multi.ToString("F7").TrimEnd('0');
                                if (temp == "1,") temp = "1";
                                amountToolBox++;

                                if (nameMatch == selectedFish)
                                {
                                    selectedFishMultipliers.Add(multi);
                                }
                                else
                                {
                                    AddFishToTooltipWords(temp, nameMatch);
                                }
                            }
                        }
                        else
                        {
                            var rarityMapping = new Dictionary<string, Rarity>()
                            {
                                {"Common", Rarity.Green},
                                {"Rare", Rarity.Blue},
                                {"Epic", Rarity.Purple},
                                {"Legendary", Rarity.Orange},
                                {"Mythical", Rarity.Red}
                            };

                            if (rarityMapping.TryGetValue(selectedFish, out Rarity rarity) && Fishes[k].rarity == rarity && multi >= selectedMulti)
                            {
                                HandleMatchedFish();
                            }
                        }
                        void HandleMatchedFish()
                        {
                            string temp = multi.ToString("F7").TrimEnd('0');
                            if (temp == "1,") temp = "1";
                            amountToolBox++;

                            AddFishToTooltipWords(temp, nameMatch);
                        }

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

                        Multipliers.Add(multi);

                        if (planettemp == Planet.Null)
                        {
                            planettemp = Fishes[k].planet;
                        }
                        else if(planettemp == Planet.Rift && Fishes[k].planet != Planet.Rift)
                        {
                            planettemp = Fishes[k].planet;
                        }
                        else if (planettemp != Fishes[k].planet && planettemp != Planet.Rift && Fishes[k].planet != Planet.Rift)
                        {
                            planettemp = Planet.Ocean;
                        }

                        break;  // If Fish was found we can stop looping threw the rest 
                    }
                    else if (k == Fishes.Count - 1)
                    {
                        Console.WriteLine(nameMatch);
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
                case Planet.Gray:
                    planet = "Gray";
                    break;
                case Planet.Purple:
                    planet = "Purple";
                    break;
                case Planet.Rift:
                    planet = "Rift";
                    break;
                case Planet.Null:
                    planet = "Null";
                    break;
            }

            return (planet, Fishes, Multipliers, selectedFishMultipliers);
        }

        private void AddFishToTooltipWords(string multi, string nameMatch)
        {
            bool found = false;
            for (int i = 0; i < hoverMultiWords.Count; i++)
            {
                if (hoverMultiWords[i] == multi)
                {
                    for (int j = 0; j < tooltipWords[i].Count; j++)
                    {
                        if (tooltipWords[i][j].Contains(nameMatch))
                        {
                            int y = 0;
                            string conv = "";
                            while (Char.IsDigit(tooltipWords[i][j][y]))
                            {
                                conv += tooltipWords[i][j][y];
                                y++;
                            }
                            int amt = int.Parse(conv) + 1;
                            tooltipWords[i][j] = string.Concat(amt.ToString(), tooltipWords[i][j].AsSpan(y));
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        tooltipWords[i].Add("1x " + nameMatch);
                    }
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                tooltipWords.Add(new List<string> { "1x " + nameMatch });
                hoverMultiWords.Add(multi);
            }
        }

        private void InsertWeight(float newWeight, string newName)
        {
            // Check if the new weight is greater than any of the existing weights
            int insertIndex = -1;
            for (int i = 0; i < top3Multis.Length; i++)
            {
                if (newWeight > top3Multis[i])
                {
                    insertIndex = i;
                    break;
                }
            }

            // If the new weight is greater than any of the existing weights
            if (insertIndex != -1)
            {
                // Insert the new weight and name at the correct position
                for (int i = top3Multis.Length - 1; i > insertIndex; i--)
                {
                    top3Multis[i] = top3Multis[i - 1];
                    top3Names[i] = top3Names[i - 1];
                }
                top3Multis[insertIndex] = newWeight;
                top3Names[insertIndex] = newName;
            }
        }

        private static float CalcMulti(float multi)
        {
            if (multi > 2)
            {
                while (multi >= 1)
                {
                    multi /= 10;
                }
                multi *= 10;
            }
            return multi;
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
            List<Payload> pl = new();
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
                            URLSubstring = "https://" + pl2.Domains[i] + (Planet.URLSubstring.Length == 0 ? "" : "/" + Planet.URLSubstring)
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
                        URLSubstring = "https://" + pl2.Domains[i] + (Planet.URLSubstring.Length == 0 ? "" : "/" + Planet.URLSubstring)
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
        Gray,
        Purple,
        Rift,
        Null
    }

}