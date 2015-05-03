using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [System.NonSerialized]
    public bool isJumping;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    // Get input data from PlayerInput
    public void RecieveInput(float Distance, Vector2 Direction)
    {
        Rigidbody2D characterRigidBody;
        

        characterRigidBody = gameObject.GetComponent<Rigidbody2D>( );
        if (characterRigidBody != null)
        {
             Direction.x = 0.0f;
            if (Direction.y > 0.0f && !isJumping)
            {
                characterRigidBody.AddForce(Direction * Mathf.Clamp(Distance, 0, 200));

                gameObject.GetComponent<Animator>().SetBool("Jump", true);
                isJumping = true;
            }
            else
            {
                characterRigidBody.AddForce( Direction * Mathf.Clamp( Distance, 0, 250 ) );
            }
        }
    }

    // When the character collides with another GameObject
    void OnCollisionEnter2D(Collision2D Col)
    {
        gameObject.GetComponent<Animator>( ).SetBool( "Jump", false );
        isJumping = false;
    }
}
