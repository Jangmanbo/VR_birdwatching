using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CFirebase : MonoBehaviour
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

    // 나의 랭킹 포인트 업데이트
    public async Task UpdateRankAsync(int point)
    {
        string userName = GetSerialNumber();

        Task<int> task = GetUserPointAsync(userName);
        point += await task;

        DocumentReference docRef = db.Collection("Rank").Document(userName);
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            {"point", point }
        };
        await docRef.SetAsync(dict, SetOptions.MergeAll);

        Debug.Log(userName+ " : " + point);
    }

    // 해당 user의 랭킹 포인트 가져오기
    private async Task<int> GetUserPointAsync(string userName)
    {
        DocumentReference docRef = db.Collection("Rank").Document(userName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Dictionary<string, object> user = snapshot.ToDictionary();
            Debug.Log(userName + "point : " + string.Format("{0}", user["point"]));
            return int.Parse(string.Format("{0}", user["point"]));
        }
        else
            return 0;
    }

    // 유저 이름 및 랭크포인트 리턴
    public async Task<List<RankInfo>> GetRankingAsync()
    {
        List<RankInfo> ranks = new List<RankInfo>();

        Query query = db.Collection("Rank").OrderByDescending("point");
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Debug.Log($"Document data for {document.Id} document:");
            Dictionary<string, object> user = document.ToDictionary();
            foreach (KeyValuePair<string, object> pair in user)
            {
                ranks.Add(new RankInfo(document.Id, int.Parse(string.Format("{0}", pair.Value))));
                //Debug.Log($"{pair.Key}: {pair.Value}");
            }
        }

        return ranks;
    }

    // 디바이스 시리얼넘버 리턴
    private string GetSerialNumber()
    {
        string identifier = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("SerialNumber : "+ identifier);
        return identifier;
    }
}
