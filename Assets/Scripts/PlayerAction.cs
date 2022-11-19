using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
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
        originCamZ = camera.transform.position.z;
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
        // �� ���� X
        if (area.detectBird == null)
        {
            Debug.Log("���� ���� ����");
        }
        // �� ����
        else
        {
            Debug.Log("�� ���� ���"); 
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
            camera.transform.position += zoomAmount;
        // ó�� ī�޶� ��ġ���� �ڷ� ���� �ʵ���
        else if (camera.transform.position.z > originCamZ)  // ���
            camera.transform.position -= zoomAmount;
    }
}