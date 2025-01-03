using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SpinnerElement : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Image Icon;
    public LootData lootData;
    public float weight;


    public void InitializeSpinnerElement(LootData lootData, float weight)
    {
        this.lootData = lootData;
        Name.text = (lootData.PrizeData.TimePrize ? "" : "x") + lootData.PrizeData.Amount + (lootData.PrizeData.TimePrize ? " Min" : "");
        Icon.sprite = lootData.PrizeData.Icon;
        this.weight = weight;
    }
    private void OnEnable()
    {
        //this.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
        //Color backgroundColor = background.color;
        //backgroundColor.a = 0.5f;
        //background.color = backgroundColor;
    }

    public SpinnerElement Select()
    {
        gameObject.transform.DOScale(new Vector3(0.7f, 0.7f, 1.0f), .1f);
        //LeanTween.scale(gameObject, new Vector3(0.7f, 0.7f, 1.0f), 0.1f);
        //background.DOFade(0.8f, .1f);
        //LeanTween.alpha(background.rectTransform, 0.8f, 0.1f);
        return this;
    }

    public SpinnerElement Deselect()
    {
        gameObject.transform.DOScale(new Vector3(0.6f, 0.6f, 1.0f), .1f);
        //LeanTween.scale(gameObject, new Vector3(0.6f, 0.6f, 1.0f), 0.1f);
        //background.DOFade(0.5f, .1f);
        //LeanTween.alpha(background.rectTransform, 0.5f, 0.1f);
        return this;
    }
}
