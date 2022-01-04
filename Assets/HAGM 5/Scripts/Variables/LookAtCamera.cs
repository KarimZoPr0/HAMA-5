using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    public Transform cam;
    void Update() {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
