using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bird
{
    private string species;
    private string name;
    private string habitat;
    private string population;
    private string feature;

    public Bird(string[] row)
    {
        species = row[0];
        name = row[1];
        habitat = row[2];
        population = row[3];
        feature = row[4];
    }

    public Bird(string _species, string _name, string _habitat, string _population, string _feature)
    {
        species = _species;
        name = _name;
        habitat = _habitat;
        population = _population;
        feature = _feature;
    }
}

public class BirdDataParse : MonoBehaviour
{
    private static List<Bird> birdList = new List<Bird>();

    [SerializeField] private TextAsset csvFile = null;

    private void Awake()
    {
        SetBirdDictionary();
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
        }
    }
}
