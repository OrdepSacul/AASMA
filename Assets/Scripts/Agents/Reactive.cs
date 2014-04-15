using UnityEngine;
using System.Collections;

public class Reactive : MonoBehaviour
{

    private float wanderRadius;
    private NavMeshAgent agent;
    //estado em que o agente se encontra
    private enum Status {Wander, PickResource, PickBomb, Fight};

    private GameObject selectedTarget;

    private ArrayList viewableTargets;

    private Vector3 moveToward, currentPosition, moveDirection, target;
    private Status currentStatus;

    GameObject bomb;
    bool bombPicked;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        bomb = GameObject.FindGameObjectWithTag("Bomb");
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
            currentStatus = Status.Wander;
        }
        else if (other.collider.tag == "Bomb")
        {
            //pick it up
            //other.gameObject.transform.parent = this.transform;
            //other.gameObject.transform.position = transform.position;// -transform.forward;
            //other.transform.GetComponent<NavMeshAgent>().SetDestination(transform.position);
            //other.transform.Translate(transform.position.x, 1, transform.position.z, Space.World); //needs rigidbody?
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
            selectedTarget = other.gameObject;
            currentStatus = Status.PickResource;
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
        //check tag and act accordingly
        if (other.collider.tag == "Coin")
        {
            //if (selectedTarget.name == other.name)
            //{
            //    Debug.Log("Bye coin!!");
            //    selectedTarget = null;
            //    currentStatus = Status.Wander;
            //}
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

        switch (currentStatus)
        {
            case Status.Wander:
                {

                    var distance = agent.remainingDistance;// (moveToward - currentPosition).magnitude;

                    if (distance < 0.1f)
                        moveToward = GetRandomDestination();

                    agent.SetDestination(moveToward);

                    //controlar inteccaocao com a bomba aqui, devivado a probs com rigidbodies/navmesh
                    if ((bomb.transform.position - transform.position).magnitude < 0.4)
                    {
                        bomb.gameObject.transform.parent = this.gameObject.transform;
                        bomb.gameObject.transform.position = this.gameObject.transform.position;
                        if (!bombPicked)
                            bomb.transform.Translate(0, 1, -0.2f, Space.Self);

                    }
                }
                break;

            case Status.PickResource:
                {
                    if (selectedTarget != null)
                        agent.SetDestination(selectedTarget.transform.position);
                    else currentStatus = Status.Wander;
                }

                break;

            case Status.PickBomb:
                { 
                
                }
                break;

            case Status.Fight:
                {

                }
                break;

        }
        


    }
}
