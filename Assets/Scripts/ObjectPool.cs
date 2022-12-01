using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject[] poolingObjectPrefab;    // 프리팹
    private Queue<GameObject>[] poolingObjectQueue; // 미리 생성한 오브젝트를 저장할 큐

    private void Start()
    {
        Instance = this;

        Initialize(10); // 10개의 오브젝트 미리 생성
    }

    // 오브젝트들 미리 생성
    private void Initialize(int initCount)
    {
        poolingObjectQueue=new Queue<GameObject>[BirdDataParse.BirdCount()];
        for(int id = 0; id < BirdDataParse.BirdCount(); id++)
        {
            poolingObjectQueue[id] = new Queue<GameObject>();
            for (int j = 0; j < initCount; j++)
            {
                poolingObjectQueue[id].Enqueue(CreateNewObject(id));  // initCount개의 오브젝트를 생성하여 인큐
            }
        }
    }

    // 프리팹 복제하여 생성한 오브젝트 생성 및 비활성화하여 리턴
    private GameObject CreateNewObject(int id)
    {
        var newObj = Instantiate(poolingObjectPrefab[id]);  // 복제
        newObj.gameObject.SetActive(false); // 비활성화
        newObj.transform.SetParent(transform);  // gameObject의 자식 오브젝트로 배치
        return newObj;
    }

    // 다른 스크립트에서 게임 오브젝트 요청하면 리턴(디큐 or 새로 생성)
    public static GameObject GetObject(int id)
    {
        if (Instance.poolingObjectQueue[id].Count > 0)
        {
            var obj = Instance.poolingObjectQueue[id].Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject(id);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }    
    }

    // 다른 스크립트에서 오브젝트 반납
    public static void ReturnObject(GameObject obj, int id)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue[id].Enqueue(obj);
    }
}
