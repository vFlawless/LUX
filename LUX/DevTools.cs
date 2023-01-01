using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUX
{
    public class DevTools
    {
       
        public static async void ResetData(FirestoreDb db)
        {
            // Function that will fix the uploaded data
            DocumentReference doc = db.Collection("URLS").Document("Domain"); // Collection with the names of the other collections (bases for leaderboards)
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
            int amountGreen = 0;
            int amountBlue = 0;
            int amountPurple = 0;
            int amountOrange = 0;
            int amountRed = 0;
            float highest = 0;
            float avrg = 0;
            int amount = 0;
            string planet = "";
            string URLSub = "";

            Payload2 pl = snapshot.ConvertTo<Payload2>();
            for (int i = 0; i < pl.Domains.Count; i++)     // For every collection (domain of URL)
            {
                Query allURL = db.Collection(pl.Domains[i]);
                QuerySnapshot allURLs = await allURL.GetSnapshotAsync();

                for (int j = 0; j < allURLs.Count; j++) // For every URL in collection
                {
                    string ID = allURLs[j].Id;
                    Payload URL = allURLs[j].ConvertTo<Payload>();
                    planet = URL.planet;
                    URLSub = URL.URLSubstring;

                    Payload payload3 = new Payload
                    {
                        amount = amount,
                        amountGreen = amountGreen,
                        amountBlue = amountBlue,
                        amountPurple = amountPurple,
                        amountOrange = amountOrange,
                        amountRed = amountRed,
                        avrgMultiplier = avrg,
                        highestMultiplier = highest,
                        planet = planet,
                        URLSubstring = URLSub
                    };
                    DocumentReference docRef = db.Collection(pl.Domains[i]).Document(ID);
                    await docRef.SetAsync(payload3);
                }
            }
        }
    }
}
