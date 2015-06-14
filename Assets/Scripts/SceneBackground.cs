using UnityEngine;
using System.Collections;

public class SceneBackground : MonoBehaviour
{
    // Struct to hold data for Background Element
    [System.Serializable]
    public struct BackgroundElement
    {
        public Sprite BackgroundSprite;
        public float MovementSpeed;
        public float MovementDistance; 
        public Vector3 SpriteLocation;
        public Vector3 SpriteScale;

        [System.NonSerialized]
        public Vector3 ObjectStartLocation;
        [System.NonSerialized]
        public GameObject ChildObject;
        [System.NonSerialized]
        public bool bMoveBackwards;
    };

    // Array to store backgrounds for scene
    public BackgroundElement[ ] SceneBackgroundElement;

	// Use this for initialization
	void Start ( )
    {
        SpriteRenderer backgroundRenderer;
        GameObject backgroundObj;

        for (int i = 0; i < SceneBackgroundElement.Length; i++)
        {
            // new GameObject
            backgroundObj = new GameObject( );

            // Parent it, set its position, and scale
            backgroundObj.transform.SetParent( transform );            
            backgroundObj.transform.position = SceneBackgroundElement[i].SpriteLocation;
            backgroundObj.transform.localScale = SceneBackgroundElement[i].SpriteScale;

            // Keep reference of new object and its start location
            SceneBackgroundElement[ i ].ChildObject = backgroundObj;
            SceneBackgroundElement[ i ].ObjectStartLocation = backgroundObj.transform.position;

            // Create Renderer and set Sprite
            backgroundRenderer = backgroundObj.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = SceneBackgroundElement[ i ].BackgroundSprite;
        }
	}
	
	// Update is called once per frame
	void Update ( ) 
    {
        for (int i = 0; i < SceneBackgroundElement.Length; i++)
        {
            // Does the SceneBackgroundElement move
            if (SceneBackgroundElement[i].MovementSpeed > 0)
            {
                // Get current location and offset it by movement speed, based on bMoveBackwards
                Vector3 newSceneBackgroundLoc = SceneBackgroundElement[ i ].ChildObject.transform.position;
                newSceneBackgroundLoc.x += (SceneBackgroundElement[i].bMoveBackwards) 
                    ? -SceneBackgroundElement[i].MovementSpeed * Time.deltaTime
                    : SceneBackgroundElement[i].MovementSpeed * Time.deltaTime;

                // Set location based on movement speed offset
                SceneBackgroundElement[ i ].ChildObject.transform.position = newSceneBackgroundLoc;

                // If ChildObject has moved farther than move distance, have it move the other way.
                if (Vector3.Distance(SceneBackgroundElement[i].ChildObject.transform.position, SceneBackgroundElement[i].ObjectStartLocation) >=
                    SceneBackgroundElement[i].MovementDistance)
                {
                    // Change the direction of the SceneBackgroundElement
                    SceneBackgroundElement[ i ].bMoveBackwards = !SceneBackgroundElement[ i ].bMoveBackwards;
                }
                
            }
        }
	}
}
