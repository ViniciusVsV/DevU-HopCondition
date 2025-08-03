using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorChangeController : MonoBehaviour
{
    public List<ColorChanger> colorChangers = new();

    void Start()
    {
        colorChangers = FindObjectsByType<ColorChanger>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void SetRedColor()
    {
        foreach (ColorChanger colorChanger in colorChangers)
        {
            if (colorChanger.gameObject.activeInHierarchy)
                colorChanger.SetColor(Color.red);
        }
    }

    public void SetBlueColor()
    {
        foreach (ColorChanger colorChanger in colorChangers)
        {
            if (colorChanger.gameObject.activeInHierarchy)
                colorChanger.SetColor(Color.blue);
        }
    }
}