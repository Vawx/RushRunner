using UnityEngine;
using System.Collections;

public class GameInfo : MonoBehaviour 
{
    // GameUI reference
    public Canvas GameUI;

    // Reference to our Character
    public Character GameCharacter;

    // Refernce to our levelPieceManager
    public LevelPieceManager LevelManager;

    // Fade object
    public Transform FadeObject;

    // Reference to fade GUITexture
    private GUITexture FadeTexture;

    // Is game restarting
    private bool bRestartingGame;

    // Is fading active
    private bool bStartFading;

    // Level and Character need reseting
    private bool bLevelAndCharacterRestart;

    // When object starts
    void Awake( )
    {
        if ( FadeObject != null )
        {
            FadeObject.transform.position = new Vector3( 0.5f, 0.5f, 0.0f );
            FadeTexture = FadeObject.GetComponent<GUITexture>( );
            FadeTexture.pixelInset = new Rect (0.0f, 0.0f, Screen.width, Screen.height );

            RestartGame( );
        }
    }

    // Updates every frame
    void Update( )
    {
        if ( bRestartingGame )
        {
            if ( bStartFading )
            {
                FadeToBlack( );
                if ( FadeTexture.color.a >= 0.95f )
                {
                    bStartFading = false;
                }
            }
            else
            {
                FadeToNormal( );
                if ( FadeTexture.color.a <= 0.5f )
                {
                    if (bLevelAndCharacterRestart)
                    {
                        // Restart level and characters before fade is done.
                        RestartLevelAndCharacter();
                    }

                    if (FadeTexture.color.a <= 0.05f)
                    {
                        bRestartingGame = false;
                        FadeTexture.enabled = false;
                    }
                }
            }
        }
    }

    // Toggle pause, on or off
    public void PauseGame( )
    {
       Time.timeScale = ( Time.timeScale == 1 ) ? 0 : 1;
    }

    // Restarts the game
    public void RestartGame( )
    {
        if (Time.timeScale == 1)
        {
            FadeTexture.enabled = true;
            bLevelAndCharacterRestart = true;
            bRestartingGame = true;
            bStartFading = true;

            HideRestartButton(true);
        }
    }

    // Restart level and character
    private void RestartLevelAndCharacter( )
    {
        bLevelAndCharacterRestart = false;

        if ( GameCharacter != null )
        {
            GameCharacter.ReviveCharacter( );
        }

        if ( LevelManager != null )
        {
            LevelManager.ResetLevelPieces( );
        }
    }

    // Fade screen to black
    private void FadeToBlack( )
    {
        if (FadeTexture != null)
        {
            FadeTexture.color = Color.Lerp( FadeTexture.color, Color.black, 2.0f * Time.deltaTime );
        }
    }

    // Fade screen back to clear
    private void FadeToNormal( )
    {
        if (FadeTexture != null)
        {
            FadeTexture.color = Color.Lerp( FadeTexture.color, Color.clear, 2.0f * Time.deltaTime );
        }
    }

    // Shows or hides the restart butt
    public void HideRestartButton(bool bHide)
    {
        if (GameUI != null)
        {
            GameUI.transform.Find( "RestartButton" ).gameObject.SetActive( !bHide );
        }
    }
}
