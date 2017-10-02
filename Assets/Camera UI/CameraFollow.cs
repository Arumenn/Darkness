using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] bool minimapMode = false;
    GameObject player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    private void LateUpdate() {
        if (minimapMode) {
            Vector3 newPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = newPos;
        } else {
            transform.position = player.transform.position;
        }
    }
}
