using UnityEngine;

public abstract class GameMode : ScriptableObject
{
    public abstract void Prepare();
    public abstract void Start();
}
