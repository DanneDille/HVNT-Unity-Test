using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject buttonPrefab;

    public void SpawnButtons(List<Color>allColors, GameObject aTargetObject) //Create a button for each beacon
    {
        float uiSize = Mathf.Clamp(800 / Mathf.Clamp(allColors.Count, 3,allColors.Count + 3), 0 , 200);
        for (int i = 0; i < allColors.Count; i++)
        { 
            GameObject newButton = Instantiate(buttonPrefab) as GameObject;
            newButton.transform.SetParent(canvas.transform, false);
            newButton.GetComponent<ICanBeColored>().SetColor(allColors[i]);
            newButton.GetComponent<S_ButtonScript>().ChaceInfo(i, aTargetObject);
            newButton.GetComponent<S_ButtonScript>().SetSize(uiSize);
        }
    }
}
