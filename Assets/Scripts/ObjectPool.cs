using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject poolingObjectPrefab;    // 프리팹

    private Queue<GameObject> poolingObjectQueue = new Queue<GameObject>(); // 미리 생성한 오브젝트를 저장할 큐

    private void Awake()
    {
        Instance = this;

        Initialize(10); // 10개의 오브젝트 미리 생성
    }

    // 오브젝트들 미리 생성
    private void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());  // initCount개의 오브젝트를 생성하여 인큐
        }
    }

    // 프리팹 복제하여 생성한 오브젝트 생성 및 비활성화하여 리턴
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab);  // 복제
        newObj.gameObject.SetActive(false); // 비활성화
        newObj.transform.SetParent(transform);  // gameObject의 자식 오브젝트로 배치
        return newObj;
    }

    // 다른 스크립트에서 게임 오브젝트 요청하면 리턴(디큐 or 새로 생성)
    public static GameObject GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }    
    }

    // 다른 스크립트에서 오브젝트 반납
    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
