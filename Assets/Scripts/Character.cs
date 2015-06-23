using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public LevelPieceManager LevelManager;

    public GameInfo Game;

    [System.NonSerialized]
    public Vector3 RestartLocation;

    public Canvas GameUI;

    [System.NonSerialized]
    public bool isJumping;
        
    [System.NonSerialized]
    public bool isDead;

    [System.NonSerialized]
    public int CoinCount;

    [System.NonSerialized]
    public int DistanceCount;

    [System.NonSerialized]
    public float CurrentTime;

    private bool isFadeOut;

	// Use this for initialization
	void Start ()
    {
	    RestartLocation = gameObject.transform.position;

        CoinCount = PlayerPrefs.GetInt( "Coins" );
        AddCoins( 0 );
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 lockXPosition = transform.position;
        lockXPosition.x = 2.25f;
        transform.position = lockXPosition;

        if (!isDead && Time.timeScale == 1 )
        {
            CurrentTime += Time.deltaTime + 0.025f;
            if (CurrentTime >= 1.0f)
            {
                AddDistance(1);
                CurrentTime = 0.0f;
            }
        }

        if (isFadeOut)
        {
            SpriteRenderer gameCharacterSprite = gameObject.GetComponent<SpriteRenderer>();
            if (gameCharacterSprite != null)
            {
                float currentAlpha = gameCharacterSprite.color.a;
                currentAlpha -= 0.0085f;

                if (currentAlpha < 0.01f)
                {
                    currentAlpha = 0.0f;
                }

                Color newColorAlpha = new Color(1.0f, 1.0f, 1.0f, currentAlpha);
                gameCharacterSprite.color = newColorAlpha;
            }
        }
	}

    // Get input data from PlayerInput
    public void RecieveInput(float Distance, Vector2 Direction)
    {
        Rigidbody2D characterRigidBody; 
        Animator characterAnimator;

        characterRigidBody = gameObject.GetComponent<Rigidbody2D>( );
        characterAnimator = gameObject.GetComponent<Animator>( );
        if (characterRigidBody != null && characterAnimator != null && LevelManager.bGameRunning)
        {
            Direction.x = 0.0f;
            if (Direction.y > 0.0f && !isJumping)
            {
                characterRigidBody.AddForce(Direction * Mathf.Clamp(Distance, 0, 200));

                characterAnimator.SetBool("Jump", true);
                isJumping = true;
            }
            else if ( Direction.y < 0.0f )
            {
                characterRigidBody.AddForce( Direction * Mathf.Clamp( Distance, 0, 250 ) );
            }
        }
    }

    // When the character collides with another GameObject
    void OnCollisionEnter2D(Collision2D Col)
    {
        Animator characterAnimator;

        characterAnimator =  gameObject.GetComponent<Animator>( );
        if (characterAnimator != null)
        {
            characterAnimator.SetBool( "Jump", false );
            isJumping = false;
        }
    }

    // Kills the character
    public void KillCharacter()
    {
        Rigidbody2D characterRigidBody;

        if (!isDead)
        {
            characterRigidBody = gameObject.GetComponent<Rigidbody2D>();
            if (characterRigidBody != null)
            {
                characterRigidBody.AddForce(new Vector2(Random.Range(-1, 1), 1) * 512);
                isDead = true;
                isFadeOut = true;

                if (Game != null)
                {
                    Game.HideRestartButton(false);
                }

                PlayerPrefs.SetInt("Coins", CoinCount);
                PlayerPrefs.Save();
            }
        }
    }

    // Revives the character
    public void ReviveCharacter()
    {
        Vector3 resetLocation = RestartLocation;
        Color resetColorAlpha = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
        SpriteRenderer gameCharacterSprite = gameObject.GetComponent<SpriteRenderer>( );

        gameObject.transform.position = resetLocation;
        isDead = false;
        isFadeOut = false;

        if (gameCharacterSprite != null)
        {
            gameCharacterSprite.color = resetColorAlpha;
        }

        DistanceCount = 0;
    }

    // Adds to the Distance count and updates the UI
    public void AddDistance(int Additional)
    {
        DistanceCount += Additional;
        if (GameUI != null && LevelManager.bGameRunning)
        {
            Text distanceText = GameUI.transform.Find( "DistanceBackground/DistanceValue" ).GetComponent<Text>( );
            if (distanceText != null)
            {
                distanceText.text = DistanceCount.ToString( );
            }
        }

        if (DistanceCount >= 100)
        {
            if (Game != null)
            {
                Game.AwardAchievment( GameInfo.RunnerAchievements.RA_Yards, 100 );
            }
        }
    }

    // Add to the coin count
    public void AddCoins(int Additional)
    {
        CoinCount += Additional;
        if (GameUI != null)
        {
            Text coinText = GameUI.transform.Find( "CoinBackground/CoinValue" ).GetComponent<Text>( );
            if (coinText != null)
            {
                coinText.text = CoinCount.ToString( );
            }
        }

        if (CoinCount >= 100)
        {
            if (Game != null)
            {
                Game.AwardAchievment( GameInfo.RunnerAchievements.RA_Pickups, 100 );
            }
        }
    }
}
