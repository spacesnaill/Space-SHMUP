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

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
