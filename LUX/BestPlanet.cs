using Google.Cloud.Firestore;

namespace LUX
{
    public class BestPlanet
    {
        public string PlanetName;
        public float AvrgMultiplier;
        public List<float> HighestMultiplier;
        public int Amount;

        public BestPlanet(string PlanetName = "", float AvrgMultiplier = 0.0f, List<float>? HighestMultiplier = null, int Amount = 0)
        {
            this.PlanetName = PlanetName;
            this.AvrgMultiplier = AvrgMultiplier;
            this.HighestMultiplier = HighestMultiplier ?? new List<float>();
            this.Amount = Amount;
        }

        public async Task<(Dictionary<string, BestPlanet[]>, int[], int, int, int, int, int, float, float)> GetBestPlanetsAsync(FirestoreDb db)
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
            string[] planetTypes = new string[] { "Yellow", "Orange", "Gray", "Blue", "Red", "Purple", "Ocean" };
            Dictionary<string, float[]> planetHighest = new Dictionary<string, float[]>
            {
                { "Yellow", new float[3]},
                { "Orange", new float[3]},
                { "Gray", new float[3]},
                { "Blue", new float[3]},
                { "Red", new float[3]},
                { "Purple", new float[3]},
                { "Ocean", new float[3]},
            };

            //Initialize List
            Dictionary<string, BestPlanet[]> bestPlanets = new()
            {
                { "Yellow", initBestP() },
                { "Orange", initBestP() },
                { "Gray", initBestP() },
                { "Blue", initBestP() },
                { "Red", initBestP() },
                { "Purple", initBestP() },
                { "Ocean", initBestP() },
            };

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
                        highestMulitplier = Planet.highestMultiplier[0] > highestMulitplier ? Planet.highestMultiplier[0] : highestMulitplier;

                        for (int l = 0; l < planetHighest[planetTypes[j]].Length; l++)   // Loop threw array to see if there are some missing or if the current Planet is better then some in there
                        {
                            if (planetHighest[planetTypes[j]][l] == 0 || Planet.avrgMultiplier > planetHighest[planetTypes[j]][l])
                            {
                                BestPlanet bestP = new BestPlanet(
                                PlanetName: $"https://{pl.Domains[i]}{(Planet.URLSubstring.Length == 0 ? "" : $"/{Planet.URLSubstring}")}",
                                AvrgMultiplier: Planet.avrgMultiplier,
                                HighestMultiplier: Planet.highestMultiplier,
                                Amount: Planet.amount
                                );
                                var res = UpdateScoreAndPlanets(planetHighest[planetTypes[j]], l, Planet.avrgMultiplier, bestPlanets[planetTypes[j]], bestP);
                                planetHighest[planetTypes[j]] = res.Item1;
                                bestPlanets[planetTypes[j]] = res.Item2;
                                break;
                            }
                        }
                    }
                }
            }
            avrgMultiplier /= amountGreen + amountBlue + amountPurple + amountOrange + amountRed;
            return (bestPlanets, amountPlanets, amountGreen, amountBlue, amountPurple, amountOrange, amountRed, avrgMultiplier, highestMulitplier);
        }


        private (float[], BestPlanet[]) UpdateScoreAndPlanets(float[] scores, int position, float score, BestPlanet[] bestPlanets, BestPlanet planet)
        {
            for (int i = scores.Length - 1; i > position; i--)
            {
                scores[i] = scores[i - 1];
                bestPlanets[i] = bestPlanets[i - 1];
            }

            scores[position] = score;
            bestPlanets[position] = planet;

            return (scores, bestPlanets);
        }

        private BestPlanet[] initBestP()
        {
            BestPlanet[] bestPlanets = new BestPlanet[3]
            {
                new BestPlanet(),
                new BestPlanet(),
                new BestPlanet(),
            };

            return bestPlanets;
        }
    }
}
