using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    public GameObject detectBird = null;    // 현재 카메라 내에 감지된 새 오브젝트

    // 카메라 영역과 새 콜라이더 충돌
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Bird"))
        {
            detectBird = other.gameObject;
        }
    }

    // 카메라 영역과 새 콜라이더 충돌 해제
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bird"))
        {
            detectBird = null;
        }
    }
}
