using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed = 2f;
    Rigidbody2D rb;

    float horiz;
    float vert;

    private Inventory inventory;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        inventory = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        horiz = InputController.Instance.Horizontal.Value;
        vert = InputController.Instance.Vertical.Value;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horiz, vert).normalized * speed;
    }
}
