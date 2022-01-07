using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterAnim : MonoBehaviour {
    public Transform target;

    [SerializeField] private TMP_Text stateLabel;

    Vector3    oldPos;
    Quaternion oldRot;

    private void Update(){
        Vector3 movement = oldRot * (transform.position - oldPos);
        if(movement.z > 0) {
            stateLabel.text = "forward";
            // forward
        } else if(movement.z < 0) {
            stateLabel.text = "backwards";
            // backwards
        }
        if(movement.x > 0) {
            stateLabel.text = "right";
            // right
        } else if(movement.x < 0) {
            stateLabel.text = "left";
            // left
        }
        oldPos = transform.position;
        oldRot = transform.rotation;
    }

}