using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static int level;

    Text levelText;

    void Awake () {
        levelText = GetComponent<Text>();
    }

    void Start() {
        level = 1;
    }

    void Update () {
        if (ScoreManager.score >= level * 1000) {
            level += 1;
        }
        levelText.text = level.ToString();
    }
}
