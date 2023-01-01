using Google.Cloud.Firestore;

namespace LUX
{
    [FirestoreData]
    public class Payload
    {
        [FirestoreProperty]
        public List<float> GreenMultipliers { get; set; }
        
        [FirestoreProperty]
        public List<float> BlueMultipliers { get; set; }
        
        [FirestoreProperty]
        public List<float> PurpleMultipliers { get; set; }
        
        [FirestoreProperty]
        public List<float> OrangeMultipliers { get; set; }
        
        [FirestoreProperty]
        public List<float> RedMultipliers { get; set; }
        
        [FirestoreProperty]
        public float HighestMultiplier { get; set; }
        
        [FirestoreProperty]
        public string URLSubstring { get; set; }

        [FirestoreProperty]
        public string Planet { get; set; }
    }

    [FirestoreData]
    public class Payload2
    {
        [FirestoreProperty]
        public List<string> Domains { get; set; }

    }
}
