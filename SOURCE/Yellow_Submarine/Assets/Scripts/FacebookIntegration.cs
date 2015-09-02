using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using com.shephertz.app42.paas.sdk.csharp.user;

public class FacebookIntegration : Singleton<FacebookIntegration> 
{
	public Button FBLoginButton;
	public Image userProfileImage;
	public Text userName;

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
		FacebookManager.Instance.GetProfilePicture (OnProfilePictureCallback);
		FacebookManager.Instance.GraphAPICall("first_name,gender,email", OnGetUserInfoCallback);
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

		if (responseObject.ContainsKey ("email"))
			GetOrCreateUser (responseObject ["email"].ToString (), "111111111", responseObject ["email"].ToString ());
		else
			Debug.Log ("Email not found...");
	}

	#region GET_USER
	void GetOrCreateUser(string pUserName, string pPassword, string pEmail)
	{
		App42UserServices.Instance.RequestUser(pUserName, 
			(object pResponse) => LinkUserFacebookAccount (pUserName, FB.AccessToken), 
			(System.Exception pEx) => CreateUser (pUserName, pPassword, pEmail));
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

//			rankingText.text += user["name"] + " : " + entry["score"] + "\n";
		}
	}

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
