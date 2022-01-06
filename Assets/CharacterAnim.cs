using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnim : MonoBehaviour {
    public Transform target;

    [SerializeField] private TMP_Text velocity;
    [SerializeField] private TMP_Text stateLabel;

    Vector3              oldPos;
    Quaternion           oldRot;
    public NavMeshAgent agent;

    private void Update() {

        velocity.text = agent.velocity.ToString();
        if(agent.velocity.z > 0) {
            stateLabel.text = "forward";
        } else if(agent.velocity.z < 0) {
            stateLabel.text = "backwards";
            // backwards
        }
        if(agent.velocity.x > 0) {
            stateLabel.text = "right";
            // right
        } else if(agent.velocity.x < 0) {
            stateLabel.text = "left";
            // left
        }
    }

}