using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float speed = 10f; //the speed in m/s
    public float fireRate = 0.3f; //seconds/shot (Unused)
    public float health = 10;
    public int score = 100; //points earned for destroying this

    public int showDamageForFrame = 2; //# frames to show damage
    public float powerUpDropChance = 1f; //chance to drop a powerup

    public bool _________________;

    public Bounds bounds; //the Bounds for this and its children
    public Vector3 boundsCenterOffset; // Dist of bounds.center from position
    public Color[] originalColors;
    public Material[] materials; // all the materials of this & its children
    public int remainingDamageFrames = 0; //damage frames left

    void Awake()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        if (remainingDamageFrames > 0)
        {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0)
            {
                UnShowDamage();
            }
        }
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

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case"ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //enemies don't take damage unless they're onscreen
                //this stops the player from shooting them before they are visible
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }
                //hurt this enemy
                ShowDamage();
                //get the damage amount from teh Projectile.type & Main.W_DEFS
                health -= Main.W_DEFS[p.type].damageOnHit;
                if (health <= 0)
                {
                    //tell the main singleton that this ship has been destroyed
                    Main.S.ShipDestroyed(this);
                    //destroy this Enemy
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        remainingDamageFrames = showDamageForFrame;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    } 
}
