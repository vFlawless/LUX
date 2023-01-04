using Google.Cloud.Firestore;

namespace LUX
{
    public class BestPlanet
    {
        public string PlanetType;
        public string PlanetName;
        public float AvrgMultiplier;
        public float HighestMultiplier;
        public int Amount;

        public BestPlanet(string PlanetType = "", string PlanetName = "", float AvrgMultiplier = 0.0f, float HighestMultiplier = 0.0f, int Amount = 0)
        {
            this.PlanetType = PlanetType;
            this.PlanetName = PlanetName;
            this.AvrgMultiplier = AvrgMultiplier;
            this.HighestMultiplier = HighestMultiplier;
            this.Amount = Amount;
        }

        public async Task<(List<BestPlanet>, int[], int, int, int, int, int, float, float)> GetBestPlanetsAsync(FirestoreDb db)
        {
            // Variables for best planets
            float avrgMultiplier = 0;
            float highestMulitplier = 0;
            int amountGreen = 0;
            int amountBlue = 0;
            int amountPurple = 0;
            int amountOrange = 0;
            int amountRed = 0;

            // Get all Collections with Planets (Domains)
            DocumentReference doc = db.Collection("URLS").Document("Domain"); // Collection with the names of the other collections (bases for leaderboards)
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();


            Payload2 pl = snapshot.ConvertTo<Payload2>();


            int[] amountPlanets = new int[7];
            List<BestPlanet> bestPlanets = new(); //0 = Yellow, 1 = Orange, 2 = Grey, 3 = Blue, 4 = Red, 5 = Purple, 6 = Ocean
            string[] planetTypes = new string[] { "Yellow", "Orange", "Grey", "Blue", "Red", "Purple", "Ocean" };
            Dictionary<string, float> planetTyper = new Dictionary<string, float>
            {
                { "Yellow", 0},
                { "Orange", 0},
                { "Grey", 0},
                { "Blue", 0},
                { "Red", 0},
                { "Purple", 0},
                { "Ocean", 0},
            };

            //Initialize List
            for (int i = 0; i < planetTypes.Length; i++)
            {
                bestPlanets.Add(new BestPlanet(PlanetType: planetTypes[i]));
            }

            for (int i = 0; i < pl.Domains.Count; i++)     // For every collection (domain of URL)
            {
                for (int j = 0; j < planetTypes.Length; j++) // For every PlanetType in that collection
                {
                    Query capitalQuery = db.Collection(pl.Domains[i]).WhereEqualTo("planet", planetTypes[j]); // Get Every Planet that is the right PlanetType
                    QuerySnapshot snapshots = await capitalQuery.GetSnapshotAsync();
                    amountPlanets[j] += snapshots.Count;

                    for (int k = 0; k < snapshots.Count; k++)    // Every Planet with that type 
                    {
                        // Safe data of all planets (for when checkbox is checked):
                        Payload Planet = snapshots[k].ConvertTo<Payload>();
                        amountGreen += Planet.amountGreen;
                        amountBlue += Planet.amountBlue;
                        amountPurple += Planet.amountPurple;
                        amountOrange += Planet.amountOrange;
                        amountRed += Planet.amountRed;
                        avrgMultiplier += Planet.avrgMultiplier * Planet.amount;
                        highestMulitplier = Planet.highestMultiplier > highestMulitplier ? Planet.highestMultiplier : highestMulitplier;

                        //MessageBox.Show(planetTypes[j] + ":   " + best.ToString() + "  " + Planet.highestMultiplier.ToString());
                        if (Planet.highestMultiplier > planetTyper[planetTypes[j]]) // If Highest multiplier is > than highest Multi before
                        {
                            //MessageBox.Show("reached");
                            bestPlanets[j] = new BestPlanet(
                                PlanetName: $"{pl.Domains[i]}{(Planet.URLSubstring.Length == 0 ? "" : $"/{Planet.URLSubstring}")}",
                                PlanetType: planetTypes[j],
                                AvrgMultiplier: Planet.avrgMultiplier,
                                HighestMultiplier: Planet.highestMultiplier,
                                Amount: Planet.amount
                                );
                            planetTyper[planetTypes[j]] = Planet.highestMultiplier;
                            //MessageBox.Show(best.ToString());
                        }
                    }
                }
            }
            avrgMultiplier /= amountGreen + amountBlue + amountPurple + amountOrange + amountRed;
            return (bestPlanets, amountPlanets, amountGreen, amountBlue, amountPurple, amountOrange, amountRed, avrgMultiplier, highestMulitplier);
        }
    }
}
