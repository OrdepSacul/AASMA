﻿using UnityEngine;
using System.Collections;

public class navagent : MonoBehaviour {

    public Transform target;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();

        
        Debug.Log("cenas");
	}
	
	// Update is called once per frame
	void Update () {

        agent.SetDestination(target.position);

	}
}
