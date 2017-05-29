using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    private void OnPreCull() {
        GetComponentInChildren<Light>().enabled = true;
    }

    private void OnPostRender() {
        GetComponentInChildren<Light>().enabled = false;
    }
}
