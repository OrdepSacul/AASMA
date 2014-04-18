using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{

    public Transform bomb;
    public int resourceValue = 30;
    private int redScore, blueScore;
    private Vector3 defaultPosition = new Vector3(0.0f,0.1f,0.0f);
    private GameObject bombGameObject;
    private Object bombInstance;
    public float timer; // set duration time in seconds in the Inspector
    private float initTimer;

    // Use this for initialization
    void Awake() //precisa de ser awake para os agentes encontrarem a bomba no inicio do jogo
    {
<<<<<<< HEAD
        redScore = blueScore = 0;
=======
>>>>>>> 7434bd2fa680747d765cfbd3e2e05261808a5259
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


    public void BombDropped(Transform carrier) {
        bombGameObject.transform.position = carrier.position;
        bombGameObject.transform.parent = null;    
    }

<<<<<<< HEAD
    public void BombScore(string team) {
        if (team == "Red")
            redScore++;
        else blueScore++;
        Debug.Log("BombScore!!");
    }

    public void ResetBomb() {
=======
    void ResetBomb() {
>>>>>>> 7434bd2fa680747d765cfbd3e2e05261808a5259
        initTimer = timer;
        bombGameObject.transform.position = defaultPosition;
        bombGameObject.transform.parent = null;    
    }


    void OnGUI() {

        GUI.Button(new Rect(0, 30, 150, 100), "Bombs Detonated:\n Blue: "+blueScore+"\n Red: "+redScore);
        //GUILayout.Button("Red Bomb Score: " + redScore + " - Blue Bomb Score: " + blueScore, new GUILayoutOption());
                    
    }


}
