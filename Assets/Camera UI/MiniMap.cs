using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    Light light;

    private void Start() {
        light = GetComponentInChildren<Light>();
    }

    private void OnPreCull() {
        if (light != null) {
            light.enabled = true;
        }
    }

    private void OnPostRender() {
        if (light != null) {
            light.enabled = false;
        }
    }
}
