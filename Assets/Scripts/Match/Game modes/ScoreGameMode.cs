using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Score (Game Mode)", menuName = "ScriptableObjects/Game mode/Score")]
public class ScoreGameMode : GameMode
{
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Team _team1, _team2;

    private const int PlayersPerTeam = 3;
    
    public override void Prepare()
    {
        void CreateTeam(Team team)
        {
            for(var i = 0; i < PlayersPerTeam; i++)
            {
                var player = Instantiate(_playerPrefab);
                player.GetComponent<TeamPart>().Team = team;
                player.GetComponent<NetworkObject>().Spawn();
            }
        }
        
        CreateTeam(_team1);
        CreateTeam(_team2);
    }

    public override void Start()
    {
        
    }
}
