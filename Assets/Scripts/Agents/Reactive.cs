using UnityEngine;
using System.Collections;

public class Reactive : MonoBehaviour
{

    private float wanderRadius;
    private NavMeshAgent agent;
    //estado em que o agente se encontra
    private enum Status {Wander, PickResource, PickBomb, Fight};

    private GameObject selectedTarget;
    private Vector3 moveToward, currentPosition, moveDirection, target;
    private Status currentStatus;

    // Use this for initialization
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        var spawner = transform.name.Contains("blue") ? GameObject.Find("BlueAgentSpawner").GetComponent<Spawner>() : GameObject.Find("RedAgentSpawner").GetComponent<Spawner>();

        //setup agent
        agent.speed = spawner.moveSpeed;
        agent.acceleration = spawner.agentAcceleration;
        this.wanderRadius = spawner.wanderRadius;
        this.GetComponent<SphereCollider>().radius = spawner.agentViewDistance; //2.27f
        agent.angularSpeed = spawner.agentAngularSpeed;
        currentPosition = transform.position;

        //set current state-machine status
        currentStatus = Status.Wander;

        //initialize wander direction
        moveToward = GetRandomDestination();
    }

    // O QUE O AGENTE TOCA
    //only works on non-trigger colliders
    void OnCollisionEnter(Collision other)
    {

        //Debug.Log("Collision!");
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            GameObject.Find("ResourceSpawner").GetComponent<ResourceSpawner>().RegisterPickup(this.tag);
        }
        else if (other.collider.tag == "Bomb")
        {
            //pick it up
        }
        else if (other.collider.tag == "Blue" && this.tag == "Blue")
        {
            //assist
        }
        else if (other.collider.tag == "Red" && this.tag == "Blue")
        {
            //fight
        }
        else if (other.collider.tag == "Wall")
        {
            //Debug.Log("HIT A WALL!");
        }

    }

    // O QUE O AGENTE VE
    //only works on trigger colliders
    void OnTriggerEnter(Collider other)
    {
        //check tag and act accordingly
        if (other.collider.tag == "Coin")
        {
            Debug.Log("Saw a coin!!");
        }
        else if (other.collider.tag == "Blue" && this.tag == "Blue")
        {
            //Debug.Log("BROOOO!!");
        }
        else if (other.collider.tag == "Red" && this.tag == "Blue")
        {
            //Debug.Log("ENEMYY!!!");
        }
        else if (other.collider.tag == "CaptureA")
        {
            //Debug.Log("Capture point A!!!");
        }
        else if (other.collider.tag == "CaptureB")
        {
            //Debug.Log("Capture point B!!!");
        }
        else if (other.collider.tag == "Bomb")
        {
            //Debug.Log("Bomb!!!");
        }
    }


    //O QUE O AGENTE DEIXA DE VER
    void OnTriggerExit(Collider other)
    {
        Debug.Log("exited");
    }


    Vector3 GetRandomDestination() {

        NavMeshHit hit;

        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

            randomDirection += transform.position;

            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            if (hit.position.y < 0.2) break;
        }

        //Debug.Log(transform.name + "  " + hit.position);
        return hit.position;

    }


    // Update is called once per frame
    void Update()
    {
        //mantem actualizada a posicao actual
        currentPosition = transform.position;

        if (currentStatus == Status.Wander)
        {

            var distance = agent.remainingDistance;// (moveToward - currentPosition).magnitude;
            //Debug.Log(moveToward);
            //Debug.Log(distance);

            if (distance < 0.1f) {

                moveToward = GetRandomDestination();
   

            }
            //transform.position = Vector3.Lerp(currentPosition, moveToward, Time.time / 100);
            //transform.position = Vector3.MoveTowards(currentPosition, moveToward, moveSpeed / 100.0f  );

            agent.SetDestination(moveToward);
                        
        }

        


    }
}
