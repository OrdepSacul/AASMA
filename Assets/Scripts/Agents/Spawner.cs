using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Transform agent;

    public int NumAgents = 5;
    public float moveSpeed = 2;
    public float agentAcceleration = 8;
    public float wanderRadius = 5;
    public float agentViewDistance = 2;
    public float agentAngularSpeed = 360;

    private int allTimeAgents;
    private Vector3 spawnPoint;

	// Use this for initialization
	void Start () {

        spawnPoint = transform.position;
        allTimeAgents = 0;

        for (int i = 0; i < NumAgents; i++)
        {
            SpawnAgent();
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}



    void SpawnAgent() {

        var newAgent = Instantiate(agent, new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z), Quaternion.identity);
        newAgent.name = agent.name.Contains("blue") ? "blueagent" + allTimeAgents : "redagent" + allTimeAgents;
        allTimeAgents++;
    
    }



}
