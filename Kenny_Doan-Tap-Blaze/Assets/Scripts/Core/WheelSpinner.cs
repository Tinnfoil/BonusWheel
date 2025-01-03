using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class WheelSpinner : MonoBehaviour
{
    public PrizeDatabase prizeDatabase;
    public float[] weights;
    public WheelSpinnerUI wheelUI;

    private void Start()
    {
        PopulateWheelSpinner();
    }

    /// <summary>
    /// Initialize the generate the data for the wheel spinner based on the public variables
    /// </summary>
    public void PopulateWheelSpinner()
    {
        wheelUI.PopulateWheelSpinner(prizeDatabase);

        weights = new float[prizeDatabase.Prizes.Length];
        // Generate the wedges and the elements in them based on the prize data
        for (int i = 0; i < prizeDatabase.Prizes.Length; i++)
        {
            LootData lootData = prizeDatabase.Prizes[i];
            weights[i] = lootData.Weight;
        }

    }
    /// <summary>
    /// Spins the wheel once
    /// </summary>
    public void SpinWheel()
    {
        if (wheelUI.spinningCoroutine != null) // Dont spin if already spinning
            return;

        int index = SpinWheel(1)[0];

        wheelUI.WinPrizeAt(index);
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
            string name = wheelUI.spinnerElements[index].lootData.PrizeData.Name;
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


}
