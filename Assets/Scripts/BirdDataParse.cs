using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bird
{
    private string species;
    private string habitat;
    private string population;
    private string feature;

    public Bird(string _species, string _habitat, string _population, string _feature)
    {
        species = _species;
        habitat = _habitat;
        population = _population;
        feature = _feature;
    }
}

public class BirdDataParse : MonoBehaviour
{
    private static Dictionary<string, Bird> BirdDictionary = new Dictionary<string, Bird>();

    [SerializeField] private TextAsset csvFile = null;

    private void Awake()
    {
        SetBirdDictionary();
    }

    public static Bird GetBirdData(string name)
    {
        return BirdDictionary[name];
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

            string name = rowValues[0]; // dictionary의 key
            Bird bird = new Bird(rowValues[1], rowValues[2], rowValues[3], rowValues[4]);   // 종, 서식지, 개체수, 특징

            BirdDictionary.Add(name, bird); // BirdDictionary에 추가

            Debug.Log(name+rowValues[1]+rowValues[2]+rowValues[3]+rowValues[4]);
        }
    }
}
