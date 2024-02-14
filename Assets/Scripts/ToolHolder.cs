using UnityEngine;

public class ToolHolder : MonoBehaviour
{
    [SerializeField] GameObject[] tools;

    public void ChangeTool(DestructType type)
    {
        Debug.Log("Tool Changed to "+type);
        foreach (var tool in tools)
            tool.SetActive(false);
        tools[(int)type].SetActive(true);
        
    }
}
