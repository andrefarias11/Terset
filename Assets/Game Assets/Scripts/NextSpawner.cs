using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSpawner : MonoBehaviour {

    private Spawner spawner;
    private GameObject currentGroupObject;
    private int currentGroupId; 

	
    void Awake () {
        spawner = FindObjectOfType<Spawner>();
	}

    void createStoppedGroup () {
        currentGroupObject = spawner.createGroup(transform.position);
        currentGroupId = spawner.nextId;
        var group = (Group) currentGroupObject.GetComponent(typeof(Group));
        group.AlignCenter();
        group.enabled = false;
    }


    void deleteCurrentGroup() {
        Destroy(currentGroupObject);
    }

    void Start() {
        createStoppedGroup();
    }
	
	void Update () {
        if (currentGroupId != spawner.nextId) {
            deleteCurrentGroup();
            createStoppedGroup();
        }
	}
}
