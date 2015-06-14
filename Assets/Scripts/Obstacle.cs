using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    // Pivot to rotate around
    public Transform Pivot;
    
    // Randome range for rotation speed
    public Vector2 RotationSpeedGap;

    // Actual rotation speed based on RotationSpeedGap
    private float RotationSpeed;

	// Use this for initialization
	void Start ()
    {
	    RotationSpeed = Random.Range( RotationSpeedGap.x, RotationSpeedGap.y );
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.timeScale == 1)
        {
            Pivot.transform.Rotate(Vector3.forward, RotationSpeed);
        }
	}

    // When a collider collides with our collider
    void OnTriggerEnter2D(Collider2D Col)
    {
        Character gameCharacter;

        if (Col.gameObject.name == "Character")
        {
            gameCharacter = Col.gameObject.GetComponent<Character>();
            if (gameCharacter != null)
            {
                gameCharacter.KillCharacter();
            }
        }
    }
}
