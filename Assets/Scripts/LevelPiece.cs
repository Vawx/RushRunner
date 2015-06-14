using UnityEngine;
using System.Collections;

public class LevelPiece : MonoBehaviour 
{
    // Coin children
    public Coin[] Coins;

    // The initial location of this level piece
    private Vector3 InitialLocation;

	// Use this for initialization
	void Awake () 
    {
	    InitialLocation = transform.position;
	}

    // Get the initial location of this
    // LevelPiece
    public Vector3 GetInitialLocation()
    {
        return InitialLocation;
    }

    // Resets all childre
    // coins
    public void ResetAllChildrenCoins()
    {
        for (int i = 0; i < Coins.Length; i++)
        {
            Coins[ i ].ActivateCoin( true );
        }
    }
}
