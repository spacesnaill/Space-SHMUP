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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
