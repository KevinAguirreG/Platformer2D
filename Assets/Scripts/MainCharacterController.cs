using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public int numJumps;
    public LayerMask layerFloor;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private bool watchingRight = true;
    private int remainingJumps;

    private void Start()
    {
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        remainingJumps = numJumps;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessJump();
    }

    bool TouchingFloor()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, new Vector2(boxCollider2D.bounds.size.x, boxCollider2D.bounds.size.y), 0f, Vector2.down, 0.2f, layerFloor);
        return raycastHit2D.collider != null;
    }
    void ProcessJump()
    {
        if (TouchingFloor())
        {
            remainingJumps = numJumps;
        }
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            remainingJumps --;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void ProcessMovement()
    {
        //Logica del movimiento 
        float inputMovement = Input.GetAxis("Horizontal");

        rigidbody2D.velocity = new Vector2 (inputMovement * speed, rigidbody2D.velocity.y);

        ChangeOrentation(inputMovement);
    }

    void ChangeOrentation (float inputMovement)
    {
        //Si miro a la derecha y me muevo a la izquierda 
        // OR Si miro a la izquierda y me muevo a la derecha
        if ( (watchingRight && inputMovement < 0) || (!watchingRight && inputMovement > 0))
        {
            //Cambiamos el bool de donde estamos voltenado
            watchingRight = !watchingRight;
            //Volteamos al personaje
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}
