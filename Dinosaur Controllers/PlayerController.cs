using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MovementController moveControl;

    Vector3 lastMousePosition;

    Animator anim;

    [SerializeField] Texture2D normalCursor, clawCursor;

    private void Awake()
    {
        moveControl = GetComponent<MovementController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lastMousePosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameplayManager.instance.playing)
        {
            bool running = moveControl.Move(lastMousePosition, GameplayManager.instance.playerSpeed);

            anim.SetBool("Idle", !running);
        }           
    }

    private void Update()
    {
        if (GameplayManager.instance.playing)
        {
            if (Input.mousePosition != null)
            {
                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lastMousePosition.z = 0;
            }

            RaycastHit2D hit = Physics2D.Raycast(lastMousePosition, Vector2.zero);

            if (hit.collider != null && Vector3.Magnitude(transform.position - hit.collider.transform.position) < 3.5f)
            {
                ChangeCursor(clawCursor);

                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetTrigger("Bite");

                    if (hit.collider.CompareTag("Prey") && GameplayManager.instance.prey.Contains(hit.transform))
                    {
                        GameplayManager.instance.PreyEaten(hit.collider.transform);
                    }
                    else if (hit.collider.CompareTag("Predator") && GameplayManager.instance.predators.Contains(hit.transform))
                    {
                        GameplayManager.instance.PredatorDeath(hit.collider.transform);
                    }
                }
            }
            else
            {
                ChangeCursor(normalCursor);
            }
        }       
    }

    void ChangeCursor(Texture2D cursor)
    {
        Vector2 hotspot = new Vector2(cursor.width / 2, cursor.height / 2);

        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }
}
