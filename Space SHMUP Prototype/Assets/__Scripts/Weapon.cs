using UnityEngine;
using System.Collections;

//this is an enum of the various possible weapon types
//it also includes a "shield" type to allow a shield power-up
//items marked [NI] below are Not Implemented in this book
public enum WeaponType
{
    none, //the default / no weapon
    blaster, //a simple blaster
    spread, //two shots simultaneously
    phaser, //shot that move in waves [NI]
    missile, //homing missiles [NI]
    laser, //damage over time [NI]
    shield, //raise shieldLevel
}

//the WeaponDefinition class allows you to set the properties
//of a specific weapon in the Inpsector. Main has an array
//of WeaponDefinitions that makes this possible
//[System.Serializable] tells Unity to try to view WeaponDefinition
//in the Inspector pane. It doesn't work for every, but it
//will work for simple classes like this
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter; //the letter to show on the power-up
    public Color color = Color.white; //color of collar and power up
    public GameObject projectilePrefab; // prefab for projectile
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; //amount of damage caused
    public float continuousDamage = 0; //damage per second (laser)
    public float delayBetweenShots = 0;
    public float velocity = 20; //speed of projectiles
}

//note: weapon prefabs, colors, and so on are set in the class Main
public class Weapon : MonoBehaviour {
    static public Transform PROJECTILE_ANCHOR;

    public bool ___________________________;
    [SerializeField]
    private WeaponType _type = WeaponType.blaster;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShot; //Time last shot was fired


    void Awake()
    {
        collar = transform.Find("Collar").gameObject;
    }

	// Use this for initialization
	void Start () {
        //call SetType() properly for the default _type
        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //find the fireDelegate of the parent
        GameObject parentGo = transform.parent.gameObject;
        if (parentGo.tag == "Hero")
        {
            Hero.S.fireDelegate += Fire;
        }
	}

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0; //you can always fire immediately after _type is set
    }

    public void Fire()
    {
        //if this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;
        //if it hasn't been enough time between shots, return
        if (Time.time - lastShot < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                break;

            case WeaponType.spread:
                p=MakeProjectile();
                p.GetComponent<Rigidbody>().velocity=Vector3.up*def.velocity;
                p=MakeProjectile();
                p.GetComponent<Rigidbody>().velocity=new Vector3(.2f, 0.9f,0)*def.velocity;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab) as GameObject;
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return (p);
    }
    

	// Update is called once per frame
	void Update () {
	
	}
}
