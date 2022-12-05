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
        rangeCollider = rangeObject.GetComponent<BoxCollider>();    // 새 스폰 영역
        SpawnBirds(minSpawn, maxSpawn); // 스폰
    }

    // 최소 min, 최대 max마리의 새를 스폰
    public void SpawnBirds(int min, int max)
    {
        System.Random rand = new System.Random();
        int count = rand.Next(min, max);
        for (int i = 0; i < count; i++)
            Spawn();
    }

    // 새 오브젝트 생성하여 배치
    private void Spawn()
    {
        var obj = ObjectPool.GetObject(SelectBird());
        obj.transform.position = SpawnPosition();
        obj.transform.parent = gameObject.transform;
    }

    // 어떤 종을 생성할지 결정
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

    // PlayerAction에서 호출
    // 씬에서 새 오브젝트 비활성화 + 0~2마리의 새 오브젝트 스폰
    public void DeleteBird(GameObject bird)
    {
        int id = bird.GetComponent<BirdData>().birdInfo.ID;
        ObjectPool.ReturnObject(bird, id);
        SpawnBirds(0, 3);
    }

    // 주어진 콜라이더 내에서 랜덤 좌표 생성
    private Vector3 SpawnPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;

        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 5f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
