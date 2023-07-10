using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] groups;
    public int nextId;

	void Start () {
        nextId = Random.Range(0, groups.Length);
        spawnNext ();
	}
	
	void Update () {
	}

    public GameObject createGroup(Vector3 v) {
        GameObject group = Instantiate(groups[nextId], v, Quaternion.identity);
        return group;
    }

	
	public void spawnNext() {
        createGroup(transform.position);
        nextId = Random.Range(0, groups.Length);
	}
}
