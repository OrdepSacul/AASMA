using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{

    public Transform bomb;
    public int resourceValue = 30;

    private Vector3 defaultPosition = new Vector3(0.0f,0.1f,0.0f);
    private int currentResources;
    private int blueScore, redScore;
    private GameObject bombGameObject;
    private int allTimeCoins;
    private Object bombInstance;
    public float timer; // set duration time in seconds in the Inspector
    private float initTimer;

    // Use this for initialization
    void Awake()
    {
        initTimer = timer;
        Debug.Log(initTimer);
        SpawnBomb();
        bombGameObject = GameObject.FindGameObjectWithTag("Bomb");
    }

    // Update is called once per frame
    void Update()
    {

        if (bombGameObject.transform.position != defaultPosition && bombGameObject.transform.parent == null)
        {
            initTimer -= Time.deltaTime; // I need timer which from a particular time goes to zero
            guiText.text = initTimer.ToString("F0");
        } else initTimer = timer;
 
        if (initTimer > 0)
        {
            guiText.text = initTimer.ToString("F0");
        } 
        else // timer is <= 0
        {
            guiText.text = "TIME OVER\nPress X to restart"; // when it goes to the end-0,game ends (shows text: time over...) 
            ResetBomb();
        }


    }



    void SpawnBomb()
    {

        bombInstance = Instantiate(bomb, defaultPosition, Quaternion.identity);

    }

    public void BombPickup(Transform carrier)
    {        
        bombGameObject.transform.parent = carrier; //somar posicao do transform do agente, se der barraca    
    }


    void BombDropped(Transform carrier) {
        bombGameObject.transform.position = carrier.position;
        bombGameObject.transform.parent = null;    
    }

    void ResetBomb() {
        initTimer = timer;
        bombGameObject.transform.position = defaultPosition;
        bombGameObject.transform.parent = null;    
    }

}
