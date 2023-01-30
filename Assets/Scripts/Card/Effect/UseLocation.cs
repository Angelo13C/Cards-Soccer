using UnityEngine;

public enum UseLocation
{
    // Players part of your team
    AllyPlayer,
    // All the players (even opponent's one)
    Player,
    // On the field not occupied by players and by the ball
    FreeField,
    // Everywhere in the field (even in parts occupied by the ball/players)
    Field,
    // The ball
    Ball,
    // The goals of the player and of the opponent
    Goal
}

// Define an extension method in a non-nested static class.
public static class UseLocationExtensions
{
    public static bool CanUse(this UseLocation useLocation, Vector2 position, Camera camera, Team team)
    {
        var hit = Physics2D.Raycast(camera.ScreenToWorldPoint(position), Vector2.zero);
        if(!hit)
            return true;

        var hittedTag = hit.transform.GetComponent<Tagged>();
        if(hittedTag == null)
            return useLocation == UseLocation.FreeField || useLocation == UseLocation.Field;
        
        var tag = hittedTag.Tag;

        switch(useLocation)
        {
            case UseLocation.AllyPlayer:
            {
                if(tag != Tag.Player)
                    return false;

                var playerTeam = hit.transform.GetComponent<TeamPart>();
                if(playerTeam == null)
                {
                    Debug.LogError("The player isn't in any team", hit.transform);
                    return false;
                }
                return playerTeam == team;
            }
            case UseLocation.Player:
                return tag == Tag.Player;
            case UseLocation.Field:
                return tag == Tag.Player || tag == Tag.Ball;
            case UseLocation.Ball:
                return tag == Tag.Ball;
            case UseLocation.Goal:
                return tag == Tag.Goal;
            default: 
                return false;
        }
    }
}