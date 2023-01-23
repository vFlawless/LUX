using Newtonsoft.Json;
using System.Net;
using static LUX.Fish;

namespace LUX
{
    public class Fish
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("rarity")]
        public Rarity rarity { get; set; }

        [JsonProperty("weight")]
        public int minWeight { get; set; }

        [JsonProperty("planet")]
        public Planet planet { get; set; }

        public Fish(string name = "", Rarity rarity = Rarity.Red, int minWeight = 0, Planet planet = Planet.Null)
        {
            this.name = name;
            this.rarity = rarity;
            this.minWeight = minWeight;
            this.planet = planet;
        }

        public class FishData
        {
            public List<FishJson>? fishes;
        }

        // Define a separate class for the fish data in the JSON file
        public class FishJson
        {
            public string? name;
            public string? rarity;
            public int weight;
            public string? planet;
        }

        public List<Fish> GetFishList()
        {
            // Deserialize the JSON data into a C# object
            WebClient webClient = new();
            string url = "https://pastebin.com/raw/kKdJ9HWC";
            string json = webClient.DownloadString(url);
            FishData fishData = JsonConvert.DeserializeObject<FishData>(json);
            fishData.fishes.RemoveAll(x => x.weight == 0);


            // Convert the fish data to a list of Fish objects
            List<Fish> fishes = new List<Fish>();
            foreach (FishJson fishJson in fishData.fishes)
            {
                // Convert the rarity and planet strings to enums
                Rarity rarity = (Rarity)Enum.Parse(typeof(Rarity), fishJson.rarity, ignoreCase: true);
                Planet planet = (Planet)Enum.Parse(typeof(Planet), fishJson.planet, ignoreCase: true);

                // Create a new Fish object and add it to the list
                fishes.Add(new Fish(fishJson.name, rarity, fishJson.weight, planet));
            }

            return fishes;
        }
    }
}
