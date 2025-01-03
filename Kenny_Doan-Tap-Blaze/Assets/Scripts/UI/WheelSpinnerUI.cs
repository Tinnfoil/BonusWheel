using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpinnerUI : MonoBehaviour
{
    public SpinnerElement SpinnerElementPrefab;
    public ResultPrizeUI resultPrizeUI;
    public List<SpinnerElement> spinnerElements;
    public RectTransform TargetGraphic;
    private float wedgeAngle = 360f / 8f;
    private int wedges = 8;

    public GameObject SpinButton;
    public GameObject ClaimButton;
    [Space]
    public TweenElement tweenElement;

    private void Start()
    {
    }

    /// <summary>
    /// Initialize the generate the data for the wheel spinner based on the public variables
    /// </summary>
    public void PopulateWheelSpinner(PrizeDatabase prizeDatabase)
    {
        // Clear the wheel of previous elements
        spinnerElements.Clear();
        for (int i = TargetGraphic.childCount - 1; i >= 0; i--)
        {
            Destroy(TargetGraphic.GetChild(i).gameObject);
        }

        // Calculate the amount of wedges on the wheel
        wedges = prizeDatabase.Prizes.Length;
        wedgeAngle = 360f / wedges;

        float currentChance = 0;
        // Generate the wedges and the elements in them based on the prize data
        for (int i = 0; i < prizeDatabase.Prizes.Length; i++)
        {
            SpinnerElement se = Instantiate(SpinnerElementPrefab.gameObject, TargetGraphic).GetComponent<SpinnerElement>();
            LootData lootData = prizeDatabase.Prizes[i];
            currentChance += lootData.ChanceToSpawn;
            se.InitializeSpinnerElement(lootData, currentChance);

            RectTransform rect = se.GetComponent<RectTransform>();
            rect.localEulerAngles = new Vector3(0, 0, (wedgeAngle * .5f) + wedgeAngle * i);
            spinnerElements.Add(se);
        }

        spinnerElements.Reverse();
    }


    public IEnumerator spinningCoroutine;


    /// <summary>
    /// Win the prize at the given index
    /// </summary>
    /// <param name="index"></param>
    public void WinPrizeAt(int index)
    {
        if (!gameObject.activeSelf || spinningCoroutine != null) // Dont spin if already spinning
            return;
        float targetAngle = Random.Range(index * wedgeAngle, (index + 1) * wedgeAngle);
        spinningCoroutine = SpinWheelCoroutine(targetAngle, 3, index);
        StartCoroutine(spinningCoroutine);
    }

    /// <summary>
    /// Animation and logic for spinning the wheel
    /// </summary>
    public IEnumerator SpinWheelCoroutine(float targetAngle, float duration, int winningIndex)
    {
        float elapsed = 0f;
        float startingAngle = TargetGraphic.localEulerAngles.z;

        // Initial Spin back for simulate using your hand
        while (elapsed < .2f)
        {
            float delta = Time.deltaTime;
            elapsed += delta;
            float t = Mathf.Clamp01(elapsed / duration);

            float easedT = 1f - Mathf.Pow(1f - t, 5f);

            float currentAngle = startingAngle + 15f * easedT;
            TargetGraphic.localEulerAngles = new Vector3(0, 0, currentAngle);

            yield return null;
        }

        yield return new WaitForSeconds(.12f);

        // Visual effect
        tweenElement.Pop();

        elapsed = 0f;
        startingAngle = TargetGraphic.localEulerAngles.z;

        targetAngle = Mathf.Repeat(targetAngle, 360f);
        startingAngle = Mathf.Repeat(startingAngle, 360f);

        // Ensure the rotation is always clockwise
        float angleDifference = targetAngle - startingAngle;
        if (angleDifference > 0)
            angleDifference -= 360f;

        // Add extra spins for a fast start
        float totalSpin = angleDifference - 1440f;

        while (elapsed < duration)
        {
            float delta = Time.deltaTime;
            elapsed += delta;

            float t = Mathf.Clamp01(elapsed / duration);

            // Use an ease
            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            float currentAngle = startingAngle + totalSpin * easedT;
            TargetGraphic.localEulerAngles = new Vector3(0, 0, currentAngle);

            yield return null;
        }

        // Ensure the final angle is at the target angle
        TargetGraphic.localEulerAngles = new Vector3(0, 0, targetAngle);

        ShowResult(winningIndex);

        spinningCoroutine = null;
    }

    public void ShowResult(int winningIndex)
    {
        gameObject.SetActive(false);
        resultPrizeUI.gameObject.SetActive(true);
        resultPrizeUI.ShowResult(spinnerElements[winningIndex].Name.text, spinnerElements[winningIndex].Icon.sprite);
        SpinButton.SetActive(false);
        ClaimButton.SetActive(true);
    }

    public void ClaimPrize()
    {
        gameObject.SetActive(true);
        resultPrizeUI.gameObject.SetActive(false);
        SpinButton.SetActive(true);
        ClaimButton.SetActive(false);
    }

}
