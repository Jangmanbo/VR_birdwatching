using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private ScreenShot screenShot;
    [SerializeField]
    private CameraArea area;

    // Update is called once per frame
    void Update()
    {
        BtnDown();
    }

    private void BtnDown()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            TryTakePicture();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            screenShot.ReadScreenShotAndShow();
        }
    }

    private void TryTakePicture()
    {
        if (area.detectBird == null)
        {
            Debug.Log("���� ���� ����");
        }
        else
        {
            Debug.Log("�� ���� ���"); 
            TakePicture(area.detectBird.name);
        }
    }

    private void TakePicture(string birdName)
    {
        screenShot.TakeScreenShot(birdName);
    }
}
