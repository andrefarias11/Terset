using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static int score;

    Text scoreText;

	
    void Awake () {
        scoreText = GetComponent<Text>();
	}

    void Start() {
        score = 0;
    }
	
	void Update () {
        scoreText.text = System.String.Format("{0:D8}", score);
	}
}
