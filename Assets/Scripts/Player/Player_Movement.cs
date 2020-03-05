using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float Speed;
    public bool FacesLeft;
    GameObject Hand;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        Hand = GameObject.Find ( "Hand" );
        sprite = GetComponentInChildren<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.anyKey )
        {
            var movement = Vector3.zero;

            if ( Input.GetKey(KeyCode.A))
            {
                movement = new Vector3 ( -Speed, 0 );
            }

            if ( Input.GetKey ( KeyCode.D ) )
            {
                movement += new Vector3 ( Speed, 0 );
            }

            if ( Input.GetKey ( KeyCode.W ) )
            {
                movement += new Vector3 ( 0,Speed );
            }

            if ( Input.GetKey ( KeyCode.S) )
            {
                movement += new Vector3 ( 0,-Speed );
            }




            if ( movement.x > 0 && FacesLeft  ) {
                FacesLeft = false;
                Hand.transform.localScale = new Vector3 ( 1, 1 );
            };

            if ( movement.x < 0 && !FacesLeft )
            {
                FacesLeft = true;
                Hand.transform.localScale = new Vector3 ( -1,1 );
            };

            //Flip sprite
            sprite.flipX = FacesLeft;

            // Change player position
            transform.position += movement;

            SendMessage ( "OnPlayerMoving" , FacesLeft);

        }
    }
}
