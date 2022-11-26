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
        // �Ʒ� �� �� ����
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1);
        // �ٹٲ�(�� ��)�� �������� csv ������ �ɰ��� string�迭�� �� ������� ����
        string[] rows = csvText.Split(new char[] { '\n' });

        // ���� ���� 1��° ���� ���Ǹ� ���� �з��̹Ƿ� i = 1���� ����
        for (int i = 1; i < rows.Length; i++)
        {
            // A, B, C, D, E���� �ɰ��� �迭�� ����
            string[] rowValues = rows[i].Split(new char[] { ',' });

            // ������ ���̸� �ݺ��� ����
            if (rowValues[0].Trim() == "") break;

            string name = rowValues[0]; // dictionary�� key
            Bird bird = new Bird(rowValues[1], rowValues[2], rowValues[3], rowValues[4]);   // ��, ������, ��ü��, Ư¡

            BirdDictionary.Add(name, bird); // BirdDictionary�� �߰�

            Debug.Log(name+rowValues[1]+rowValues[2]+rowValues[3]+rowValues[4]);
        }
    }
}
