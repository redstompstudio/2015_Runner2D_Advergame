using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using com.shephertz.app42.paas.sdk.csharp.user;

public class FacebookIntegration : Singleton<FacebookIntegration> 
{
	public bool isConnected;

	public Button FBLoginButton;
	public Image userProfileImage;
	public Text userName;

	private struct FBUserInfo
	{
		public string userID;
		public string userFirstName;
		public string userLastName;
		public string userEMail;
		public string userGender;
		public string userAge;
	}
	private FBUserInfo fbUserInfo;

	void Awake()
	{
		FacebookManager.Instance.Initialize ();

		if(FBLoginButton != null)
		{
			FBLoginButton.onClick.RemoveAllListeners ();
			FBLoginButton.onClick.AddListener (OnLoginButton);
		}
	}

	public void OnLoginButton()
	{
		string permissions = 	FacebookManager.PERMISSION_PUBLIC_PROFILE + "," +
			FacebookManager.PERMISSION_USER_FRIENDS + "," +
			FacebookManager.PERMISSION_USER_LIKES + "," +
			FacebookManager.PERMISSION_EMAIL;

		FacebookManager.Instance.Login (permissions, OnLoginCallback);
	}

	public void OnLoginCallback(FBResult pResult)
	{
		if (string.IsNullOrEmpty (pResult.Error)) 
		{
			isConnected = true;
			FacebookManager.Instance.GetProfilePicture (OnProfilePictureCallback);
			FacebookManager.Instance.GraphAPICall ("first_name,gender,email", OnGetUserInfoCallback);
		}
		else
		{
			isConnected = false;
		}
		//FacebookManager.Instance.CheckUserPermissions ();
	}

	void OnProfilePictureCallback(FBResult pResult)
	{
		if(userProfileImage != null)
			userProfileImage.sprite = Sprite.Create (pResult.Texture, new Rect (0, 0, 128, 128), Vector2.zero);

		FBLoginButton.gameObject.SetActive (false);
	}

	void OnGetUserInfoCallback(FBResult pResult)
	{
		var responseObject = Json.Deserialize(pResult.Text) as Dictionary<string, object>;

		//		foreach (string key in responseObject.Keys) 
		//			Debug.Log (string.Format ("{0} : {1}", key, responseObject [key]));

		if(userName != null)
			userName.text = responseObject["first_name"] + " " + responseObject["last_name"];

		fbUserInfo.userFirstName = responseObject ["first_name"].ToString();

		if(responseObject.ContainsKey("last_name"))
			fbUserInfo.userLastName = responseObject ["last_name"].ToString();

		if (responseObject.ContainsKey ("gender"))
			fbUserInfo.userGender = responseObject ["gender"].ToString ();

		if (responseObject.ContainsKey ("email"))
			fbUserInfo.userEMail = responseObject ["email"].ToString ();
		else
			fbUserInfo.userEMail = "noinfo@info.com";

		if (responseObject.ContainsKey ("id"))
		{
			fbUserInfo.userID = responseObject ["id"].ToString ();
			GetOrCreateUser (fbUserInfo.userID, fbUserInfo.userID, fbUserInfo.userEMail);
		}
	}

	#region GET_USER
	void GetOrCreateUser(string pUserName, string pPassword, string pEmail)
	{
		App42UserServices.Instance.RequestUser(pUserName, 
			(object pResponse) => LinkUserFacebookAccount (pUserName, FB.AccessToken), 	//Found User, link to FB account!
			(System.Exception pEx) => CreateUser (pUserName, pPassword, pEmail));		//User not found, create a new one!
	}

	void OnGetUserSuccess(object pResponse)
	{
	}

	void OnGetUserException(System.Exception pExecption)
	{
		Debug.Log ("Something went wrong while getting a user: " + pExecption.ToString());
	}
	#endregion

	#region CREATE_USER
	void CreateUser(string pUserName, string pPassword, string pEmail)
	{
		App42UserServices.Instance.CreateUser (pUserName, pPassword, pEmail, 
			(object pResponse) => LinkUserFacebookAccount (pUserName, FB.AccessToken), 
			(System.Exception pEx) => Debug.Log ("Something went wrong while creating a user: " + pEx.ToString ()));
	}
	#endregion

	#region LINK FACEBOOK ACCOUNT
	void LinkUserFacebookAccount(string pUserName, string pAccessToken)
	{
		App42UserServices.Instance.LinkFacebookAccount (pUserName, pAccessToken, 
			OnLinkedFBAccountSuccess, OnLinkedFBAccountException);
	}

	void OnLinkedFBAccountSuccess(object pResponse)
	{
		Debug.Log ("Linked FB Account: Success");
	}

	void OnLinkedFBAccountException(System.Exception pEx)
	{
		Debug.Log ("Linked FB Account: Failed!");
	}

	#endregion

#region RANKING

	public void SaveScore(string pScoreBoardName, double pScore)
	{
		if(isConnected)
			App42LeaderBoardServices.Instance.SaveUserScore (pScoreBoardName, fbUserInfo.userEMail, pScore, OnSaveScoreSuccess, OnSaveScoreException);
		else
			Debug.LogError("Can't SaveScore() : user not connected!");
	}

	public void OnSaveScoreSuccess(object pResponse)
	{
		Debug.Log ("Saved Score!");
	}

	public void OnSaveScoreException(System.Exception pException)
	{
		Debug.LogError("SaveScore Exception: " + pException.Message);
	}

	public void UpdateRanking()
	{
		FacebookManager.Instance.GetRanking (OnUpdateRankingCallback);
	}

	void OnUpdateRankingCallback(FBResult pResult)
	{
		List<object> scoresList = FacebookUtil.DeserializeScores(pResult.Text);

		foreach (object score in scoresList)
		{
			var entry = (Dictionary<string, object>)score;
			var user = (Dictionary<string, object>)entry["user"];
		}
	}
#endregion

	public void GetUserLikes()
	{
		FacebookManager.Instance.GetUserLikes (OnGetUserLikesCallback);
	}

	void OnGetUserLikesCallback(List<FBPageData> pPageData)
	{	
		for(int i = 0; i < pPageData.Count; i++)
		{
			Debug.Log( pPageData[i].ToString ());	
		}
	}
}
