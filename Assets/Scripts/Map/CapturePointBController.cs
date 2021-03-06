﻿using UnityEngine;
using System.Collections;

public class CapturePointBController : MonoBehaviour
{

    public int PointsPerSecond = 5;
    public int CaptureTime = 10;

    public Sprite pigBanner, sheepBanner, emptyBanner;

    private float startTime, redPoints, bluePoints;
    private ArrayList nearbyRed, nearbyBlue;

    private string allegiance = "";

    private GameObject banner;
    //private GameObject bannerPig;
    //private GameObject bannerSheep;

    // Use this for initialization
    void Start()
    {

        //bluePoints = GameObject.Find("ResourceSpawner").GetComponent<ResourceSpawner>().blueScore;
        //redPoints = GameObject.Find("ResourceSpawner").GetComponent<ResourceSpawner>().redScore;

        startTime = 0;

        nearbyBlue = new ArrayList();
        nearbyRed = new ArrayList();

        Debug.Log("this is B");
        banner = GameObject.Find("bannerB").gameObject;//.GetComponent<SpriteRenderer>();

        SetBannerEmpty();

    }

    void SetBannerPig() //red
    {
        banner.GetComponent<SpriteRenderer>().sprite = pigBanner;
        allegiance = "Red";
    }

    void SetBannerSheep() //blue
    {
        banner.GetComponent<SpriteRenderer>().sprite = sheepBanner;
        allegiance = "Blue";
    }
    void SetBannerEmpty()
    {
        banner.GetComponent<SpriteRenderer>().sprite = emptyBanner;
    }


    public float GetBluePoints()
    {
        return bluePoints;
    }

    public float GetRedPoints()
    {
        return redPoints;
    }


    public string WhosControlling()
    {
        return allegiance;
    }

    public void AgentArrived(GameObject agent)
    {
        switch (agent.tag)
        {
            case "Blue":
                {
                    if (!nearbyBlue.Contains(agent))
                        nearbyBlue.Add(agent);
                } break;
            case "Red":
                {
                    if (!nearbyRed.Contains(agent))
                        nearbyRed.Add(agent);
                } break;
        }
        //Debug.Log("Red Array: " + nearbyRed.Count + " -- Blue Array: " + nearbyBlue.Count);
    }

    public void AgentLeft(GameObject agent)
    {
        switch (agent.tag)
        {
            case "Blue":
                {
                    if (nearbyBlue.Contains(agent))
                        nearbyBlue.Remove(agent);
                } break;
            case "Red":
                {
                    if (nearbyRed.Contains(agent))
                        nearbyRed.Remove(agent);
                } break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //azuis incrementam timer, vermelhos decrementam

        var tendency = -nearbyRed.Count + nearbyBlue.Count;


        if (startTime <= -CaptureTime)
            SetBannerPig();
        if (startTime >= CaptureTime)
            SetBannerSheep();

        if (tendency < 0) //more reds
        {
            startTime -= Time.deltaTime * Mathf.Abs(tendency);
        }
        else if (tendency > 0) //more blues
        {
            startTime += Time.deltaTime * Mathf.Abs(tendency);
        }


        //cap min/max capture time
        if (startTime < -CaptureTime)
            startTime = -CaptureTime;
        if (startTime > CaptureTime)
            startTime = CaptureTime;


        if (allegiance == "Red")
            redPoints += Time.deltaTime * PointsPerSecond;
        else if (allegiance == "Blue")
            bluePoints += Time.deltaTime * PointsPerSecond;


    }


    void OnGUI()
    {

        GUI.Button(new Rect(0, 260, 150, 85), "PointB\nBonus Score:\n Blue: " + Mathf.RoundToInt(bluePoints) + "\n Red: " + Mathf.RoundToInt(redPoints) + "\nCapAlign: " + Mathf.RoundToInt(startTime));

        //GUI.Button(new Rect(0, 300, 150, 40), startTime.ToString());


    }

}
