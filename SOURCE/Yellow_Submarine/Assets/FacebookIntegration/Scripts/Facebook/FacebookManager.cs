using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System.Text;

public class FacebookManager : MonoBehaviour 
{
	private static FacebookManager instance;

	public const string PERMISSION_PUBLIC_PROFILE = "public_profile";
	public const string PERMISSION_USER_FRIENDS = "user_friends";
	public const string PERMISSION_EMAIL = "email";
	public const string PERMISSION_USER_ABOUT_ME = "user_about_me";
	public const string PERMISSION_USER_BIRTHDAY = "user_birthday";
	public const string PERMISSION_USER_LIKES = "user_likes";

	public delegate void OnFacebookDelegate(FBResult pResult);
	private OnFacebookDelegate onLoginCallback;
	private OnFacebookDelegate onGetProfilePictureCallback;
	private OnFacebookDelegate onGetUserInfoCallback;

	public delegate void OnFBPageInfosDelegate(List<FBPageData> pPageDatas);

	private Dictionary <string, string> userProfileInfo;

	private StringBuilder command;

#region PROPERTIES
	public static FacebookManager Instance
	{
		get {
			if (instance == null)
				instance = FindObjectOfType (typeof(FacebookManager)) as FacebookManager;

			return instance;
		}
	}

	public bool IsLoggedIn
	{
		get{
			return FB.IsLoggedIn;
		}
	}

	public bool IsInitialized{
		get{
			return FB.IsInitialized;
		}
	}
#endregion

	public void Initialize()
	{
		FB.Init (OnInitializationCompleted, OnHideUnity);
	}

	void OnInitializationCompleted ()
	{
		if(FB.IsLoggedIn)
			Debug.Log ("Initilization Completed: Already Logged In!");
	}

	public void Login(string pPermissions, OnFacebookDelegate pCallback)
	{
		if (string.IsNullOrEmpty(pPermissions))
			pPermissions = PERMISSION_PUBLIC_PROFILE;

		onLoginCallback += pCallback;

		Debug.Log ("Permissions: " + pPermissions);
		FB.Login(pPermissions, OnLoginCallback);
	}

	void OnLoginCallback (FBResult pResult)
	{
		if (pResult.Error == null) 
		{
		}
		else
			Debug.LogError ("Login Failed: " + pResult.Error.ToString ());

		if (onLoginCallback != null)
			onLoginCallback (pResult);

		onLoginCallback = null;
	}

	public void Logout()
	{
		if(FB.IsLoggedIn)
			FB.Logout();
	}

	public void CheckUserPermissions()
	{
		FB.API ("/me/permissions", Facebook.HttpMethod.GET, (FBResult pResult) => {
			var responseObject = Json.Deserialize(pResult.Text) as Dictionary<string, object>;

			foreach (string key in responseObject.Keys) 
			{
				List<object> list = (List<object>)responseObject[key];
				foreach(object o in list)
				{ 
					Dictionary<string, object> data = (Dictionary<string, object>)o;
					Debug.Log(string.Format("{0} : {1}", data["permission"], data["status"]));
				}
			}
		});
	}
		
	private void OnHideUnity(bool pIsUnityShowing)
	{
	}

	public void GetProfilePicture(OnFacebookDelegate pPictureCallback)
	{
		onGetProfilePictureCallback += pPictureCallback;

		FB.API (FacebookUtil.GetPictureURL ("me", 128, 128), Facebook.HttpMethod.GET, (FBResult result) => {
			if (onGetProfilePictureCallback != null)
				onGetProfilePictureCallback (result);

			onGetProfilePictureCallback = null;		
		});
	}

	public void GetUserInfos(string pParams, OnFacebookDelegate pCallback)
	{
		FB.API ("/me/?fields=id," + pParams, Facebook.HttpMethod.GET, (FBResult result) => {
			if(pCallback != null)
				pCallback(result);
		});
	}

	public void GraphAPICall (string pParams, OnFacebookDelegate pCallback)
	{
		FB.API ("/me/?fields=id," + pParams, Facebook.HttpMethod.GET, (FBResult result) => {
			if(pCallback != null)
				pCallback(result);
		});
	}

	public void GetRanking(OnFacebookDelegate pCallback)
	{
		//FB.API ("/app/scores?fields=score,user.limit(10)", Facebook.HttpMethod.GET, (FBResult result) => {
		FB.API ("/" + FB.AppId + "/scores?fields=score,user.limit(10)", Facebook.HttpMethod.GET, (FBResult result) => {
			if(pCallback != null)
				pCallback(result);	
		});
	}

	public void SetRankingScore(int pScore, OnFacebookDelegate pCallback)
	{
		var scoreData = new Dictionary<string, string> ();

		scoreData ["score"] = pScore.ToString();

		FB.API ("/me/scores", Facebook.HttpMethod.POST, (FBResult result) => {
		//FB.API ("/app/scores", Facebook.HttpMethod.POST, (FBResult result) => {
			if(pCallback != null)
				pCallback(result);	
		}, scoreData);
	}

	public void GetUserLikes(OnFacebookDelegate pCallback, int pLimit = 50)
	{
		command = new StringBuilder ("/me/likes/fields?fields=");

		//Request the name of the pages.
		command.Append ("name");

		//Request the pages category.
		command.Append(",category");

		//Number of likes of the page.
		command.Append (",likes");

		//The number of pages returned.
		command.Append ("&limit=");
		command.Append (pLimit.ToString ());

		FB.API (command.ToString(), Facebook.HttpMethod.GET, (FBResult result) =>{
			if(pCallback != null)
			{
				pCallback(result);
			}
		});
	}

	public void GetUserLikes(OnFBPageInfosDelegate pCallback, int pLimit = 50)
	{
		command = new StringBuilder ("/me/likes?fields=");

		//Request the name of the pages.
		command.Append ("name");

		//Request the pages category.
		command.Append(",category");

		//Number of likes of the page.
		command.Append (",likes");

		//The number of pages returned.
		command.Append ("&limit=");
		command.Append (pLimit.ToString ());

		FB.API (command.ToString(), Facebook.HttpMethod.GET, (FBResult result) =>{
			if(pCallback != null)
			{
				List<FBPageData> pagesDataList = new List<FBPageData>();

				var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
				var data = (List<object>)responseObject ["data"];

				for(int i = 0; i < data.Count; i++)
				{
					FBPageData pageData = new FBPageData();

					var info = (Dictionary<string, object>)data [i];

					pageData.pageName = info["name"].ToString();
					pageData.pageID = info["id"].ToString();
					pageData.pageCategory = info["category"].ToString();
					pageData.pageLikeCount = int.Parse(info["likes"].ToString());

					pagesDataList.Add(pageData);
				}

				pCallback(pagesDataList);
			}
		});
	}
}
