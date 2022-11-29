using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private float originCamZ;   // 처음 카메라 위치
    private Vector3 zoomAmount = new Vector3(0, 0, 0.1f);   // fps당 카메라 이동 벡터

    [SerializeField] private BirdManager birdManager;
    [SerializeField] private GameObject clipBoard;
    [SerializeField] private GameObject centerEyeAnchor;

    [SerializeField] private ScreenShot screenShot;
    [SerializeField] private GameObject camera;
    [SerializeField] private CameraArea area;

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
        if (OVRInput.GetDown(OVRInput.Button.One))  // 버튼 A
            TryTakePicture();
        if (OVRInput.GetDown(OVRInput.Button.Two))  // 버튼 B
            screenShot.ReadScreenShotAndShow();
        if (OVRInput.GetDown(OVRInput.Button.Four)) // 버튼 Y
        {
            // 메뉴 비활성화
            if (clipBoard.activeSelf)
                clipBoard.SetActive(false);
            // 메뉴 활성화
            else
            {
                clipBoard.transform.position = centerEyeAnchor.transform.position + gameObject.transform.forward * 2;   // 위치 : 플레이어가 현재 바라보는 방향
                clipBoard.transform.rotation = Quaternion.Euler(new Vector3(clipBoard.transform.rotation.eulerAngles.x, centerEyeAnchor.transform.rotation.eulerAngles.y + 180, clipBoard.transform.rotation.eulerAngles.z));   // 회전값 : 플레이어가 마주보도록
                clipBoard.SetActive(true);
            }
        }

        // 카메라 확대/축소
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            CameraZoom(true);
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            CameraZoom(false);
    }

    // 카메라에 새가 감지되었는지 확인
    private void TryTakePicture()
    {
        Debug.Log("PlayerAction 사진 찍기 시도");
        // 새 감지 X
        if (area.detectBird == null)
        {
            Debug.Log("PlayerAction 찍을 새가 없음");
        }
        // 새 감지
        else
        {
            Debug.Log("PlayerAction 사진 찍기 성공");
            int ID = area.detectBird.GetComponent<BirdID>().ID;
            TakePicture(area.detectBird.GetComponent<BirdID>().ID);
            DataController.instance.data.picture[ID]++;
        }
    }

    // 사진 촬영
    private void TakePicture(int ID)
    {
        screenShot.TakeScreenShot(ID);
    }

    // 카메라 확대/축소
    private void CameraZoom(bool zoom)
    {
        if (zoom)   // 확대
            camera.transform.localPosition += zoomAmount;
        // 처음 카메라 위치보다 뒤로 가지 않도록
        else if (camera.transform.localPosition.z > originCamZ)  // 축소
            camera.transform.localPosition -= zoomAmount;
    }

    // 플레이어의 콜라이더 영역에서 새가 나감
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bird"))
        {
            Debug.Log(other.name);
            birdManager.DeleteBird(other.gameObject);
        }
    }
}
