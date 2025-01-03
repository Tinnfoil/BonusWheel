using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TweenElement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bobble()
    {
        transform.DOScale(.85f, 0).SetEase(Ease.OutQuad).OnComplete(() => { transform.DOScale(1f, .1f).SetEase(Ease.OutQuad); });
    }

    public void Pop()
    {
        transform.DOScale(1.15f, .1f).SetEase(Ease.OutQuad).OnComplete(() => { transform.DOScale(1f, .5f).SetEase(Ease.OutQuad); });
    }
}
