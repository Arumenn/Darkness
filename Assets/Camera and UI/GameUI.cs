using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Text>().text = SceneManager.GetActiveScene().name;
	}
}
