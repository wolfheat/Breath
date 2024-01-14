using UnityEngine;

public class ToolHolder : MonoBehaviour
{
    [SerializeField] GameObject[] tools;
    private int activeTool = 0;

    public void ChangeTool(DestructType type)
    {
        if(activeTool != (int)type)
        {
            tools[activeTool].SetActive(false);
            activeTool = (int)type;
            tools[activeTool].SetActive(true);
        }
    }
}
