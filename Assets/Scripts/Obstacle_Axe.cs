using UnityEngine;
using System.Collections;

public class Obstacle_Axe : MonoBehaviour 
{
    // When our collider collides with another collider
    void OnTriggerEnter2D(Collider2D Col)
    {
        Character gameCharacter;

        if (Col.gameObject.name == "Character")
        {
            gameCharacter = Col.gameObject.GetComponent<Character>( );
            if (gameCharacter != null)
            {
                gameCharacter.KillCharacter( );
            }
        }
    }
}
