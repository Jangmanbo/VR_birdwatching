using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FIrebase : MonoBehaviour
{
    private FirebaseFirestore db;
    public bool available;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        // Google Play 버전 확인
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                available = true;
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
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

    // 유저 이름 및 랭크포인트 출력 스레드
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

    // 디바이스 시리얼넘버 리턴
    private string GetSerialNumber()
    {
        string identifier = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("SerialNumber : "+ identifier);
        return identifier;
    }
}
