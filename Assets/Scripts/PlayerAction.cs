using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private BirdManager birdManager;
    [SerializeField]
    private ScreenShot screenShot;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private CameraArea area;

    private float originCamZ;   // ó�� ī�޶� ��ġ
    private Vector3 zoomAmount = new Vector3(0, 0, 0.1f);   // fps�� ī�޶� �̵� ����

    private void Awake()
    {
        originCamZ = camera.transform.localPosition.z + zoomAmount.z;
    }

    // Update is called once per frame
    void Update()
    {
        BtnDown();
    }

    private void BtnDown()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
            TryTakePicture();
        if (OVRInput.GetDown(OVRInput.Button.Two))
            screenShot.ReadScreenShotAndShow();

        // ī�޶� Ȯ��/���
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            CameraZoom(true);
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            CameraZoom(false);
    }

    // ī�޶� ���� �����Ǿ����� Ȯ��
    private void TryTakePicture()
    {
        Debug.Log("PlayerAction ���� ��� �õ�");
        // �� ���� X
        if (area.detectBird == null)
        {
            Debug.Log("PlayerAction ���� ���� ����");
        }
        // �� ����
        else
        {
            Debug.Log("PlayerAction ���� ��� ����"); 
            TakePicture(area.detectBird.name);
        }
    }

    // ���� �Կ�
    private void TakePicture(string birdName)
    {
        screenShot.TakeScreenShot(birdName);
    }

    // ī�޶� Ȯ��/���
    private void CameraZoom(bool zoom)
    {
        if (zoom)   // Ȯ��
            camera.transform.localPosition += zoomAmount;
        // ó�� ī�޶� ��ġ���� �ڷ� ���� �ʵ���
        else if (camera.transform.localPosition.z > originCamZ)  // ���
            camera.transform.localPosition -= zoomAmount;
    }

    // �÷��̾��� �ݶ��̴� �������� ���� ����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bird"))
        {
            Debug.Log(other.name);
            birdManager.DeleteBird(other.gameObject);
        }
    }
}
