using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorChangeController : MonoBehaviour
{
    [SerializeField] private float colorChangeSpeed = 0.5f;
    [SerializeField]
    private Color[] colorPalette = {
        Color.white,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };

    private List<ColorChanger> colorChangers = new();
    private int currentColorIndex = 0;
    private float lerpFactor = 0f;
    private Color currentColor;
    private Color targetColor;

    void Start()
    {
        // Find all color changers (including inactive ones)
        colorChangers = FindObjectsByType<ColorChanger>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        // Initialize colors
        currentColor = colorPalette[0];
        targetColor = colorPalette[1];

        // Set initial color for all changers
        foreach (var changer in colorChangers)
            changer.SetColor(currentColor);
    }

    void Update()
    {
        if (colorChangers.Count == 0) return;

        // Increment the lerp factor based on time
        lerpFactor += Time.deltaTime * colorChangeSpeed;

        // Calculate current color
        Color newColor = Color.Lerp(currentColor, targetColor, lerpFactor);

        // Update all color changers
        foreach (var changer in colorChangers)
        {
            if (changer != null) // Check if destroyed
                changer.SetColor(newColor);
        }

        // When we reach the target color, set a new target
        if (lerpFactor >= 1f)
        {
            lerpFactor = 0f;
            currentColor = targetColor;
            currentColorIndex = (currentColorIndex + 1) % colorPalette.Length;
            targetColor = colorPalette[currentColorIndex];
        }
    }
}