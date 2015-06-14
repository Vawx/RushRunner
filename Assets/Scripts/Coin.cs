using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{

    // When trigger collider is collided with
    void OnTriggerEnter2D(Collider2D Col)
    {
        Character gameChar;
        if (Col.gameObject.name == "Character")
        {
            gameChar = Col.gameObject.GetComponent<Character>( );
            if (gameChar != null)
            {
                if (!gameChar.isDead)
                {
                    gameChar.AddCoins(1);
                    ActivateCoin( false );
                }
            }
        }
    }

    // Activates or deactivates
    // coin
    public void ActivateCoin(bool bActivate)
    {
        SpriteRenderer renderer;
        BoxCollider2D collider;

        renderer = gameObject.GetComponent<SpriteRenderer>( );
        collider = gameObject.GetComponent<BoxCollider2D>( );

        collider.enabled = bActivate;
        renderer.enabled = bActivate;
    }
}
