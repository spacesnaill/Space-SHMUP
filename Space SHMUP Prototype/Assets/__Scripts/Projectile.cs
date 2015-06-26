using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    [SerializeField]
    private WeaponType _type;
    //this public property masks the field _type and takes action wehn it is set
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    void Awake()
    {
        //test to se whether this has passed off screen every 2 seconds
        InvokeRepeating("CheckOffScreen", 2f, 2f);
    }

    public void SetType(WeaponType eType)
    {
        //set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        GetComponent<Renderer>().material.color = def.projectileColor;
    }

    void CheckOffScreen()
    {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
