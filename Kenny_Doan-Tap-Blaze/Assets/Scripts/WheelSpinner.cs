using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Linq;

public class WheelSpinner : MonoBehaviour
{
    public PrizeDatabase prizeDatabase;
    public SpinnerElement SpinnerElementPrefab;
    public ResultPrizeUI resultPrizeUI;
    public List<SpinnerElement> spinnerElements;
    public float[] weights;
    public RectTransform TargetGraphic;
    private float wedgeAngle = 360f / 8f;
    private int wedges = 8;

    public GameObject SpinButton;
    public GameObject ClaimButton;

    [Space]
    public TweenElement tweenElement;

    private void Start()
    {
        PopulateWheelSpinner();
    }

    /// <summary>
    /// Initialize the generate the data for the wheel spinner based on the public variables
    /// </summary>
    public void PopulateWheelSpinner()
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
        weights = new float[prizeDatabase.Prizes.Length];
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
            weights[i] = lootData.Weight;
        }

        spinnerElements.Reverse();
    }


    private IEnumerator spinningCoroutine;
    /// <summary>
    /// Spins the wheel once
    /// </summary>
    public void SpinWheel()
    {
        if (spinningCoroutine != null) // Dont spin if already spinning
            return;

        int index = SpinWheel(1)[0];

        WinPrizeAt(index);

    }

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
    /// Test for the spinning and simulating the wheel 1000 times
    /// </summary>
    /// <param name="text"></param>
    public void SpinWheel1000(TextMeshProUGUI text)
    {
        Debug.Log("Starting Test");
        int[] indexes = SpinWheel(1000);
        Dictionary<string, int> winningData = new Dictionary<string, int>();
        for (int i = 0; i < indexes.Length; i++)
        {
            int index = indexes[i];
            string name = spinnerElements[index].lootData.PrizeData.Name;
            if (!winningData.ContainsKey(name))
            {
                winningData.Add(name, 1);
            }
            else
            {
                winningData[name]++;
            }
        }

        var sortedDict = winningData.OrderBy(entry => entry.Key).ToDictionary(entry => entry.Key, entry => entry.Value);

        string result = "Output Data:\n";
        foreach (var item in sortedDict)
        {
            string data = item.Key + " : " + item.Value + " Hits | ~" + (int)(((float)item.Value / indexes.Length) * 100f) + "%" + "\n";
            result += data;
            Debug.Log(data);
        }
        text.text = result;

        Debug.Log("Test Complete, ran " + indexes.Length + " Spins");

    }

    /// <summary>
    /// Return an array of winning indexes for the spinner
    /// </summary>
    public int[] SpinWheel(int amount)
    {
        int[] winningIndexes = ProbabilityManager.PickIndexes(weights, amount);

        return winningIndexes;
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
