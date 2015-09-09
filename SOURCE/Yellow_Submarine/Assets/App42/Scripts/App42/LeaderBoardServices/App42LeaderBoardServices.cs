using UnityEngine;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;

public class App42LeaderBoardServices : Singleton<App42LeaderBoardServices>
{
	private ServiceAPI serviceAPI;
	ScoreBoardService scoreBoardService;

	private void Awake()
	{
		serviceAPI = new ServiceAPI (App42Manager.Instance.API_KEY, App42Manager.Instance.SECRET_KEY);
		Initialize ();
	}

	private void Initialize()
	{
		if (scoreBoardService == null)
			scoreBoardService = serviceAPI.BuildScoreBoardService();
	}

	public void SaveUserScore (string pLeaderBoardName, string pUserName, double pScore, 
	                          App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response (pSuccess, pException);
		scoreBoardService.SaveUserScore (pLeaderBoardName, pUserName, pScore, response);
	}

	public void GetTopNRankers (string pGameName, int pMax, App42Response.OnSuccessDelegate pSuccess,
	                           App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response (pSuccess, pException);
		scoreBoardService.GetTopNRankers (pGameName, pMax, response);
	}

	public void GetTopNRankers(string pGameName, int pMax, string pFBToken,
		App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response (pSuccess, pException);
		scoreBoardService.GetTopNRankersFromFacebook (pGameName, pFBToken, pMax, response);
	}
}
