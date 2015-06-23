using UnityEngine;
using System.Collections;

public class LevelPieceManager : MonoBehaviour
{
    [System.NonSerialized]
    // If the game is being played or at main menu
    public bool bGameRunning;

    // MainMenu level piece
    public LevelPiece IdleLevelPiece;

    // Starting level piece 
    public LevelPiece StartingLevelPiece;

    // Level pieces to rotate 
    public LevelPiece[] LevelPieces;

    // How quickly the level pieces move
    public float LevelPiecesMoveRate;

    // The currently active Level Piece
    private LevelPiece[] ActiveLevelPieces;

	// Use this for initialization
	void Start( ) 
    {
        ActiveLevelPieces = new LevelPiece[2];
        ResetLevelPieces( bGameRunning );
	}

    // Set ActiveLevelPieces to Idle
    void SetIdlePieces( )
    {
        ActiveLevelPieces[ 0 ] = StartingLevelPiece;
        ActiveLevelPieces[ 1 ] = IdleLevelPiece;
        ActiveLevelPieces[ 1 ].transform.position = StartingLevelPiece.gameObject.transform.FindChild("EndLocation").position;
    }

    // Set ActiveLevelPieces to Game
    void SetGamePieces( )
    {
        ActiveLevelPieces[0] = StartingLevelPiece;
        ActiveLevelPieces[1] = GetRandomLevelPiece();
        ActiveLevelPieces[1].transform.position = StartingLevelPiece.gameObject.transform.FindChild("EndLocation").position;
    }
	
	// Update is called once per frame
	void Update ( )
    {
        for (int i = 0; i < ActiveLevelPieces.Length; i++)
        {
            Vector3 newLocation = ActiveLevelPieces[i].transform.position;
            newLocation.x -= LevelPiecesMoveRate * Time.deltaTime;

            ActiveLevelPieces[i].transform.position = newLocation;

            if (ActiveLevelPieces[i].transform.position.x < transform.position.x)
            {
                if (bGameRunning)
                {
                    if (ActiveLevelPieces[i] == StartingLevelPiece)
                    {
                        ActiveLevelPieces[i].gameObject.SetActive(false);
                    }

                    ActiveLevelPieces[i].transform.position = ActiveLevelPieces[i].GetInitialLocation();

                    ActiveLevelPieces[i] = GetRandomLevelPiece();
                    ActiveLevelPieces[i].transform.position = FindOtherLevelPiece(ActiveLevelPieces[i]).gameObject.transform.FindChild("EndLocation").position;
                    ActiveLevelPieces[i].ResetAllChildrenCoins();
                }
                else
                {
                    LevelPiece nextLevelPiece = ( i == 0 ) ? ActiveLevelPieces[ 1 ] : ActiveLevelPieces[ 0 ];

                    ActiveLevelPieces[ i ].transform.position = nextLevelPiece.gameObject.transform.Find( "EndLocation" ).position;
                }
            }
        }
	}

    // Get the other LevelPiece
    // from the LevelPieces
    // Array
    private LevelPiece FindOtherLevelPiece(LevelPiece CurrentLevelPiece)
    {
        for (int i = 0; i < ActiveLevelPieces.Length; i++)
        {
            if (ActiveLevelPieces[ i ] != CurrentLevelPiece)
            {
                return ActiveLevelPieces[ i ];
            }
        }
        return null;
    }

    // Get random level piece
    // from LevelPieces Array
    private LevelPiece GetRandomLevelPiece()
    {
        LevelPiece returnPiece = null;
        while (returnPiece == null)
        {
            for (int i = 0; i < LevelPieces.Length; i++)
            {
                if ( !isActivePiece( LevelPieces[ i ] ) )
                {
                    returnPiece = LevelPieces[ i ];
                }
            }
        }
        return returnPiece;
    }

    // Check if LevelPiece
    // is already used.
    private bool isActivePiece(LevelPiece Piece)
    {
        for (int i = 0; i < ActiveLevelPieces.Length; i++)
        {
            if (Piece == ActiveLevelPieces[ i ])
            {
                return true;
            }
        }
        return false;
    }

    // Resets all LevelPieces
    public void ResetLevelPieces( bool bRunning )
    {
        bGameRunning = bRunning;

        StartingLevelPiece.transform.position = StartingLevelPiece.GetInitialLocation();
        StartingLevelPiece.gameObject.SetActive(true);
        IdleLevelPiece.gameObject.SetActive( !bGameRunning );

        for (int i = 0; i < LevelPieces.Length; i++)
        {
            LevelPieces[i].transform.position = LevelPieces[i].GetInitialLocation();
            LevelPieces[i].ResetAllChildrenCoins();
        }

        if (bGameRunning)
        {
            SetGamePieces();
        }
        else
        {
            SetIdlePieces();
        }
    }
}
