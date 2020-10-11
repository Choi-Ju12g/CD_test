using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    public RawImage display;
    WebCamTexture camTexture;
    private int currentIndex = 0;

    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No Camera detected");
            camAvailable = false;
            return;
        }

        backCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        /*
        for(int i = 0; i<devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        
        if(backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }*/

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;


    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(700,240,120,30),"Take & Save"))
        {
            TakeSnapShot();
        }
    }
    
    private string _SavePath = @"D:\unity\Age-seven";
    int _CaptureCounter = 0;

    void TakeSnapShot()
    {
        Texture2D snap = new Texture2D(backCam.width, backCam.height);
        snap.SetPixels(backCam.GetPixels());
        snap.Apply();

        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        ++_CaptureCounter;

    }

// Update is called once per frame
void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        //background.rectTransform.localScale = new Vector3(backCam.width, scaleY*backCam.height,0f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }
}
