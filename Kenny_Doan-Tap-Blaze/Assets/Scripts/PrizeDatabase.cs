using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Data/PrizeDatabase")]
public class PrizeDatabase : ScriptableObject
{
    public LootData[] Prizes;


#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        if (UnityEditor.EditorApplication.isCompiling) return;
        if (UnityEditor.EditorApplication.isUpdating) return;

        if (Application.isPlaying)
        {
            return;
        }


        float totalWeight = 0;
        for (int i = 0; i < Prizes.Length; i++)
        {
            totalWeight += Prizes[i].Weight;

            if (i >= 1 && i == Prizes.Length - 1 && Prizes[i].ID == Prizes[i - 1].ID)
            {
                LootData cc = Prizes[i];
                cc.ID++;
                cc.Weight = 1;
                Prizes[i] = cc;
            }
        }


        for (int i = 0; i < Prizes.Length; i++)
        {
            LootData cc = Prizes[i];
            cc.ChanceToSpawn = Prizes[i].Weight != 0 ? (Prizes[i].Weight / totalWeight) : 0;
            Prizes[i] = cc;
        }
    }
#endif

}
#if UNITY_EDITOR


[CustomEditor(typeof(PrizeDatabase))]
public class PrizeDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PrizeDatabase database = (PrizeDatabase)target;
        base.OnInspectorGUI();

        if (database.Prizes != null)
        {
            string orderInfo = "";
            for (int i = 0; i < database.Prizes.Length; i++)
            {
                orderInfo += database.Prizes[i].PrizeData.Name + ": " + (int)(database.Prizes[i].ChanceToSpawn * 100) + "%" + "\n";

            }

            EditorGUILayout.HelpBox(orderInfo, MessageType.Info);
        }



    }

}
#endif