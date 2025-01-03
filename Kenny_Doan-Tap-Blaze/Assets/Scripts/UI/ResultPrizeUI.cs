using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPrizeUI : MonoBehaviour
{
    public Animator animator;
    public Image IconImage;
    public TextMeshProUGUI PrizeName;
    public void ShowResult(string prizeName, Sprite icon)
    {
        IconImage.sprite = icon;
        PrizeName.text = prizeName;
        animator.SetTrigger("Show Result");
    }
}
