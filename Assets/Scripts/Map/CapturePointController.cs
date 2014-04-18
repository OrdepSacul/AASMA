using UnityEngine;
using System.Collections;

public class CapturePointController : MonoBehaviour {

    public int PointsPerSecond = 5;
    public int CaptureTime = 10;

    private int redAgents, blueAgents;

    private SpriteRenderer bannerEmpty;
    private SpriteRenderer bannerPig;
    private SpriteRenderer bannerSheep;

	// Use this for initialization
	void Start () {
        bannerEmpty = transform.FindChild("banner_empty").GetComponent<SpriteRenderer>();
        bannerPig = transform.FindChild("banner_pig").GetComponent<SpriteRenderer>();
        bannerSheep = transform.FindChild("banner_sheep").GetComponent<SpriteRenderer>();

        bannerSheep.enabled = false;
        bannerPig.enabled = false;
	}

    void SetBannerPig(){
        bannerPig.enabled = true;
        bannerEmpty.enabled = false;
        bannerSheep.enabled = false;
    }

    void SetBannerSheep()
    {
        bannerPig.enabled = false;
        bannerEmpty.enabled = false;
        bannerSheep.enabled = true;
    }
    void SetBannerEmpty()
    {
        bannerPig.enabled = false;
        bannerEmpty.enabled = true;
        bannerSheep.enabled = false;
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("BADJORAS!");
        switch (other.tag)
        {
            case "CaptureTriggerRed":
                {
                    Debug.Log("hit!");
                    redAgents++;
                }
                break;
            case "CaptureTriggerBlue":
                {
                    Debug.Log("hit!");
                    blueAgents++;
                }
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "CaptureTriggerRed":
                {

                    redAgents--;
                }
                break;
            case "CaptureTriggerBlue":
                {
                    blueAgents--;
                }
                break;
        }
    }



	// Update is called once per frame
	void Update () {
        if (redAgents + blueAgents > 0)
        {
            
        }
	}


    void OnGUI()
    {
        
    }

}
