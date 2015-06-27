using UnityEngine;
using System.Collections;

//Part is another serializable data storage class just like WeaponDefinition
[System.Serializable]
public class Part
{
    //these three fields need to be defined in the Inspector pane
    public string name; //the name of this part
    public float health; //the amount of health this part has
    public string[] protectedBy; //the other parts that protect this

    //these two fields are set automatically in Start()
    //caching like this makes it faster and easier to find these later
    public GameObject go; //the GameObject of this part
    public Material mat; //the Material to show damage
}

public class Enemy_4 : Enemy {

    //Enemy_4 will start offscreen and then pick a random point on screen to
    //move to. Once it has arrived, it will pick another random point and continue
    //until the player has shot it down
    public Vector3[] points; //stores the p0 and p1 for interpolation
    public float timeStart; //bith time for this Enemy_4
    public Part[] parts; //the arrt of ship Parts
    public float duration = 4; //duration of movement

	// Use this for initialization
	void Start () {
        points = new Vector3[2];
        //there is already an initial position chosen by Main.SpawnEnemy()
        //so add is to points as the initial p0 and p1
        points[0] = pos;
        points[1] = pos;

        InitMovement();

        //cache GameObject and Material of each Part in parts
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
	}

    void InitMovement()
    {
        //pick a new point to move to that is on screen
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.enemySpawnPadding;
        Bounds cBounds = Utils.camBounds;
        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        points[0] = points[1]; //shift points[1] to points[0]
        points[1] = p1; //add p1 as points[1]

        //reset the time
        timeStart = Time.time;
    }

    public override void Move()
    {
        //this completely overrides Enemy.Move() with a linear interpolation

        float u = (Time.time - timeStart) / duration;
        if (u >= 1)
        {
            //if u >=1
            InitMovement(); //then initializes movement to a new point
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2); // apply ease out easing to u

        pos = (1 - u) * points[0] + u * points[1]; //simple linear interpolation
    }
	
    //this  will override the OnCollisionEnter that is part of Enemy.cs
    //Because of the way that MonoBehaviour declares common Unity functions
    //like OnCollisionEnter(), the override keyword is not necessary
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //enemies don't take damage unless they're on screen
                //this stops the player from shooting them before they are visible
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }

                //hurt this enemy
                //find the GameObject that was hit
                //the collision coll has contacts[], an arry of ContactPoints
                //because there was a collision, we're guaranteed that there is at least
                //a contacts[0], and ContactPoints have a reference to thisCollider,
                //which will be the collider for the part of the Enemy_4 that was hit
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    //if prtHit wasn't found then it's usually because, very rarely, thisCollider on
                    //contacts[0] will be the ProjectileHero instead of the ship part
                    //if so, just look for otherCollider instead
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //check whether this part is still protected
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        //if one of the protecting parts hasn't been destroyed
                        if (!Destroyed(s))
                        {
                            //then don't dmage this part yet
                            Destroy(other); //destory this ProjectileHero
                            return; //return before causing damage
                        }
                    }
                }
                //it's not protected, so make it take damage
                //get the damage amount from the Projectile.type and Main.W_DEFS
                prtHit.health -= Main.W_DEFS[p.type].damageOnHit;
                //show dmaange on the part
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    //instead of destroying this enemy, disable the enemy part
                    prtHit.go.SetActive(false);
                }
                //check to see if the whole ship is destroyed
                bool allDestroyed = true; //assume it is destroyed
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        //if a part still exists
                        allDestroyed = false; //change allDestroyed to false
                        break; //and break out of the foreach loop
                    }
                }
                if (allDestroyed)
                {
                    //if it IS completely destroyed
                    Main.S.ShipDestroyed(this);
                    //destroy this enemy
                    Destroy(this.gameObject);
                }
                Destroy(other); //destroy the ProjectileHero
                break;
        }
    }

    //these two functions find a part in this.parts by name or GameObject
    Part FindPart(string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }

    Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }

    //these functions return true if the Part has been destroyed
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            //if no real Part was passed in
            return (true);
        }
        //returns the result of the comparison, prt.health <= 0
        //if prt.health is 0 or less, returns true (yes, it will be destroyed)
        return (prt.health <= 0);
    }

    //thsi changes the color of just one Part to red instead of the whole ship
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        remainingDamageFrames = showDamageForFrame;
    }
}
