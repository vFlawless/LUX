using Google.Cloud.Firestore;

namespace LUX
{
    [FirestoreData]
    public class Payload
    {
        [FirestoreProperty]
        public int amount { get; set; }

        [FirestoreProperty]
        public float avrgMultiplier { get; set; }

        [FirestoreProperty]
        public int amountGreen { get; set; }

        [FirestoreProperty]
        public int amountBlue { get; set; }

        [FirestoreProperty]
        public int amountPurple { get; set; }

        [FirestoreProperty]
        public int amountOrange { get; set; }

        [FirestoreProperty]
        public int amountRed { get; set; }

        [FirestoreProperty]
        public float highestMultiplier { get; set; }

        [FirestoreProperty]
        public string planet { get; set; }
        
        [FirestoreProperty]
        public string URLSubstring { get; set; }

    }
    

    [FirestoreData]
    public class Payload2
    {
        [FirestoreProperty]
        public List<string> Domains { get; set; }

    }

}
