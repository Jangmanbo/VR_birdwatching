using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bird
{
    private string species;
    public string Species
    {
        get { return species; }
        set { species = value; }
    }
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    private string habitat;
    public string Habitat
    {
        get { return habitat; }
        set { habitat = value; }
    }
    private string population;
    public string Population
    {
        get { return population; }
        set { population = value; }
    }
    private string feature;
    public string Feature
    {
        get { return feature; }
        set { feature = value; }
    }
    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public Bird(string[] row)
    {
        species = row[0];
        name = row[1];
        habitat = row[2];
        population = row[3];
        feature = row[4];
        score = System.Int32.Parse(row[5]);
    }

    public Bird(string _species, string _name, string _habitat, string _population, string _feature, int _score)
    {
        species = _species;
        name = _name;
        habitat = _habitat;
        population = _population;
        feature = _feature;
        score = _score;
    }
}

public class BirdDataParse : MonoBehaviour
{
    private static List<Bird> birdList = new List<Bird>();
    public static List<float> probability = new List<float>() { 0 }; // 누적 확률

    [SerializeField] private TextAsset csvFile = null;

    private void Awake()
    {
        SetBirdDictionary();
    }

    public static int BirdCount()
    {
        return birdList.Count;
    }

    public static Bird GetBirdData(int id)
    {
        return birdList[id];
    }

    public void SetBirdDictionary()
    {
        // 아래 한 줄 빼기
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1);
        // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음
        string[] rows = csvText.Split(new char[] { '\n' });

        // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
        for (int i = 1; i < rows.Length; i++)
        {
            // A, B, C, D, E열을 쪼개서 배열에 담음
            string[] rowValues = rows[i].Split(new char[] { ',' });

            // 파일의 끝이면 반복문 종료
            if (rowValues[0].Trim() == "") break;

            Bird bird = new Bird(rowValues);
            birdList.Add(bird); // birdList에 추가
            
            float prob = float.Parse(rowValues[6]);  // 현재 확률
            prob += probability[i - 1]; // + 이전 확률 -> 누적 확률
            probability.Add(prob);  // 누적 확률 리스트에 추가
            Debug.Log(prob);
        }
    }
}
