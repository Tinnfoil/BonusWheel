using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SafeAreaManager : MonoBehaviour
{
    public Canvas TargetCanvas;
    RectTransform SafeAreaTransform;

    ScreenOrientation orientation = ScreenOrientation.AutoRotation;
    Rect currentSafeArea = new Rect();

    public Action OnSafeAreaChanged;

    // Start is called before the first frame update
    void Start()
    {
        SafeAreaTransform = GetComponent<RectTransform>();

        orientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;

        SetSafeArea();
    }

    public void SetSafeArea()
    {
        if (SafeAreaTransform == null)
        {
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= TargetCanvas.pixelRect.width;
        anchorMin.y /= TargetCanvas.pixelRect.height;

        anchorMax.x = (Screen.currentResolution.width * safeArea.width / Screen.currentResolution.width) / anchorMax.x;
        float ratio = anchorMax.y / Screen.currentResolution.height;
        //anchorMax.y = (Screen.currentResolution.height * safeArea.height / Screen.currentResolution.height) / anchorMax.y;
        anchorMax.y = ratio;

        SafeAreaTransform.anchorMin = anchorMin;
        SafeAreaTransform.anchorMax = anchorMax;

        orientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;

        OnSafeAreaChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if ((orientation != Screen.orientation) || (currentSafeArea != Screen.safeArea))
        {
            SetSafeArea();
        }
    }
}
