using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUX
{
    public class Fish : Form1
    {
        public List<float> Multipliers = new();
        public string name;
        public Rarity rarity;
        public int minWeigth;
        public Planet planet;

        public Fish(string name = "", Rarity rarity = Rarity.Red, int minWeigth = 0, Planet planet = Planet.Null)
        {
            this.name = name;
            this.rarity = rarity;
            this.minWeigth = minWeigth;
            this.planet = planet;
        }

        public List<Fish> GetFishList(List<float> GreenMulti, List<float> BlueMulti, List<float> PurpleMulti, List<float> OrangeMulti,List<float> RedMulti)
        {
            List<Fish> Fishes = new()
            {
                    //Greens\\
                    new Fish("Mud Minnow", Rarity.Green, 1, Planet.Orange),
                    new Fish("Ray Carp", Rarity.Green, 3, Planet.Orange),
                    new Fish("Sardune", Rarity.Green, 2, Planet.Orange),
                    new Fish("Dust Fish", Rarity.Green, 1, Planet.Yellow),
                    new Fish("Dune Runner", Rarity.Green, 6, Planet.Yellow),
                    new Fish("Gasper", Rarity.Green, 4, Planet.Yellow),
                    new Fish("Green Pinstripe", Rarity.Green, 4, Planet.Blue),
                    new Fish("Silverskin", Rarity.Green, 10, Planet.Blue),
                    new Fish("Plump", Rarity.Green, 2, Planet.Blue),
                    new Fish("Red Peeper", Rarity.Green, 3, Planet.Red),
                    new Fish("Firefly", Rarity.Green, 5, Planet.Red),
                    new Fish("Ruby Flounder", Rarity.Green, 9, Planet.Red),
                    new Fish("Finster", Rarity.Green, 2, Planet.Grey),
                    new Fish("Lunar Gupper", Rarity.Green, 4, Planet.Grey),
                    new Fish("Rock Skipper", Rarity.Green, 6, Planet.Grey),
                    new Fish("Plume", Rarity.Green, 4, Planet.Purple),
                    new Fish("Surge Runner", Rarity.Green, 9, Planet.Purple),
                    new Fish("Flow Fin", Rarity.Green, 6, Planet.Purple),

                    //Blues\\
                    new Fish("Gleam Clam", Rarity.Blue, 2, Planet.Orange),
                    new Fish("Sandstone Fish", Rarity.Blue, 5, Planet.Orange),
                    new Fish("Red Yapp", Rarity.Blue, 7, Planet.Orange),
                    new Fish("Sunshade", Rarity.Blue, 10, Planet.Orange),
                    new Fish("Muk Puppy", Rarity.Blue, 14, Planet.Yellow),
                    new Fish("Targill", Rarity.Blue, 3, Planet.Yellow),
                    new Fish("Moon Clam", Rarity.Blue, 2, Planet.Yellow),
                    new Fish("Yellowtail", Rarity.Blue, 7, Planet.Yellow),
                    new Fish("Inkster", Rarity.Blue, 9, Planet.Blue),
                    new Fish("Pulser", Rarity.Blue, 6, Planet.Blue),
                    new Fish("Blue Umbra", Rarity.Blue, 73, Planet.Blue),
                    new Fish("Violet Skimmer", Rarity.Blue, 5, Planet.Blue),
                    new Fish("Cherry Dan", Rarity.Blue, 15, Planet.Red),
                    new Fish("Rayleigh Tail", Rarity.Blue, 7, Planet.Red),
                    new Fish("Lavaring", Rarity.Blue, 11, Planet.Red),
                    new Fish("Crimson Dolphin", Rarity.Blue, 65, Planet.Red),
                    new Fish("Speckler", Rarity.Blue, 7, Planet.Grey),
                    new Fish("Silverfin", Rarity.Blue, 10, Planet.Grey),
                    new Fish("Glowsie", Rarity.Blue, 13, Planet.Grey),
                    new Fish("Hooknose", Rarity.Blue, 8, Planet.Grey),
                    new Fish("UV Floater", Rarity.Blue, 12, Planet.Purple),
                    new Fish("Plasma Pygmy", Rarity.Blue, 2, Planet.Purple),
                    new Fish("Arceel", Rarity.Blue, 15, Planet.Purple),
                    new Fish("Arc Pincher", Rarity.Blue, 5, Planet.Purple),

                    //Purples\\
                    new Fish("Amber Jelly", Rarity.Purple, 4, Planet.Orange),
                    new Fish("Sawfish", Rarity.Purple, 450, Planet.Orange),
                    new Fish("Space Horse", Rarity.Purple, 1, Planet.Yellow),
                    new Fish("Ribbon Fighter", Rarity.Purple, 100, Planet.Yellow),
                    new Fish("Dark Inkster", Rarity.Purple, 175, Planet.Blue),
                    new Fish("Azure Crab", Rarity.Purple, 16, Planet.Blue),
                    new Fish("Cosmic Jelly", Rarity.Purple, 25, Planet.Red),
                    new Fish("Magma Eel", Rarity.Purple, 14, Planet.Red),
                    new Fish("Shimmerscale", Rarity.Purple, 5, Planet.Grey),
                    new Fish("Dusk Terrapin", Rarity.Purple, 120, Planet.Grey),
                    new Fish("Bleach Belly", Rarity.Purple, 23, Planet.Red),
                    new Fish("Ember", Rarity.Purple, 4, Planet.Red),
                    new Fish("Bob", Rarity.Purple, 52, Planet.Yellow),
                    new Fish("Sand Nautilus", Rarity.Purple, 14, Planet.Yellow),
                    new Fish("Cinereal Eel", Rarity.Purple, 7, Planet.Grey),
                    new Fish("Gremo", Rarity.Purple, 12, Planet.Grey),
                    new Fish("Plunge Pincer", Rarity.Purple, 6, Planet.Orange),
                    new Fish("Rock Tortle", Rarity.Purple, 21, Planet.Orange),
                    new Fish("Speckled Spacestar", Rarity.Purple, 16, Planet.Blue),
                    new Fish("Icepick Dasher", Rarity.Purple, 34, Planet.Blue),
                    new Fish("Watt Worm", Rarity.Purple, 3, Planet.Purple),
                    new Fish("City Shark", Rarity.Purple, 600, Planet.Purple),
                    new Fish("Periwinkle Whale", Rarity.Purple, 10, Planet.Purple),
                    new Fish("Charged Whisker", Rarity.Purple, 18, Planet.Purple),
                    
                    //Oranges\\
                    new Fish("Muffer Fish", Rarity.Orange, 35, Planet.Orange),
                    new Fish("Star Lion", Rarity.Orange, 20, Planet.Yellow),
                    new Fish("Cerulean Ray", Rarity.Orange, 1200, Planet.Blue),
                    new Fish("Heavenly Scarlett Squid", Rarity.Orange, 300, Planet.Red),
                    new Fish("Intergalactic Ripper", Rarity.Orange, 500, Planet.Grey),
                    new Fish("OEΓ-A", Rarity.Orange, 7, Planet.Purple),
                
                    //Reds\\
                    new Fish("Octoking", Rarity.Red, 4600, Planet.Ocean),
                    new Fish("Rainbow Darter", Rarity.Red, 35, Planet.Ocean),
                    new Fish("Mecha Snapper", Rarity.Red, 150, Planet.Purple)

            };

            // Intitialize multipliers
            for (int i = 0; i < Fishes.Count; i++)
            {
                switch (Fishes[i].rarity)
                {
                    case Rarity.Green:
                        Fishes[i].Multipliers = GreenMulti;
                        break;
                    case Rarity.Blue:
                        Fishes[i].Multipliers = BlueMulti;
                        break;
                    case Rarity.Purple:
                        Fishes[i].Multipliers = PurpleMulti;
                        break;
                    case Rarity.Orange:
                        Fishes[i].Multipliers = OrangeMulti;
                        break;
                    case Rarity.Red:
                        Fishes[i].Multipliers = RedMulti;
                        break;
                }
            }

            return Fishes;
        }
    }
}
