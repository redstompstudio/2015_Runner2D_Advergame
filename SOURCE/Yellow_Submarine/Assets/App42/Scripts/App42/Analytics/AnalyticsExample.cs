using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using System.Collections.Generic;

public class AnalyticsExample : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		App42Analytics.Initialize ("Jorge Salvi");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			App42Analytics.TrackEvent ("EVENT_SINGLE", null, OnTrackEventSuccess, OnTrackEventException); 
//			 
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			Dictionary<string,object> properties = new Dictionary<string, object> ();  
			properties.Add ("gold", 555);  			 
			App42Analytics.TrackEvent ("EVENTPROPERTY", properties, OnTrackEventSuccess, OnTrackEventException); 
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			App42Analytics.EnableSessionTracking(true);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			App42Analytics.EnableSessionTracking(false);
		}
	}

	void OnDestroy(){
		App42Analytics.EnableSessionTracking(false);
	}

	void OnTrackEventSuccess(object pResponse)
	{
				
		Debug.Log (pResponse.ToString());

	}
	
	void OnTrackEventException(System.Exception pEx)
	{
		Debug.Log ("Something is wrong!");
	}
}
