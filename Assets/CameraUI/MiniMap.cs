using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    Light lLight;

    private void Start() {
        lLight = GetComponentInChildren<Light>();
    }

    private void OnPreCull() {
        if (lLight != null) {
            lLight.enabled = true;
        }
    }

    private void OnPostRender() {
        if (lLight != null) {
            lLight.enabled = false;
        }
    }
}
