using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float GrowTime = 1f;

    private int stage = 0;
    private float timer;
    private bool ticking = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        timer = GrowTime;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ticking)
        {
            if (timer < 0)
            {
                timer = GrowTime;
                stage++;
                animator.SetInteger("Stage", stage);
                if (stage > 4)
                {
                    ticking = false;
                }
            }
            timer -= Time.deltaTime;
        }
    }   
}
