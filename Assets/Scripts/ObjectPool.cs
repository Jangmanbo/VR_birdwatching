using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject poolingObjectPrefab;    // ������

    private Queue<GameObject> poolingObjectQueue = new Queue<GameObject>(); // �̸� ������ ������Ʈ�� ������ ť

    private void Awake()
    {
        Instance = this;

        Initialize(10); // 10���� ������Ʈ �̸� ����
    }

    // ������Ʈ�� �̸� ����
    private void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());  // initCount���� ������Ʈ�� �����Ͽ� ��ť
        }
    }

    // ������ �����Ͽ� ������ ������Ʈ ���� �� ��Ȱ��ȭ�Ͽ� ����
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab);  // ����
        newObj.gameObject.SetActive(false); // ��Ȱ��ȭ
        newObj.transform.SetParent(transform);  // gameObject�� �ڽ� ������Ʈ�� ��ġ
        return newObj;
    }

    // �ٸ� ��ũ��Ʈ���� ���� ������Ʈ ��û�ϸ� ����(��ť or ���� ����)
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

    // �ٸ� ��ũ��Ʈ���� ������Ʈ �ݳ�
    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
