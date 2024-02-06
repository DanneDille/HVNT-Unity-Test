using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_ButtonScript : MonoBehaviour,ICanBeColored
{
    // Start is called before the first frame update
    [SerializeField]
    private int index;
    private GameObject targetObject;
    [SerializeField]
    private RectTransform[] uiElemts;
    public void ChaceInfo(int aIndex, GameObject aTargetObject)
    {
        index = aIndex;
        targetObject = aTargetObject;
    }
    public void ButtonPressed()
    {
        targetObject.GetComponent<IHaveIndex>().GiveIndex(index);
    }
    public void SetColor(Color newColor)
    {
           GetComponent<Image>().color = newColor; 
    }
    public void SetSize(float size)
    {
        foreach(RectTransform AUIElemt in uiElemts)
        {
            AUIElemt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            AUIElemt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        }
        
    }
}
