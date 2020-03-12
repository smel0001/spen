using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 10f;
    public float duration = 2f;

    [System.NonSerialized]
    public Vector2 dir;

    private float timer;

    void Start()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.Find("Player").transform.position;
        this.dir = direction.normalized;

        rb = GetComponent<Rigidbody2D>();
        timer = duration;
    }

    void FixedUpdate()
    {
        if (timer > 0f)
        {
            rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
            timer -= Time.fixedDeltaTime;
        }
        else
        {
            ObjectManager.Instance.RemoveObject(this.gameObject.GetInstanceID());
            //Destroy(gameObject);
        }
    }
}
