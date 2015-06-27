using UnityEngine;
using System.Collections;

public class Enemy_4 : Enemy {

    //Enemy_4 will start offscreen and then pick a random point on screen to
    //move to. Once it has arrived, it will pick another random point and continue
    //until the player has shot it down
    public Vector3[] points; //stores the p0 and p1 for interpolation
    public float timeStart; //bith time for this Enemy_4
    public float duration = 4; //duration of movement

	// Use this for initialization
	void Start () {
        points = new Vector3[2];
        //there is already an initial position chosen by Main.SpawnEnemy()
        //so add is to points as the initial p0 and p1
        points[0] = pos;
        points[1] = pos;

        InitMovement();
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
	
}
