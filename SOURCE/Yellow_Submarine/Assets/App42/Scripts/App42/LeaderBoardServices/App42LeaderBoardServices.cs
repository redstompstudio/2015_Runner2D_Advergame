using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;

public class App42LeaderBoardServices : MonoBehaviour 
{
	private static App42LeaderBoardServices instance;

	private ServiceAPI serviceAPI;
	ScoreBoardService scoreBoardService;
	//private UserService userService;

	public static App42LeaderBoardServices Instance {
		get {
			if (instance == null)
				instance = FindObjectOfType (typeof(App42LeaderBoardServices)) as App42LeaderBoardServices;

			if (instance.scoreBoardService == null)
				instance.Initialize ();

			return instance;
		}
	}

	void Awake()
	{
		instance = this;
		serviceAPI = new ServiceAPI (App42Manager.Instance.API_KEY, App42Manager.Instance.SECRET_KEY);
	}

	private void Initialize()
	{
		scoreBoardService = serviceAPI.BuildScoreBoardService();
	}

	public void SaveUserScore (string pLeaderBoardName, string pUserName, double pScore, 
	                          App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response (pSuccess, pException);
		scoreBoardService.SaveUserScore (pLeaderBoardName, pUserName, pScore, response);

//		scoreBoardService.GetTopNRankers
	}

	public void GetTopNRankers (string pGameName, int pMax, App42Response.OnSuccessDelegate pSuccess,
	                           App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response (pSuccess, pException);
		scoreBoardService.GetTopNRankers (pGameName, pMax, response);
	}
}
