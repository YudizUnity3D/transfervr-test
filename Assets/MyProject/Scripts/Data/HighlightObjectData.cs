using UnityEngine;

[System.Serializable]
/// <summary>
/// This is the data container for highlighting objects  
/// </summary>
public class HighlightObjectData
{
    public Renderer objectRenderer;
    public Material highlightedMaterial;
    public Material normalMaterial;
}
