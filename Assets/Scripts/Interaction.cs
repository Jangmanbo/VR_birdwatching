using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public ScreenShot screenShot;

    // Update is called once per frame
    void Update()
    {
        BtnDown();
    }

    private void BtnDown()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            screenShot.TakeScreenShot();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            screenShot.ReadScreenShotAndShow();
        }
    }
}
