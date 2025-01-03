using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceUnder : MonoBehaviour
{
    public RectTransform targetRectTransform;
    public Image targetImage;
    public SafeAreaManager safeAreaManager;
    public bool PlaceTop = false;
    RectTransform rectTransform;
    Rect CurrentTargetRect = new Rect();
    private void Awake()
    {
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void HandleSafeAreaChanged()
    {
        //targetRectTransform.anchorMin = new Vector2(targetRectTransform.anchorMin.y, targetRectTransform.anchorMin.x);
        //targetRectTransform.anchorMax = new Vector2(targetRectTransform.anchorMax.y, targetRectTransform.anchorMax.x);
    }


    private void OnDestroy()
    {
        //if (safeAreaManager) { safeAreaManager.OnSafeAreaChanged -= HandleSafeAreaChanged; }
    }

    private void Update()
    {
        if (CurrentTargetRect != targetRectTransform.rect)
        {
            UpdatePosition();
        }
    }

    public void UpdatePosition()
    {
        CurrentTargetRect.position = new Vector3(0, -50, 0);
        CurrentTargetRect = targetRectTransform.rect;

        float size = 0;
        if ((float)targetImage.sprite.texture.width / targetRectTransform.rect.width <
            (float)targetImage.sprite.texture.height / targetRectTransform.rect.height)
        {
            size = (targetImage.sprite.texture.width * targetRectTransform.rect.height / (float)targetImage.sprite.texture.height);
        }
        else
        {
            size = targetRectTransform.rect.width;
        }
        float y = size * targetImage.pixelsPerUnit * targetRectTransform.localScale.y;

        //rectTransform.rect.Set(0, -y / 2, CurrentTargetRect.width, CurrentTargetRect.height);
        rectTransform.anchoredPosition = new Vector2(0, (PlaceTop ? -1 : 1) * (-y / 2));
    }
}
