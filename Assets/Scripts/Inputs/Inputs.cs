using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{

    public Controls Controls { get; set; }

    public static Inputs Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Created Inputs Controller");
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        Controls = new Controls();
        Controls.Enable();
    }
}
