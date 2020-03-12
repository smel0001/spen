using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float GrowTime = 1f;

    //Maybe better way to do this?
    //Titles?
    public int HarvestItemID = 3;
    public int SeedItemId = 4;
    public int NumStages = 4;
    private int stage = 0;
    private float timer;
    private bool ticking = true;

    private Animator animator;

    private Item harvestDrop;
    private Item seedDrop;

    // Start is called before the first frame update
    void Start()
    {
        harvestDrop = ItemDatabase.Instance.FetchItemById(HarvestItemID);
        seedDrop = ItemDatabase.Instance.FetchItemById(SeedItemId);
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
                if (stage > NumStages - 1)
                {
                    ticking = false;
                    tag = "Clickable";
                }
            }
            timer -= Time.deltaTime;
        }
    }

    public void OnCast()
    {
        if (stage > NumStages - 1)
        {
            //Drop a flower and some seeds
            harvestDrop.CreateInWorld(this.transform.position);
            seedDrop.CreateInWorld(this.transform.position);
            ObjectManager.Instance.RemoveObject(this.gameObject.GetInstanceID());
        }
    }
}
