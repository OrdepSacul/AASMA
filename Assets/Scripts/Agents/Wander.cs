using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {


    private Vector3 moveDirection;
    //private ResourceSpawner resScript = GameObject.Find("ResourceSpawner").GetComponent<ResourceSpawner>();
    private int moveSpeed = 2;

	// Use this for initialization
	void Start () {
	
	}
	

    int score = 0;
 
    //only works on non-trigger colliders
    void OnCollisionEnter2D( Collision2D other) {

        Debug.Log("Collision!");
        if (other.collider.tag == "Coin") {
            score += 5;
            Destroy(other.gameObject);
            GameObject.Find("ResourceSpawner").GetComponent<ResourceSpawner>().RegisterPickup( this.tag );
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

    }


    //only works on trigger colliders
    void OnTriggerEnter2D( Collider2D other ) { 
        //check tag and act accordingly
        if (other.collider2D.tag == "Coin") {
            Debug.Log("Saw a coin!!");
        }
        else if (other.collider2D.tag == "Blue" && this.tag == "Blue")
        {
            Debug.Log("BROOOO!!");
        }
        else if (other.collider2D.tag == "Red" && this.tag == "Blue")
        {
            Debug.Log("ENEMYY!!!");
        }
        else if (other.collider2D.tag == "CaptureA")
        {
            Debug.Log("Capture point A!!!");
        }
        else if (other.collider2D.tag == "CaptureB")
        {
            Debug.Log("Capture point B!!!");
        }
        else if (other.collider2D.tag == "Bomb")
        {
            Debug.Log("Bomb!!!");
        }
    }


	// Update is called once per frame
	void Update () {
        
        //this.gameObject.transform.Rotate(new Vector3(0,0,1), 30.0f*Time.deltaTime, Space.Self);


        // 1
        Vector3 currentPosition = transform.position;
        // 2
        if (Input.GetButton("Fire1"))
        {
            // 3
            Vector3 moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 4
            moveDirection = moveToward - currentPosition;
            moveDirection.z = 0;
            moveDirection.Normalize();
        }




        Vector3 target = moveDirection * moveSpeed + currentPosition;
        transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);





	}
}
