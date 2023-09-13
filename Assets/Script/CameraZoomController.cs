using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class CameraZoomController : MonoBehaviour
{
    //private enum AngleLevel
    //{
    //    wideAngle,
    //    fullBody,
    //    halfBody,
    //    closeUp,
    //    extremeCloseUp
    //}

    [SerializeField] Slider sliderZoom, sliderTilt, sliderPanning;
    [SerializeField] Button checkSlider;
    [SerializeField] TextMeshProUGUI textAngleName;
    [SerializeField] Camera camera;
    [SerializeField] GameObject uiComponent;

    public Transform cameraTransform;

    private float minHeight = 1f;
    private float maxHeight = 10f;

    private float minXPos = -1f;
    private float maxXPos = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //AngleLevel angleLevel;
        //angleLevel = AngleLevel.wideAngle;

        float currentHeight = cameraTransform.position.y;
        sliderTilt.value = Mathf.InverseLerp(minHeight, maxHeight, currentHeight);

        float initialValue = (minXPos + maxXPos) / 2f;
        sliderPanning.value = initialValue;

        sliderTilt.onValueChanged.AddListener(UpdateCameraHeight);
        sliderPanning.onValueChanged.AddListener(UpdateCameraXPosition);
        checkSlider.onClick.AddListener(CheckZoom);
    }

    // Update is called once per frame
    void Update()
    {
        camera.fieldOfView = sliderZoom.value;
    }

    private void UpdateCameraHeight(float sliderValue)
    {
        float newHeight = Mathf.Lerp(minHeight, maxHeight, sliderValue);
        Vector3 CameraPosition = cameraTransform.position;
        CameraPosition.y = newHeight;
        cameraTransform.position = CameraPosition;
    }

    private void UpdateCameraXPosition(float sliderValue)
    {
        float newXPos = Mathf.Lerp(minXPos, maxXPos, sliderValue);
        Vector3 cameraPos = cameraTransform.position;
        cameraPos.x = newXPos;
        cameraTransform.position = cameraPos;
    }

    public void CheckZoom()
    {
        print("Clicked");

        if (sliderZoom.value >= 50 && sliderZoom.value <= 80)
        {
            textAngleName.text = "Extreme Long Shot";
            uiComponent.SetActive(false);
            StartCoroutine(takeScreenShot());
        }
        else if(sliderZoom.value >= 27 && sliderZoom.value <= 30)
        {
            textAngleName.text = "Long Shot";
            uiComponent.SetActive(false);
            StartCoroutine(takeScreenShot());
        }
        else if(sliderZoom.value >= 15 && sliderZoom.value <=18)
        {
            textAngleName.text = "Medium Shot";
            uiComponent.SetActive(false);
            StartCoroutine(takeScreenShot());
        }
        else if(sliderZoom.value >= 10 && sliderZoom.value <= 12)
        {
            textAngleName.text = "Close Up Shot";
            uiComponent.SetActive(false);
            StartCoroutine(takeScreenShot());
        }
        else if(sliderZoom.value >= 3 && sliderZoom.value <= 9)
        {
            textAngleName.text = "Extreme Shot";
            uiComponent.SetActive(false);
            StartCoroutine(takeScreenShot());
        }
        else
        {
            textAngleName.text = "Empty";
        }

        StartCoroutine(redoUIActive());
    }

    IEnumerator takeScreenShot()
    {
        yield return new WaitForEndOfFrame();

        if (!Directory.Exists("Assets/Screenshot"))
        {
            Directory.CreateDirectory("Assets/Screenshot");
        }

        ScreenCapture.CaptureScreenshot("Screenshot" + System.DateTime.Now.ToString("MM-dd-yyyy") + " .png");
    }

    IEnumerator redoUIActive()
    {
        yield return new WaitForSeconds(2.5f);

        uiComponent.SetActive(true);
    }
}
