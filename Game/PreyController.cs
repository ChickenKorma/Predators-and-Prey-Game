using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyController : MonoBehaviour
{
    MovementController moveControl;

    public Transform nearestPredator;

    [SerializeField] float lastBreed, breedRate;

    float wanderTime;

    Vector3 wanderTarget;

    Animator anim;

    bool running;

    private void Awake()
    {
        moveControl = GetComponent<MovementController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.instance.PreyBorn(transform);

        lastBreed = Time.timeSinceLevelLoad + (GameplayManager.instance.breedRate * Random.Range(0.1f, 0.9f));

        wanderTarget = new Vector3(Random.Range(-8.5f, 8.5f), Random.Range(-4.5f, 4.5f), 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameplayManager.instance.playing)
        {
            float distance = 1000;
            Vector3 direction = Vector3.zero;

            if (nearestPredator != null)
            {
                direction = transform.position - nearestPredator.position;
                distance = direction.magnitude;
            }

            if (distance < 3f || (distance < 5f && running))
            {
                Vector3 target = transform.position + direction.normalized;
                moveControl.Move(target, GameplayManager.instance.preySpeed);

                wanderTime = 0;

                running = true;
            }
            else if (Time.timeSinceLevelLoad > GameplayManager.instance.breedRate + lastBreed)
            {
                lastBreed = Time.timeSinceLevelLoad;
                Spawner.instance.SpawnPrey(transform.position);

                running = false;

                wanderTime = 0;
            }
            else
            {
                if (wanderTime > 3 || Vector3.Magnitude(wanderTarget - transform.position) < 0.1f)
                {
                    wanderTarget = new Vector3(Random.Range(-8.5f, 8.5f), Random.Range(-4.5f, 4.5f), 0);

                    wanderTime = 0;
                }
                else
                {
                    wanderTime += Time.deltaTime;
                }

                moveControl.Move(wanderTarget, GameplayManager.instance.preySpeed * 0.5f);

                running = false;
                
            }

            anim.SetBool("Fast", running);
        }              
    }
}
