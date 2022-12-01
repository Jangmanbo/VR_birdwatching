using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject[] poolingObjectPrefab;    // ������
    private Queue<GameObject>[] poolingObjectQueue; // �̸� ������ ������Ʈ�� ������ ť

    private void Start()
    {
        Instance = this;

        Initialize(10); // 10���� ������Ʈ �̸� ����
    }

    // ������Ʈ�� �̸� ����
    private void Initialize(int initCount)
    {
        poolingObjectQueue=new Queue<GameObject>[BirdDataParse.BirdCount()];
        for(int id = 0; id < BirdDataParse.BirdCount(); id++)
        {
            poolingObjectQueue[id] = new Queue<GameObject>();
            for (int j = 0; j < initCount; j++)
            {
                poolingObjectQueue[id].Enqueue(CreateNewObject(id));  // initCount���� ������Ʈ�� �����Ͽ� ��ť
            }
        }
    }

    // ������ �����Ͽ� ������ ������Ʈ ���� �� ��Ȱ��ȭ�Ͽ� ����
    private GameObject CreateNewObject(int id)
    {
        var newObj = Instantiate(poolingObjectPrefab[id]);  // ����
        newObj.gameObject.SetActive(false); // ��Ȱ��ȭ
        newObj.transform.SetParent(transform);  // gameObject�� �ڽ� ������Ʈ�� ��ġ
        return newObj;
    }

    // �ٸ� ��ũ��Ʈ���� ���� ������Ʈ ��û�ϸ� ����(��ť or ���� ����)
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

    // �ٸ� ��ũ��Ʈ���� ������Ʈ �ݳ�
    public static void ReturnObject(GameObject obj, int id)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue[id].Enqueue(obj);
    }
}
