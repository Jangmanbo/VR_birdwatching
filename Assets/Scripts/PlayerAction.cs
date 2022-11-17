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

    private float originCamZ;   // 처음 카메라 위치
    private Vector3 zoomAmount = new Vector3(0, 0, 0.1f);   // fps당 카메라 이동 벡터

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

        // 카메라 확대/축소
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            CameraZoom(true);
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            CameraZoom(false);
    }

    // 카메라에 새가 감지되었는지 확인
    private void TryTakePicture()
    {
        // 새 감지 X
        if (area.detectBird == null)
        {
            Debug.Log("찍을 새가 없음");
        }
        // 새 감지
        else
        {
            Debug.Log("새 사진 찍기"); 
            TakePicture(area.detectBird.name);
        }
    }

    // 사진 촬영
    private void TakePicture(string birdName)
    {
        screenShot.TakeScreenShot(birdName);
    }

    // 카메라 확대/축소
    private void CameraZoom(bool zoom)
    {
        if (zoom)   // 확대
            camera.transform.position += zoomAmount;
        // 처음 카메라 위치보다 뒤로 가지 않도록
        else if (camera.transform.position.z > originCamZ)  // 축소
            camera.transform.position -= zoomAmount;
    }
}
