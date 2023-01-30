using UnityEngine;

public class TeamPart : MonoBehaviour
{
    [SerializeField] private Team _team;
    public Team Team => _team;
}
