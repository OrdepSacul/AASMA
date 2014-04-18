using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour {

    public int MaxResources;
    public Transform agent;
    public int resourceValue = 5;
    
    private float randX = 0.0f;
    private float randZ = 0.0f;
    private int currentResources;
    private int blueScore, redScore;

    private int allTimeCoins;


    // Use this for initialization
    void Start()
    {

        allTimeCoins = 0;

        for (int i = 0; i < MaxResources; i++)
        {
            SpawnResource();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (currentResources < MaxResources)
            SpawnResource();
                        
    }



    void SpawnResource()
    {
        
        var newCoin = Instantiate(agent, GetRandomDestination2(), Quaternion.identity);
        newCoin.name = "coin" + allTimeCoins; //give it a name so we can identify it in an array if needed
        currentResources++;
        allTimeCoins++;

        //add to array?

    }

    public void RegisterPickup(string team)
    {
        //Debug.Log("pickup!");
        currentResources--;
        if (team == "Blue")
            blueScore += resourceValue;
        else
            redScore += resourceValue;
    }


    Vector3 GetRandomDestination2()
    {

        NavMeshHit hit;

        while (true)
        {
            //Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

            var MapSize = GameObject.Find("Map").transform.localScale;

            randX = (Random.value * 2.0f - 1) * (MapSize.x / 2 - 1);
            randZ = (Random.value * 2.0f - 1) * (MapSize.z / 2 - 1);

            Vector3 randomPos = new Vector3(randX, 0.0f, randZ);

            NavMesh.SamplePosition(randomPos, out hit, 0.1f, 1);
            if (hit.position.y < 0.1) break;
        }

        //Debug.Log(transform.name + "  " + hit.position);
        return hit.position;

    }


    void OnGUI()
    {

        GUILayout.Button("Red Score: " + redScore + " - Blue Score: " + blueScore);

    }
}
