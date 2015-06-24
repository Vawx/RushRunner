using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class GameInfo : MonoBehaviour 
{
    public enum RunnerAchievements
    {
        RA_Rounds,
        RA_Pickups,
        RA_Yards,
    };

    // If game is running or at menu idle
    [System.NonSerialized]
    public bool bGameRunning;

    // MainMenu UI reference
    public Canvas MainMenuUI;

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

        GameCenterManager.RegisterAchievement( "G_100Yards" );
        GameCenterManager.RegisterAchievement( "G_100Pickups" );
        GameCenterManager.RegisterAchievement( "G_10Rounds" );

        GameCenterManager.Dispatcher.addEventListener( GameCenterManager.GAME_CENTER_ACHIEVEMENT_PROGRESS, OnAchievementProgress );
        GameCenterManager.Dispatcher.addEventListener( GameCenterManager.GAME_CENTER_ACHIEVEMENTS_RESET, OnAchievementsReset );

        GameCenterManager.OnAchievementsLoaded += OnAchievementLoaded;

        GameCenterManager.init( );

        GameCenterManager.ResetAchievements( );
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
            LevelManager.ResetLevelPieces( bGameRunning );
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
            GameUI.transform.Find( "BackToMenuButton" ).gameObject.SetActive( !bHide );
        }
    }

    // Game state button was pressed
    public void GameStateButtonPressed( bool bRunning )
    {
        bGameRunning = bRunning;

        if (MainMenuUI != null)
        {
            MainMenuUI.gameObject.SetActive(!bRunning);
        }

        if (GameUI != null)
        {
            GameUI.gameObject.SetActive(bRunning);
        }

        RestartGame( );
    }

    /** ----------------------------
     * GameCenter Events
     * ---------------------------*/

    // When achievements are loaded
    private void OnAchievementLoaded(ISN_Result Result)
    {
        if (Result.IsSucceeded)
        {
            foreach (AchievementTemplate template in GameCenterManager.Achievements)
            {
                print( template.id + ": " + template.progress );
            }
        }
    }

    // When achievements are reset
    private void OnAchievementsReset()
    {

    }

    // When achievments send progress
    private void OnAchievementProgress(CEvent Event)
    {
        ISN_AchievementProgressResult result = Event.data as ISN_AchievementProgressResult;

        if (result.IsSucceeded)
        {
            AchievementTemplate template = result.info;
            print( template.id + ": " + template.progress.ToString( ) );
        }
    }

    // Set achievment for RunnerGame
    public void SubmitAchievmentProgress(RunnerAchievements Achievement, float AchievmentValue )
    {
        string lastAchievement = "";
        switch (Achievement)
        {
            case RunnerAchievements.RA_Pickups:
                    lastAchievement = "G_100Pickups";
                break;
            case RunnerAchievements.RA_Rounds:
                    lastAchievement = "G_10Rounds";
                break;
            case RunnerAchievements.RA_Yards:
                    lastAchievement = "G_100Yards";
                break;
        }

        GameCenterManager.SubmitAchievement(GameCenterManager.GetAchievementProgress(lastAchievement) + AchievmentValue, lastAchievement);    
    }

    // Instead of adding to an achievement progress value, set it as a whole
    public void SubmitAchievementAsWhole(RunnerAchievements Achievment, float AchievmentWholeValue)
    {
        string lastAchievement = "";
        switch (Achievment)
        {
            case RunnerAchievements.RA_Pickups:
                lastAchievement = "G_100Pickups";
                break;
            case RunnerAchievements.RA_Rounds:
                lastAchievement = "G_10Rounds";
                break;
            case RunnerAchievements.RA_Yards:
                lastAchievement = "G_100Yards";
                break;
        }

        GameCenterManager.SubmitAchievement(AchievmentWholeValue, lastAchievement);  
    }

    void OnAuthFinished(ISN_Result res)
    {
        if (res.IsSucceeded)
        {
            IOSNativePopUpManager.showMessage("Player Authored ", "ID: " + GameCenterManager.Player.PlayerId + "\n" + "Alias: " + GameCenterManager.Player.Alias);
        }
        else
        {
            IOSNativePopUpManager.showMessage("Game Center ", "Player auth failed");
        }
    }

}
