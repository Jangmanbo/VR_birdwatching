using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [Range(10, 20)]
    [SerializeField] private int minSpawn, maxSpawn;
    [SerializeField] private GameObject rangeObject;
    private BoxCollider rangeCollider;

    // Start is called before the first frame update
    void Start()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();    // �� ���� ����
        SpawnBirds(minSpawn, maxSpawn); // ����
    }

    // �ּ� min, �ִ� max������ ���� ����
    public void SpawnBirds(int min, int max)
    {
        System.Random rand = new System.Random();
        int count = rand.Next(min, max);
        for (int i = 0; i < count; i++)
            Spawn();
    }

    // �� ������Ʈ �����Ͽ� ��ġ
    private void Spawn()
    {
        var obj = ObjectPool.GetObject(SelectBird());
        obj.transform.position = SpawnPosition();
        obj.transform.parent = gameObject.transform;
    }

    // � ���� �������� ����
    private int SelectBird()
    {
        float rand = Random.Range(0f, 1f);
        for (int id = 0; id < BirdDataParse.BirdCount(); id++)
        {
            if (rand < BirdDataParse.probability[id + 1])
                return id;
        }
        return 0;
    }

    // PlayerAction���� ȣ��
    // ������ �� ������Ʈ ��Ȱ��ȭ + 0~2������ �� ������Ʈ ����
    public void DeleteBird(GameObject bird)
    {
        int id = bird.GetComponent<BirdData>().birdInfo.ID;
        ObjectPool.ReturnObject(bird, id);
        SpawnBirds(0, 3);
    }

    // �־��� �ݶ��̴� ������ ���� ��ǥ ����
    private Vector3 SpawnPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;

        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 5f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
