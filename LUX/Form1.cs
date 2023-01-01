using Google.Cloud.Firestore;
using System.Security.Policy;

namespace LUX
{
    public partial class Form1 : Form
    {
        protected FirestoreDb db;
        List<float> GreenMulti = new();
        List<float> BlueMulti = new();
        List<float> PurpleMulti = new();
        List<float> OrangeMulti = new();
        List<float> RedMulti = new();
        string previousMessage = "";
        //                                        Adding all the Fishes                                               \\
        List<Fish> Fishes = new();

        public Form1()
        {
            InitializeComponent();
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
            byte[] resourceBytes = Properties.Resources.cloudfire;
            
            // Write the resource to a temporary file
            string tempPath = Path.GetTempFileName();
            File.WriteAllBytes(tempPath, resourceBytes);
            
            // Set the GOOGLE_APPLICATION_CREDENTIALS environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

            // GoogleCredential.FromFile(path);
            db = FirestoreDb.Create("luxfishing-1b3b6"); / /IMPORTANT
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            // Resetting the Multipliers so they don't get stacked
            GreenMulti = new List<float>(); 
            BlueMulti = new List<float>();
            PurpleMulti = new List<float>();
            OrangeMulti = new List<float>();
            RedMulti = new List<float>();
            
            //                                        Adding all the Fishes                                               \\
            Fishes = new Fish().GetFishList(GreenMulti, BlueMulti, PurpleMulti, OrangeMulti, RedMulti);

            string temp = URLTextbox.Text.ToLower();
            string Message = FishingHistoryTextbox.Text;

            if(CheckTextboxesEmpty(temp, Message))
            {
                return;
            }

            if(previousMessage == Message)
            {
                label1.Text = "Do not enter the same Text twice:";
                label1.ForeColor = System.Drawing.Color.Red;
                return;
            }

            label1.Text = "Enter your fishing history";
            label1.ForeColor = System.Drawing.Color.Black;
            label2.Text = "Enter The URL you were fishing on:";
            label2.ForeColor = System.Drawing.Color.Black;

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
            var result = await new BestPlanet().GetBestPlanetsAsync(db); 
            AppendTextbox(result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6, result.Item7, result.Item8);
        }


        private bool CheckTextboxesEmpty(string temp, string Message)
        {
            // Check if Text boxes are empty \\
            if (temp.Length == 0)
            {
                label2.Text = "Please enter the URL you were fishing on:";
                label2.ForeColor = System.Drawing.Color.Red;
                return true;
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

            string Pre = "https://www.";
            string Pre2 = "www.";
            string Pre3 = "https://www";
            string Pre4 = "https://";

            // Check if URL starts with any of the patterns (Format Domain of URL)
            if (Url.StartsWith(Pre))
            {
                Url = Url.Replace(Pre, "");
            }
            else if (Url.StartsWith(Pre2))
            {
                Url = Url.Replace(Pre2, "");
            }
            else if (Url.StartsWith(Pre3))
            {
                Url = Url.Replace(Pre3, "");
            }
            else if (Url.StartsWith(Pre4))
            {
                Url = Url.Replace(Pre4, "");
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
                                    List<float> GreenMulti, List<float> BlueMulti, List<float> PurpleMulti, List<float> OrangeMulti, List<float> RedMulti,
                                    int amount, string rest, string Domain
                                    )
        {
            List<float> Multipliers;
            string x = "";

            Payload pl = new Payload
            {
                GreenMultipliers = GreenMulti,
                PurpleMultipliers = PurpleMulti,
                OrangeMultipliers = OrangeMulti,
                RedMultipliers = RedMulti,
                BlueMultipliers = BlueMulti,
                HighestMultiplier = highestMultiplier,
                URLSubstring = rest,
                Planet = planet
            };


            x += $"Session stats ({planet}): \n";
            x = AppendX(x, averageMulitplier, highestMultiplier, pl, amount);
            x += $"{$"For {amount} {(amount >= 0 ? "fishes" : "fish")}",-30} {GreenMulti.Count * 100 + BlueMulti.Count * 200 + PurpleMulti.Count * 1000 + OrangeMulti.Count * 20000 + RedMulti.Count * 1000000:n0} Credits\n";


            pl = await DataAsync(pl, Domain, amount);

            Multipliers = pl.GreenMultipliers.Concat(pl.BlueMultipliers).Concat(pl.PurpleMultipliers).Concat(pl.OrangeMultipliers).Concat(pl.RedMultipliers).ToList();
            amount = Multipliers.Count;

            // get avrg Multiplier 
            averageMulitplier = (float)(Multipliers.Count > 0 ? Multipliers.Average() : 0.0);
            // get highest multiplier
            highestMultiplier = (float)(Multipliers.Count > 0 ? Multipliers.Max() : 0.0);

            x += "--------------------------------------------------------------------\n";
            x += $"Alltime Stats for {Domain}{(rest.Length > 0 ? $"/{rest}" : rest)}: \n\n";
            x = AppendX(x, averageMulitplier, highestMultiplier, pl, amount);
            x += $"For {amount} {(amount >= 0 ? "Fishes" : "Fish")}\n";

            return x;
        }

        private static string AppendX(string x, double averageMulitplier, double highestMultiplier, Payload pl, int amount)
        {
            x += $"Your average multiplier is: {averageMulitplier:0.00000000}\n";
            x += $"Your highest multiplier is: {highestMultiplier:0.00}\n";

            x += "\n";

            x += "That's an average multiplier of:\n";

            x += $"{(pl.GreenMultipliers.Count > 0 ? pl.GreenMultipliers.Average().ToString("0.0000") + " Greens." : "You have caught too few green fish.")}\n";
            x += $"{(pl.BlueMultipliers.Count > 0 ? pl.BlueMultipliers.Average().ToString("0.0000") + " Blues." : "You have caught too few blue fish")}\n";
            x += $"{(pl.PurpleMultipliers.Count > 0 ? pl.PurpleMultipliers.Average().ToString("0.0000") + " Purples." : "You have caught too few purple fish")}\n";
            x += $"{(pl.OrangeMultipliers.Count > 0 ? pl.OrangeMultipliers.Average().ToString("0.0000") + " Oranges." : "You have caught too few orange fish")}\n";
            x += $"{(pl.RedMultipliers.Count > 0 ? pl.RedMultipliers.Average().ToString("0.0000") + " Reds." : "You have caught too few red fish")}\n";

            x += $"\n";

            x += $"You've gotten an average of:\n";

            x += $"{(float)pl.GreenMultipliers.Count / amount * 100:00.00}% Greens\n";
            x += $"{(float)pl.BlueMultipliers.Count / amount * 100:00.00}% Blues\n";
            x += $"{(float)pl.PurpleMultipliers.Count / amount * 100:00.00}% Purples\n";
            x += $"{(float)pl.OrangeMultipliers.Count / amount * 100:00.00}% Oranges\n";
            x += $"{(float)pl.RedMultipliers.Count / amount * 100:00.00}% Reds\n";

            x += "\n";

            return x;
        }




        private void AppendTextbox(List<BestPlanet> bestPlanets, int[] amountPlanets, int amountGreen, int amountBlue, int amountPurple, int amountOrange, int amountRed, List<float> avrgMultiplier)
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
                        $"{amountPlanets[i]} {bestPlanets[i].PlanetType} {(amountPlanets[i] == 1 ? "planet" : "planets")} in db".PadRight(40) +
                        $"{bestPlanets[i].Amount} {(bestPlanets[i].Amount == 1 ? "fish" : "fishes")} caught" +
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
                        $"avarege multiplier: {avrgMultiplier.Average():0.0000}" +
                        $"\n" +
                        $"highest multiplier: {avrgMultiplier.Max():0.000}" +
                        $"\n" +
                        $"{(float)amountGreen / amount * 100:00.00}% Greens".PadRight(15) +
                        $"({amountGreen} fishes)\n" +
                        $"{(float)amountBlue / amount * 100:00.00}% Blues".PadRight(15) +
                        $"({amountBlue} fishes)\n" +
                        $"{(float)amountPurple / amount * 100:00.00}% Purples".PadRight(15) +
                        $"({amountPurple} fishes)\n" +
                        $"{(float)amountOrange / amount * 100:00.00}% Oranges".PadRight(15) +
                        $"({amountOrange} fishes)\n" +
                        $"{(float)amountRed / amount * 100:00.00}% Reds".PadRight(15) +
                        $"({amountRed} fishes)\n" +
                        $"{amount} Fishes caught."
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
                string ID = capitalQuerySnapshot[0].Id; //Get ID of Document (for changing later on)

                Payload pl = capitalQuerySnapshot[0].ConvertTo<Payload>();
                payload.GreenMultipliers = payload.GreenMultipliers.Concat(pl.GreenMultipliers).ToList();
                payload.BlueMultipliers = payload.BlueMultipliers.Concat(pl.BlueMultipliers).ToList();
                payload.PurpleMultipliers = payload.PurpleMultipliers.Concat(pl.PurpleMultipliers).ToList();
                payload.OrangeMultipliers = payload.OrangeMultipliers.Concat(pl.OrangeMultipliers).ToList();
                payload.RedMultipliers = payload.RedMultipliers.Concat(pl.RedMultipliers).ToList();
                payload.HighestMultiplier = payload.HighestMultiplier < pl.HighestMultiplier ? pl.HighestMultiplier : payload.HighestMultiplier;
                
                if(payload.Planet == "Null" || amount <= 4) //Gotta check this
                {
                    payload.Planet = pl.Planet;
                }

                //Update Values for URL:
                DocumentReference document = db.Collection(url).Document(ID);
                await document.UpdateAsync("URLSubstring", payload.URLSubstring);
                await document.UpdateAsync("GreenMultipliers", payload.GreenMultipliers);
                await document.UpdateAsync("BlueMultipliers", payload.BlueMultipliers);
                await document.UpdateAsync("PurpleMultipliers", payload.PurpleMultipliers);
                await document.UpdateAsync("OrangeMultipliers", payload.OrangeMultipliers);
                await document.UpdateAsync("RedMultipliers", payload.RedMultipliers);
                await document.UpdateAsync("HighestMultiplier", payload.HighestMultiplier);
                await document.UpdateAsync("Planet", payload.Planet);
            }

            return payload;
        }



        private static (string, List<Fish>, List<float>) GetPlanet(List<Fish> Fishes, string[] lines)
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
                        if(float.Parse(number) / Fishes[k].minWeigth < 1000) // There was a problem when people are coming from europe / america because of the decimal '.' / ',' 
                        {
                            Multipliers.Add(float.Parse(number) / Fishes[k].minWeigth);
                            Fishes[k].Multipliers.Add(float.Parse(number) / Fishes[k].minWeigth);
                        }
                        else
                        {
                            Multipliers.Add((float.Parse(number) / Fishes[k].minWeigth) / 1000);
                            Fishes[k].Multipliers.Add((float.Parse(number) / Fishes[k].minWeigth) / 1000);
                        }

                        if (planettemp == Planet.Null)
                        {
                            planettemp = Fishes[k].planet;
                        }
                        else if (planettemp != Fishes[k].planet)
                        {
                            planettemp = Planet.Ocean;
                        }
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