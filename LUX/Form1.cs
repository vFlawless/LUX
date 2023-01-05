using Google.Cloud.Firestore;
using System.Net;
using System.Diagnostics;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;

namespace LUX
{
    public partial class Form1 : Form
    {
        protected FirestoreDb db;
        string regex = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
        string currentVersion = "1.2";
        int GreenMulti;
        int BlueMulti;
        int PurpleMulti;
        int OrangeMulti;
        int RedMulti;
        string previousMessage = "";
        bool previously_checked = false;
        //                                        Adding all the Fishes                                               \\
        List<Fish> Fishes = new();

        public Form1()
        {
            InitializeComponent();

            WebClient webClient = new WebClient();

            try
            {
                if (!webClient.DownloadString("https://pastebin.com/raw/F5sf1aJ7").Contains(currentVersion)){
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "LUX Fishing Tool V" + currentVersion;

            byte[] resourceBytes = Properties.Resources.cloudfire;

            // Write the resource to a temporary file
            string tempPath = Path.GetTempFileName();
            File.WriteAllBytes(tempPath, resourceBytes);

            // Set the GOOGLE_APPLICATION_CREDENTIALS environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

            // GoogleCredential.FromFile(path);
            db = FirestoreDb.Create("");
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

            if(CheckTextboxesEmpty(temp, Message))
            {
                return;
            }

            label1.Text = "Enter your fishing history";
            label1.ForeColor = System.Drawing.Color.Black;
            label2.Text = "Enter the URL you were fishing on:";
            label2.ForeColor = System.Drawing.Color.Black;
            
            if(previousMessage == Message && UploadDataCheckbox.Checked && previously_checked)
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
                new string[] {"\n\n"},
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

            StatsTextBox.Text = await OutputStringAsync(planet, averageMulitplier, highestMultiplier, GreenMulti, BlueMulti, PurpleMulti, OrangeMulti, RedMulti, amount, rest, Domain);;
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            // This Button is for showing the best Planets
            FishingHistoryTextbox.Text = "";
            var result = await new BestPlanet().GetBestPlanetsAsync(db); 
            AppendTextbox(result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6, result.Item7, result.Item8, result.Item9);
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

            string[] Pre = { "https://www.", "www.", "https://www", "https://", "https:/", "https:", "http://www.", "http://www", "http://", "http:/", "http:" };

            // Check if URL starts with any of the patterns (Format Domain of URL)
            
            for(int i = 0; i < Pre.Length; i++)
            {
                if (Url.StartsWith(Pre[i]))
                {
                    Url = Url.Replace(Pre[i], "");
                }
            }
            
            //only start of Link == collection, document (with random ID) contains rest of link:
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
            x += $"{$"For {pl.amount:n0} {(pl.amount >= 0 ? "fishes" : "fish")}",-30} {pl.amountGreen * 100 + pl.amountBlue * 200 + pl.amountPurple * 1000 + pl.amountOrange * 20000 + pl.amountRed * 1000000:n0} Credits\n";

            if (UploadDataCheckbox.Checked)
            {
                pl = await DataAsync(pl, Domain, amount);

                x += "--------------------------------------------------------------------\n";
                x += $"Alltime Stats for {Domain}{(rest.Length > 0 ? $"/{rest}" : rest)}: \n\n";
                x = AppendX(x, pl.avrgMultiplier, pl.highestMultiplier, pl, pl.amount);
                x += $"For {pl.amount:n0} {(pl.amount >= 0 ? "Fishes" : "Fish")}\n";
            }
            
            return x;
        }

        private static string AppendX(string x, double averageMulitplier, double highestMultiplier, Payload pl, int amount)
        {
            x += $"Your average multiplier is: {averageMulitplier:0.00000}\n";
            x += $"Your highest multiplier is: {highestMultiplier:0.000}\n";

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




        private void AppendTextbox(List<BestPlanet> bestPlanets, int[] amountPlanets, int amountGreen, int amountBlue, int amountPurple, int amountOrange, int amountRed, float avrgMultiplier, float highestMultiplier)
        {
            StatsTextBox.Text = "";

            for (int i = 0; i < bestPlanets.Count; i++)
            {
                // We want a difference of 40, 40 = first Text + White Spaces


                if (bestPlanets[i].PlanetName != "")
                {
                    StatsTextBox.Text +=
                        $"Best {bestPlanets[i].PlanetType} planet:".PadRight(40) +
                        $"{bestPlanets[i].PlanetName}" +
                        $"\n" +
                        $"{amountPlanets[i]:n0} {bestPlanets[i].PlanetType} {(amountPlanets[i] == 1 ? "planet" : "planets")} in db".PadRight(40) +
                        $"{bestPlanets[i].Amount:n0} {(bestPlanets[i].Amount == 1 ? "fish" : "fishes")} caught" +
                        $"\naverage multiplier: {bestPlanets[i].AvrgMultiplier:0.0000}" +
                        $"\nhighest multiplier: {bestPlanets[i].HighestMultiplier:0.000} \n\n--------------------------------------------------------------------\n\n";
                }
                else
                {
                    StatsTextBox.Text += $"Currently there is no {bestPlanets[i].PlanetType} planet in the database. \n\n--------------------------------------------------------------------\n\n";
                }
            }

            if (AllStatsCheckBox.Checked)
            {
                int amount = amountGreen + amountBlue + amountPurple + amountOrange + amountRed;
                // Show stats of all planets
                StatsTextBox.Text +=
                        $"--------------------------------------------------------------------\n\n\n" + 
                        $"Stats for all {amountPlanets.Sum()} planets" + $"\n" +
                        $"\n" +
                        $"avarege multiplier: {avrgMultiplier:0.0000}" +
                        $"\n" +
                        $"highest multiplier: {highestMultiplier:0.000}" +
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
                        $"{amount:n0} Fishes caught.        {amountGreen * 100 + amountBlue * 200 + amountPurple * 1000 + amountOrange * 20000 + amountRed * 1000000:n0} Credits"
                        ;
            }
        }


        



        async Task<Payload> DataAsync(Payload payload, string url, int amount)
        {
            Query capitalQuery = db.Collection(url).WhereEqualTo("URLSubstring", payload.URLSubstring);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
            
            if(capitalQuerySnapshot.Count == 0)
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
                    planet = "Pruple";
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
            for(int i = 0; i < pl.Domains.Count; i++)
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