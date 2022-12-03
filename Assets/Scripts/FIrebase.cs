using Firebase.Firestore;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FIrebase : MonoBehaviour
{
    private FirebaseFirestore db;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        //UpdateRank();
        Task task = GetUserRankAsync();
        task.Start();
    }

    private void UpdateRank()
    {
        DocumentReference docRef = db.Collection("Rank").Document("user3");
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            {"point", 3 }
        };
        docRef.SetAsync(dict, SetOptions.MergeAll);
    }

    private async Task GetUserRankAsync()
    {
        Query query = db.Collection("Rank").WhereGreaterThanOrEqualTo("point", 2);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Debug.Log($"Document data for {document.Id} document:");
            Dictionary<string, object> user = document.ToDictionary();
            foreach (KeyValuePair<string, object> pair in user)
            {
                Debug.Log($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
