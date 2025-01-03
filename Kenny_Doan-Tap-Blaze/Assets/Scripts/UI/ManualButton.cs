using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManualButton : MonoBehaviour
{
    public WheelSpinner spinner;
    public GameObject Container;
    public TextMeshProUGUI ManualText;
    
    public void ToggleContainer()
    {
        Container.SetActive(!Container.activeSelf);
        ManualText.text = Container.activeSelf ? "Manual\n^" : "Manual\nv";
    }

    public void SetPrizeIndex(int index)
    {
        spinner.wheelUI.WinPrizeAt(index);
    }
}
