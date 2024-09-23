using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform floorController;

    [SerializeField] private Transform[] waypoints;

    private bool isWaiting;
    private int currentWayPoint;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D point in other.contacts) 
            {
                if(point.normal.y <= -0.9)
                {
                      Destroy(this.gameObject) ;

                }
                else
                {
                    print("Perdiste");
                    SceneManager.LoadScene(0);
                }
            }
           
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != waypoints[currentWayPoint].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayPoint].position, speed * Time.deltaTime);
        }else if(!isWaiting)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        isWaiting = true;   
        yield return new WaitForSeconds(waitTime);
        currentWayPoint++;
        if(currentWayPoint == waypoints.Length)
        {
            currentWayPoint = 0;
        }
        isWaiting = false;
    }
}
