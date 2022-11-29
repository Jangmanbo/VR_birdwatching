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

            Bird bird = new Bird(rowValues);
            birdList.Add(bird); // birdList�� �߰�
        }
    }
}
