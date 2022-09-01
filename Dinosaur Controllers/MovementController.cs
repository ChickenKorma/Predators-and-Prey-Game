using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Transform objTransform;

    Rigidbody2D rb;

    SpriteRenderer rend;

    private void Awake()
    {
        objTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    public bool Move(Vector3 target, float speed)
    {
        Vector3 direction = target - objTransform.position;

        if(direction.magnitude > 0.05f)
        {
            Vector3 newPos = objTransform.position + (direction.normalized * speed * Time.fixedDeltaTime * Mathf.Clamp(direction.magnitude * 2, 0, 1));
            rb.MovePosition(new Vector3(newPos.x, newPos.y, 0));

            if(Mathf.Abs(direction.x) > 0.2f)
            {
                rend.flipX = direction.x > 0;
            }  
        }

        return direction.magnitude > 0.1f;
    }
}
