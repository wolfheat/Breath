
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemID
{
    public string Value;
}

public interface IObjectWithType
{
    public abstract int Type {get;}
}

public abstract class BaseObjectWithType : MonoBehaviour, IObjectWithType
{
    public abstract int Type { get;}
}
