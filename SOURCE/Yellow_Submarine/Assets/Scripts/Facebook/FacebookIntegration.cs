using UnityEngine;
using UnityEngine.UI;
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

		public User.Profile profile;
	}
	private FBUserInfo fbUserInfo;

	private void Awake()
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
			FacebookManager.Instance.GraphAPICall ("first_name,last_name,gender,email", OnGetUserInfoCallback);
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
		
		if (responseObject.ContainsKey ("id"))
		{
			fbUserInfo.userID = responseObject ["id"].ToString ();

			if (responseObject.ContainsKey ("email"))
				fbUserInfo.userEMail = responseObject ["email"].ToString ();
			else
				fbUserInfo.userEMail = fbUserInfo.userID + "@mail.com";
			
			GetOrCreateUser (fbUserInfo.userEMail, fbUserInfo.userID, fbUserInfo.userEMail);
		}
	}

#region GET_USER
	void GetOrCreateUser(string pUserName, string pPassword, string pEmail)
	{
		App42UserServices.Instance.RequestUser(pUserName, 
			(object pResponse) => LinkUserFacebookAccount (pUserName, FB.AccessToken), 	//Found User, link to FB account!
			(System.Exception pEx) => {
				
				fbUserInfo.profile = new User.Profile();								//Generate the user App42 Profile
				fbUserInfo.profile.firstName = fbUserInfo.userFirstName;
				fbUserInfo.profile.lastName = fbUserInfo.userLastName;	
				fbUserInfo.profile.SetSex(fbUserInfo.userGender);

				CreateUserWithProfile (pUserName, pPassword, pEmail, fbUserInfo.profile);			//User not found, create a new one!
			});		
	}

	void OnGetUserSuccess(object pResponse)
	{
		Debug.Log ("User created: Success!");
	}

	void OnGetUserException(System.Exception pExecption)
	{
		Debug.Log ("GetUser Exception: " + pExecption.Message);
	}
#endregion

#region CREATE_USER
	void CreateUser(string pUserName, string pPassword, string pEmail)
	{
		App42UserServices.Instance.CreateUser (pUserName, pPassword, pEmail, 
			(object pResponse) => LinkUserFacebookAccount (pUserName, FB.AccessToken), 
			(System.Exception pEx) => Debug.Log ("Create User Exception: " + pEx.ToString ()));
	}

	void CreateUserWithProfile(string pUserName, string pPassword, string pEmail, User.Profile pProfile)
	{
		App42UserServices.Instance.CreateUserWithProfile (pUserName, pPassword, pEmail, pProfile,
			(object pRespose) => LinkUserFacebookAccount(pUserName, FB.AccessToken),
			(System.Exception pEx) => Debug.Log("Create With Profile Exception: " + pEx.Message));
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
