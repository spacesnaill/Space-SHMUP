using UnityEngine;
using System.Collections;

public class Enemy_3 : Enemy {
    
    //Enemy_3 will move following a Bezier curve, which is a linear interporlation between more than two points
    public Vector3[] points;
    public float birthTime;
    public float lifeTime = 10f;

    //again Start works well because it is not used by Enemy
	// Use this for initialization
	void Start () {
        points = new Vector3[3]; //initialize points
        
        //the start positions has already been set by Main.SpawnEnemy()
        points[0] = pos;

        //set xMin and xMax the same way that Main.SpawnEnemy[] does
        float xMin = Utils.camBounds.min.x + Main.S.enemySpawnPadding;
        float xMax = Utils.camBounds.max.x - Main.S.enemySpawnPadding;

        Vector3 v;
        //pick a random middle posiion in the bottom half of the screen
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = Random.Range(Utils.camBounds.min.y, 0);
        points[1] = v;

        //pick a random final positon above the top of the screen
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        //set the birthTime to the current time
        birthTime = Time.time;
	}

    public override void Move()
    {
        //Bezier curves work based on a u value between 0 & 1
        float u = (Time.time - birthTime) / lifeTime;
        //print(u);
        if (u > 1)
        {
            //this Enemy_3 has finished its life
            Destroy(this.gameObject);
            return;
        }

        //Interpolate the three Bezier curve points
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;
        //base.Move();
       
    }

}
