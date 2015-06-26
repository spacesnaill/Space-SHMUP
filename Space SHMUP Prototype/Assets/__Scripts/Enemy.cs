using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float speed = 10f; //the speed in m/s
    public float fireRate = 0.3f; //seconds/shot (Unused)
    public float health = 10;
    public int score = 100; //points earned for destroying this

    public bool _________________;

    public Bounds bounds; //the Bounds for this and its children
    public Vector3 boundsCenterOffset; // Dist of bounds.center from position

    void Awake()
    {
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    //this is a Property: a method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void CheckOffscreen()
    {
        //if bounds are still tehir default value
        if (bounds.size == Vector3.zero)
        {
            //then set them
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            //Also find the diff between bounds.center & transform.position
            boundsCenterOffset = bounds.center - transform.position;
        }

        //every time, update teh bounds to the ucrrent positon
        bounds.center = transform.position + boundsCenterOffset;
        //check to see wehether the bounds are completely offscreen
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero)
        {
            //if this enemy has gone off the bottom edge of the screen
            if (off.y < 0)
            {
                //then destroy it
                Destroy(this.gameObject);
            }
        }
    }
}
