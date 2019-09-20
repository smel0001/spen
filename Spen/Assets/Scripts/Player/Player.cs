using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed = 2f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0f,0f,0f);

        movement.x = InputController.Instance.Horizontal.Value;
        movement.y = InputController.Instance.Vertical.Value;

        //transform.Translate(movement.normalized * speed * Time.deltaTime);
        rb.MovePosition(transform.position + movement.normalized * speed * Time.deltaTime);
    }
}
