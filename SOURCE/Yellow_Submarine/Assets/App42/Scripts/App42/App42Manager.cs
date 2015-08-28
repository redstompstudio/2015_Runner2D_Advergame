using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using com.shephertz.app42.paas.sdk.csharp.game;
using com.shephertz.app42.paas.sdk.csharp.user;
using AssemblyCSharp;

public class App42Manager : MonoBehaviour 
{
	private static App42Manager instance;

	public string API_KEY = "";
	public string SECRET_KEY = "";

	public static App42Manager Instance {
		get {
			if (instance == null)
				instance = FindObjectOfType (typeof(App42Manager)) as App42Manager;
			return instance;
		}
	}

	void Awake()
	{
		instance = this;
	}

	/*
	private ServiceAPI serviceAPI;
	private GameService gameService;

	private SocialService socialService;
	private SocialResponse socialResponse;

	private ScoreBoardService scoreBoardService;
	private ScoreBoardResponse scoreBoardResponse;

	

#region USERS
	public void CreateUser()
	{
		
	}
#endregion

#region SOCIAL SERVICES - FACEBOOK
	public void InitializeSocialService()
	{
		if(socialService == null)
			socialService = serviceAPI.BuildSocialService ();
	}

	/// <summary>
	/// Link a facebook account with App42 services.
	/// </summary>
	/// <param name="pUserName">P user name.</param>
	/// <param name="pAccessToken">P access token.</param>
	/// <param name="appID">App I.</param>
	/// <param name="appSecret">App secret.</param>
	/// <param name="pCallback">P callback.</param>
	public void LinkUserFacebookAccount(string pUserName, string pAccessToken, 
		string appID = null, string appSecret = null, App42CallBack pCallback = null)
	{
		if (socialService == null)
			InitializeSocialService ();

		if (appID == null || appSecret == null)
			socialService.LinkUserFacebookAccount (pUserName, pAccessToken, pCallback);
		else
			socialService.LinkUserFacebookAccount (pUserName, pAccessToken, appID, appSecret, pCallback);
	}

	/// <summary>
	/// Gets the user's Facebook profile.
	/// </summary>
	/// <param name="pAccessToken">P access token.</param>
	/// <param name="pCallback">P callback.</param>
	public void GetFacebookProfile(string pAccessToken, App42CallBack pCallback)
	{
		if (socialService == null)
			InitializeSocialService ();
		
		socialService.GetFacebookProfile (pAccessToken, pCallback);
	}

	/// <summary>
	/// Gets the facebook profiles from IDs.
	/// </summary>
	/// <param name="pProfileIDs">P profile I ds.</param>
	/// <param name="pCallback">P callback.</param>
	public void GetFacebookProfilesFromIDs(List<string> pProfileIDs, App42CallBack pCallback)
	{
		if (socialService == null)
			InitializeSocialService ();

		socialService.GetFacebookProfilesFromIds (pProfileIDs, pCallback);
	}

	/// <summary>
	/// Gets the facebook friends from user token.
	/// </summary>
	/// <param name="pAccessToken">P access token.</param>
	/// <param name="pCallback">P callback.</param>
	public void GetFacebookFriendsFromUserToken(string pAccessToken, App42CallBack pCallback)
	{
		if (socialService == null)
			InitializeSocialService ();
		
		socialService.GetFacebookFriendsFromAccessToken (pAccessToken, pCallback);
	}

	/// <summary>
	/// Gets the facebook friends from user.
	/// </summary>
	/// <param name="pUserName">P user name.</param>
	/// <param name="pCallback">P callback.</param>
	public void GetFacebookFriendsFromUser(string pUserName, App42CallBack pCallback)
	{
		if (socialService == null)
			InitializeSocialService ();
		
		socialService.GetFacebookFriendsFromLinkUser (pUserName, pCallback);
	}
#endregion

#region SCORE SERVICE

	public void InitializeScoreService()
	{
		if (scoreBoardService == null)
			scoreBoardService = serviceAPI.BuildScoreBoardService ();
	}

	public void SaveUserScore(string pGameName, string pUserName, double pGameScore,
		App42CallBack pCallback)
	{
		if (scoreBoardService == null)
			InitializeScoreService ();

		scoreBoardService.SaveUserScore (pGameName, pUserName, pGameScore, pCallback);
	}	

	public void GetScoresByUser(string pGameName, string pUserName, App42CallBack pCallback)
	{
		if (scoreBoardService == null)
			InitializeScoreService ();

		scoreBoardService.GetScoresByUser (pGameName, pUserName, pCallback);
	}

	public void GetHighestScoreByUser(string pGameName, string pUserName, App42CallBack pCallback)
	{
		if (scoreBoardService == null)
			InitializeScoreService ();

		scoreBoardService.GetHighestScoreByUser (pGameName, pUserName, pCallback);
	}
#endregion
*/
}
