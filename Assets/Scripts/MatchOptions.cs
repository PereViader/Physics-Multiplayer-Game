using UnityEngine;
using System.Collections;

public class MatchOptions : MonoBehaviour
{
    [SerializeField]
    private int team = -1;

    [PunRPC]
    public void SetTeam(int team)
    {
        this.team = team;
    }

    public int GetTeam()
    {
        return team;
    }
}