using UnityEngine;
using System.Collections;
using System;

public class Reactive : MonoBehaviour
{

    private float wanderRadius, viewDistance;
    private NavMeshAgent agent;
    //estado em que o agente se encontra
    private enum Status {Wander, PickResource, PickBomb, Fight, CapturePoint};

    private GameObject selectedTarget;

    private Hashtable viewableTargets;

    private Vector3 moveToward, currentPosition, moveDirection, target, redBaseLocation, blueBaseLocation;
    private Status currentStatus;

    GameObject bomb;
    bool bombPicked;

    // Use this for initialization
    void Start()
    {
        redBaseLocation = GameObject.Find("RedAgentSpawner").transform.position;
        blueBaseLocation = GameObject.Find("BlueAgentSpawner").transform.position;
        viewableTargets = new Hashtable();
        agent = GetComponent<NavMeshAgent>();
        bomb = GameObject.FindGameObjectWithTag("Bomb");
        var spawner = transform.name.Contains("blue") ? GameObject.Find("BlueAgentSpawner").GetComponent<Spawner>() : GameObject.Find("RedAgentSpawner").GetComponent<Spawner>();

        //setup agent
        agent.speed = spawner.moveSpeed;
        agent.acceleration = spawner.agentAcceleration;
        this.wanderRadius = spawner.wanderRadius;
        this.GetComponent<SphereCollider>().radius = viewDistance = spawner.agentViewDistance; //2.27f
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
            viewableTargets.Remove(other.gameObject.name);
            Destroy(other.gameObject);
            GameObject.Find("ResourceSpawner").GetComponent<ResourceSpawner>().RegisterPickup(this.tag);
            SetNearestTarget();
            //currentStatus = Status.Wander;
            //Debug.Log(viewableTargets);
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
            //Debug.Log("Saw a coin!!");
            //selectedTarget = other.gameObject;
            //currentStatus = Status.PickResource;
            if (!viewableTargets.Contains(other.name))
                viewableTargets.Add(other.name, other.gameObject);
            SetNearestTarget();
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

    }



    Vector3 GetRandomDestination() {

        NavMeshHit hit;

        while (true)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;

            randomDirection += transform.position;

            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            if (hit.position.y < 0.2) break;
        }

        //Debug.Log(transform.name + "  " + hit.position);
        return hit.position;

    }

    bool CarriesBomb(GameObject agent) {
        return agent.transform.childCount > 0 ? true : false;    
    }

    void PickBomb() {

        bomb.gameObject.transform.parent = this.gameObject.transform;
        bomb.gameObject.transform.position = this.gameObject.transform.position;
        if (!bombPicked)
            bomb.transform.Translate(0, 1, -0.2f, Space.Self);

    }

    void PlaceBomb() {
        GameObject.Find("BombSpawner").GetComponent<BombSpawner>().BombScore(this.tag); 
        GameObject.Find("BombSpawner").GetComponent<BombSpawner>().ResetBomb();        
    }
    
    void SetNearestTarget() {
        var min = 99999.0f;
        GameObject nearest = null;

        var e = viewableTargets.GetEnumerator();
        //try
        //{
            while (e.MoveNext())
            {
                var key = e.Key as string;
                var value = e.Value as GameObject;
                if (GameObject.Find(key) == null)
                {
                    viewableTargets.Remove(key);
                    break;
                }
                    
                //Debug.Log(value.name);
                var temp = (value.transform.position - transform.position).magnitude;
                if (temp < min)
                {
                    min = temp;
                    nearest = value;
                }

            }
            //Debug.Log(nearest.gameObject);
            if (nearest != null)
                selectedTarget = nearest.gameObject;
            else
                selectedTarget = null;
        //}
        //catch (Exception ex)
        //{
        //    Debug.Log("BADJORAS!!");
        //    viewableTargets.Remove(selectedTarget.name);
        //    selectedTarget = null;
        //}
    }


    // Update is called once per frame
    void Update()
    {
        //mantem actualizada a posicao actual
        currentPosition = transform.position;

        //controlar inteccaocao com a bomba aqui, devivado a probs com rigidbodies/navmesh :/
        //controlar a prioridade da bomba em relacao a resources aqui?
        if (bomb.transform.parent == null)
        {
            if ((bomb.transform.position - transform.position).magnitude <= viewDistance)
                currentStatus = Status.PickBomb;
        }
        else if (transform.childCount > 0) {
            //Debug.Log("Distance to bases: " + (redBaseLocation - this.transform.position).magnitude + " - " + (blueBaseLocation - this.transform.position).magnitude);
            if (
                ((redBaseLocation - this.transform.position).magnitude < 1 && this.tag == "Blue") ||
                ((blueBaseLocation - this.transform.position).magnitude < 1 && this.tag == "Red")
               )
                PlaceBomb();
        }


        switch (currentStatus)
        {
            case Status.Wander:
                {

                    if (viewableTargets.Count > 0)
                        currentStatus = Status.PickResource;

                    var distance = agent.remainingDistance;// (moveToward - currentPosition).magnitude;

                    if (distance < 0.1f)
                        moveToward = GetRandomDestination();

                    agent.SetDestination(moveToward);

                }
                break;

            case Status.PickResource:
                {
                    if (viewableTargets.Count == 0 || selectedTarget == null)
                        currentStatus = Status.Wander;

                    //such.. ugly.. code..
                    try
                    {
                        agent.SetDestination(selectedTarget.transform.position);
                    }catch(Exception e)
                    {
                        //Debug.Log(e.Message);
                        SetNearestTarget();
                    }
                        
                }

                break;

            case Status.PickBomb:
                {
                    if ((bomb.transform.position - transform.position).magnitude < 0.4)
                    {
                        PickBomb();
                        currentStatus = Status.Wander;
                    }
                    agent.SetDestination(bomb.transform.position);

                }
                break;

            case Status.Fight:
                {

                }
                break;

            case Status.CapturePoint:
                {
                    
                    
                }
                break;

        }
        


    }
}
