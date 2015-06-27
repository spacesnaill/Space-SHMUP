using UnityEngine;
using System.Collections;

//enemy 1 sxtends the Enemy class
public class Enemy_1 : Enemy {
    //because Enemy_1 extends Enemy, the ______bool won't work the same way in the inspector pane

    //# seconds for a full sine wave
    public float waveFrequency = 2;
    //sine wave width in meters
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0 = -12345; //the initial x value of pos
    private float birthTime;

	// Use this for initialization
	void Start () {
	    //set x0 to the initial x positions of Enemy_1
        //this works fine because ths position will have already been set by Main.SpawnEnemy() before Start() runs
        //this is also good because there is no Start() method on Enemy
        x0 = pos.x;

        birthTime = Time.time;

	}
	
    //override the Move function on Enemy
    public override void Move()
    {
        //because pos is a property you can't directly set pos.x so get the pos as an editable Vector3
        Vector3 tempPos = pos;
        //theta adjusts based on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        //base.Move() still handles the movement down in y
        base.Move();
    }

}
