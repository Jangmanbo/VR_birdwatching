using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    public GameObject detectBird = null;    // ���� ī�޶� ���� ������ �� ������Ʈ

    // ī�޶� ������ �� �ݶ��̴� �浹
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Bird"))
        {
            detectBird = other.gameObject;
        }
    }

    // ī�޶� ������ �� �ݶ��̴� �浹 ����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bird"))
        {
            detectBird = null;
        }
    }
}
