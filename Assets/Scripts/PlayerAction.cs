using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private float originCamZ;   // ó�� ī�޶� ��ġ
    private Vector3 zoomAmount = new Vector3(0, 0, 0.1f);   // fps�� ī�޶� �̵� ����

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
        if (OVRInput.GetDown(OVRInput.Button.One))  // ��ư A
            TryTakePicture();
        if (OVRInput.GetDown(OVRInput.Button.Two))  // ��ư B
            screenShot.ReadScreenShotAndShow();
        if (OVRInput.GetDown(OVRInput.Button.Four)) // ��ư Y
        {
            // �޴� ��Ȱ��ȭ
            if (clipBoard.activeSelf)
                clipBoard.SetActive(false);
            // �޴� Ȱ��ȭ
            else
            {
                clipBoard.transform.position = centerEyeAnchor.transform.position + gameObject.transform.forward * 2;   // ��ġ : �÷��̾ ���� �ٶ󺸴� ����
                clipBoard.transform.rotation = Quaternion.Euler(new Vector3(clipBoard.transform.rotation.eulerAngles.x, centerEyeAnchor.transform.rotation.eulerAngles.y + 180, clipBoard.transform.rotation.eulerAngles.z));   // ȸ���� : �÷��̾ ���ֺ�����
                clipBoard.SetActive(true);
            }
        }

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
            int ID = area.detectBird.GetComponent<BirdID>().ID;
            TakePicture(area.detectBird.GetComponent<BirdID>().ID);
            DataController.instance.data.picture[ID]++;
        }
    }

    // ���� �Կ�
    private void TakePicture(int ID)
    {
        screenShot.TakeScreenShot(ID);
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
