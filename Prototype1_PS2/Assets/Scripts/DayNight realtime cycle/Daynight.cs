using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daynight : MonoBehaviour {

    public string time;
    public float timespeed = 1.0f;
    private float currentTime;
    private float maxTime = 86400.0f;

	// Use this for initialization
	void Start () {
        time = System.DateTime.Now.ToString("hh:mm::ss");

        SetToRealTime();
       

        float rot = 360 * (currentTime / maxTime) - 90;
        transform.rotation = Quaternion.Euler(rot, 0, 0);

    }
	
	// Update is called once per frame
	void Update () {

        float rot = (360 * (1 / maxTime)) * Time.deltaTime * timespeed;
        transform.Rotate(rot, 0, 0);
        

        currentTime += Time.deltaTime * timespeed;

        SetTimeToString();
        //This need to be applied to both sun and moon with opposite position
    }

    void SetToRealTime() {
        currentTime = 0f;

        currentTime += System.DateTime.Now.Hour * 3600f;
        currentTime += System.DateTime.Now.Minute * 60f;
        currentTime += System.DateTime.Now.Second;
    }

    void SetTimeToString()
    {
        float sec = currentTime;
        time = "";

        int hours = Mathf.FloorToInt(sec / 3600);
        sec -= hours * 3600;

        int minutes = Mathf.FloorToInt(sec / 60);
        sec -= minutes * 60;

        int seconds = Mathf.FloorToInt(sec);
        sec -= seconds;

        time += hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();
    }
}
