using UnityEngine;

[CreateAssetMenu(fileName = "Game mode selector", menuName = "ScriptableObjects/Game mode selector")]
public class GameModeSelector : ScriptableObject
{
    [SerializeField] private GameMode _selected;
    public GameMode Selected => _selected;
}
