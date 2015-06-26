using UnityEngine;
using System.Collections;
using System.Collections.Generic; //required to use Lists or Dictionaries
public class Main : MonoBehaviour {
    static public Main S; //singleton

    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f; // the # enemies/second
    public float enemySpawnPadding = 1.5f; //Padding for position
    public WeaponDefinition[] weaponDefinitions;

    public bool _________________;

    public WeaponType[] activeWeaponTypes;
    public float enemySpawnRate; //delay between enemy spawns

    void Awake()
    {
        S = this;
        //Set utils.camBounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        //0.5 enemies/second = enemySpawnRate of 2
        enemySpawnRate = 1f / enemySpawnPerSecond;
        //Invoke call SpawnEnemy() once after a 2 second delay
        Invoke("SpawnEnemy", enemySpawnRate);
    }

    void Start()
    {
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++)
        {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }
    }

    public void SpawnEnemy()
    {
        //pick a random enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
        //poistion the enemy above the screen with a random x position
        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + enemySpawnPadding;
        float xMax = Utils.camBounds.max.x - enemySpawnPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.camBounds.max.y + enemySpawnPadding;
        go.transform.position = pos;
        //call SpawnEnemy() again in a couple of seconds
        Invoke("SpawnEnemy", enemySpawnRate);
    }

    public void DelayedRestart(float delay)
    {
        //invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        //reload _Scene_0 to restart the game
        Application.LoadLevel("__Scene_0");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
