using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorController : MonoBehaviour
{
    MovementController moveControl;

    public Transform nearestPrey;

    [SerializeField] bool leaving = false;

    private void Awake()
    {
        moveControl = gameObject.GetComponent<MovementController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.instance.PredatorBorn(transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameplayManager.instance.playing)
        {
            if (!leaving)
            {
                if(nearestPrey != null)
                {
                    moveControl.Move(nearestPrey.position, GameplayManager.instance.predatorSpeed);
                }               
            }
            else
            {
                if (transform.position.y < -6 || transform.position.y > 6 || transform.position.x > 10 || transform.position.x < -10)
                {
                    GameplayManager.instance.predators.Remove(transform);
                    Destroy(gameObject);
                }
                else
                {
                    Vector3 direction = transform.position;
                    Vector3 target = transform.position + direction.normalized;

                    moveControl.Move(target, GameplayManager.instance.predatorSpeed);
                }
            }
        }      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prey") && GameplayManager.instance.prey.Contains(collision.transform))
        {
            GameplayManager.instance.PreyDeath(collision.transform); 

            leaving = true;
        }
    }

}
