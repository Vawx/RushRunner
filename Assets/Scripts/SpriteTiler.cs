using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpriteTiler : EditorWindow
{
    // Grid settings to make tiled by
    public float GridXSlider = 1;
    public float GridYSlider = 1;

    // Sprites for both the ground and dirt
    public Sprite TileGroundSprite;
    public Sprite TileDirtSprite;

    // Name of the GameObject that holds our tiled Objects
    public string TileSpriteRootGameObjectName = "Tiled Object";

    // Menu option to bring up Sprite Tiler window
    [MenuItem("RushRunner/Sprite Tile")] 
    public static void OpenSpriteTileWindow()
    {
        EditorWindow.GetWindow< SpriteTiler > ( true, "Sprite Tiler" ); 
    }

    // Called to render GUI frames and elements 
    void OnGUI()
    {
        // Setting for GameObject name that holds our tiled Objects
        GUILayout.Label("Tile Level Object Name", EditorStyles.boldLabel);
        TileSpriteRootGameObjectName = GUILayout.TextField( TileSpriteRootGameObjectName, 25 );

        // Slider for X grid value (left to right)
        GUILayout.Label("X: " + GridXSlider, EditorStyles.boldLabel);
        GridXSlider = GUILayout.HorizontalScrollbar( GridXSlider, 1.0f, 0.0f, 30.0f );
        GridXSlider = (int)GridXSlider;

        // Slider for Y grid value(up to down)
        GUILayout.Label("Y: " + GridYSlider, EditorStyles.boldLabel);
        GridYSlider = GUILayout.HorizontalScrollbar(GridYSlider, 1.0f, 0.0f, 30.0f);
        GridYSlider = (int)GridYSlider;

        // File chose to be our Ground Sprite
        GUILayout.Label("Sprite Ground File", EditorStyles.boldLabel);
        TileGroundSprite = EditorGUILayout.ObjectField(TileGroundSprite, typeof(Sprite), true) as Sprite;

        // File chose to be our Dirt Sprite
        GUILayout.Label("Sprite Dirt File", EditorStyles.boldLabel);
        TileDirtSprite = EditorGUILayout.ObjectField(TileDirtSprite, typeof(Sprite), true) as Sprite;

        // If butt "Create Tiled" is clicked
        if (GUILayout.Button("Create Tiled"))
        {
            // If the Grid settings are both zero, 
            // send notification to user
            if (GridXSlider == 0 && GridYSlider == 0)
            {
                ShowNotification(new GUIContent("Must have either X or Y grid set to a value greater than 0"));
                return;
            }

            // if Dirt and Ground Sprite exist
            if (TileDirtSprite != null && TileGroundSprite !=null)
            {
                // If the Sprites sizes dont match,
                // send notifcation to user
                if (TileDirtSprite.bounds.size.x != TileGroundSprite.bounds.size.x || TileDirtSprite.bounds.size.y != TileGroundSprite.bounds.size.y)
                {
                    ShowNotification(new GUIContent("Both Sprites must be of matching size."));
                    return;
                }

                // Create GameObject and tiled 
                // Objects with user settings
                CreateSpriteTiledGameObject(GridXSlider, GridYSlider, TileGroundSprite, TileDirtSprite, TileSpriteRootGameObjectName);
            }
            else
            {
                // If either Dirt or Ground Sprite dont exist,
                // send notifcation to user
                ShowNotification( new GUIContent( "Must have Dirt and Ground Sprite selected." ) );
                return;
            }
        }
    }

    // Create GameObject and tiled childen based on user settings
    public static void CreateSpriteTiledGameObject(float GridXSlider, float GridYSlider, Sprite SpriteGroundFile, Sprite SpriteDirtFile, string RootObjectName)
    {
        // Store size of Sprite
        float spriteX = SpriteGroundFile.bounds.size.x;
        float spriteY = SpriteGroundFile.bounds.size.y;

        // Create the root GameObject which will hold children that tile
        GameObject rootObject = new GameObject( );

        // Set position in world to 0,0,0
        rootObject.transform.position = new Vector3( 0.0f, 0.0f, 0.0f );

        // Name it based on user settings
        rootObject.name = RootObjectName;

        // Create starting values for while loop
        int currentObjectCount = 0;
        int currentColumn = 0;
        int currentRow = 0;
        Vector3 currentLocation = new Vector3( 0.0f, 0.0f, 0.0f );
        
        // Continue loop until all rows 
        // and columns have been filled
        while (currentRow < GridYSlider)
        {
            // Create a child GameObject, set its parent to root, 
            // name it, and offset its location based on current location
            GameObject gridObject = new GameObject( );
            gridObject.transform.SetParent( rootObject.transform );
            gridObject.name = RootObjectName + "_" + currentObjectCount;
            gridObject.transform.position = currentLocation;

            // Give child gridObject a SpriteRenderer and set sprite on CurrentRow
            SpriteRenderer gridRenderer = gridObject.AddComponent<SpriteRenderer>( );
            gridRenderer.sprite = ( currentRow == 0 ) ? SpriteGroundFile : SpriteDirtFile;

            // Give the gridObject a BoxCollider
            gridObject.AddComponent<BoxCollider2D>();

            // Offset currentLocation for next gridObject to use
            currentLocation.x += spriteX;

            // Increment current column by one
            currentColumn++;

            // If the current collumn is greater than the X slider
            if (currentColumn >= GridXSlider)
            {
                // Reset column, incrmement row, reset x location
                // and offset y location downwards
                currentColumn = 0;
                currentRow++;
                currentLocation.x = 0;
                currentLocation.y -= spriteY;
            }

            // Add to currentObjectCount for naming of 
            // gridObject children.
            currentObjectCount++;
        }
    }
}
