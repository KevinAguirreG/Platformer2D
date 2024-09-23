using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public float radio;
    public LayerMask playerLayer;
    public Transform playerTransform;

    public float speed;
    public float distance;
    public Vector3 initialPoint;

    private bool watchingRight = false;

    public movementStatus actualStatus;
    public enum movementStatus
    {
        Waiting,
        Following,
        Returning,
    }

    private void Start()
    {
        initialPoint = transform.position;
    }


    private void Update()
    {
        switch (actualStatus)
        {
            case movementStatus.Waiting:
                StatusWaiting();
                break; 
            case movementStatus.Following:
                StatusFollowing();
                break;
            case movementStatus.Returning:
                StatusReturning();
                break;
        }


        
    }

    private void StatusWaiting()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radio, playerLayer);
        if (playerCollider)
        {
            playerTransform = playerCollider.gameObject.transform;

            actualStatus = movementStatus.Following;
        }
    }


    private void StatusFollowing()
    {
        if(playerTransform == null)
        {
            actualStatus = movementStatus.Returning;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

        TurnObjective(playerTransform.position);

        if (Vector2.Distance(transform.position, initialPoint) > distance || 
            Vector2.Distance(transform.position, playerTransform.position) > distance)
        {
            actualStatus = movementStatus.Returning;
            playerTransform = null;
        }
    }

    private void StatusReturning()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPoint, speed * Time.deltaTime);

        TurnObjective(initialPoint);


        if (Vector2.Distance(transform.position, initialPoint) < 0.1f)
        {
            actualStatus = movementStatus.Waiting;
        }
    }

    private void TurnObjective(Vector3 obj)
    {
        if (obj.x > transform.position.x && !watchingRight)
        {
            Turn();
        }else if (obj.x < transform.position.x && watchingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        watchingRight = !watchingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D point in other.contacts)
            {
                if (point.normal.y <= -0.9)
                {
                    Destroy(this.gameObject);

                }
                else
                {
                    print("Perdiste");
                    SceneManager.LoadScene(0);
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
        Gizmos.DrawWireSphere(initialPoint, distance);
    }

}
