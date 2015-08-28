using UnityEngine;
using System.Collections;

using AssemblyCSharp;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.social;

using System;
using System.Runtime.Remoting.Messaging;


public class App42UserServices : MonoBehaviour 
{
	private static App42UserServices instance;

	private ServiceAPI serviceAPI;
	private UserService userService;
	private SocialService socialService;

	private UserResponse callBack = new UserResponse ();

	public static App42UserServices Instance {
		get {
			if (instance == null)
				instance = FindObjectOfType (typeof(App42UserServices)) as App42UserServices;

			//if (instance.userService == null || instance.socialService == null)
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
		if(userService == null)
			userService = serviceAPI.BuildUserService ();

		if(socialService == null)
			socialService = serviceAPI.BuildSocialService ();
	}

	public void CreateUser(string pName, string pPassword, string pEmail, 
		App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response(pSuccess, pException);
		userService.CreateUser (pName, pPassword, pEmail, response);
	}

	public void LinkFacebookAccount(string pUserName, string pAccessToken,
		App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response(pSuccess, pException);
		socialService.LinkUserFacebookAccount (pUserName, pAccessToken, response);
	}

	public void CreateUserWithProfile(string pName, string pPassword, string pEmail, User.Profile pProfile)
	{
		userService.CreateUserWithProfile (pName, pPassword, pEmail, pProfile, callBack);
	}

	public void RequestUser(string pUserName, App42Response.OnSuccessDelegate pOnSuccess,
		App42Response.OnExceptionDelegate pOnException)
	{
		App42Response response = new App42Response(pOnSuccess, pOnException);
		userService.GetUser (pUserName, response);
	}

	public void RequestUserByEmail(string pEmail)
	{
		userService.GetUserByEmailId (pEmail, callBack);	
	}

	public void CreateOrUpdateUserProfile(User pUser)
	{
		userService.CreateOrUpdateProfile (pUser, callBack);
	}
}

