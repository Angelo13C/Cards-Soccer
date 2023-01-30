using UnityEngine;

public enum Tag
{
    Player,
    Goal,
    Walls,
    Ball
}

public class Tagged : MonoBehaviour
{
    [SerializeField] private Tag _tag;
    public Tag Tag => _tag;
}
