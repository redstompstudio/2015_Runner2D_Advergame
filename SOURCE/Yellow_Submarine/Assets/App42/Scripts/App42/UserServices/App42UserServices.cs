using UnityEngine;
using System.Collections;

using AssemblyCSharp;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.social;

using System;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;


public class App42UserServices : Singleton<App42UserServices> 
{
	private ServiceAPI serviceAPI;
	private UserService userService;
	private SocialService socialService;

	private UserResponse callBack = new UserResponse ();

	void Awake()
	{
		serviceAPI = new ServiceAPI (App42Manager.Instance.API_KEY, App42Manager.Instance.SECRET_KEY);
		Initialize ();
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

	public void CreateUserWithProfile(string pName, string pPassword, string pEmail, User.Profile pProfile,
		App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response(pSuccess, pException);
		userService.CreateUserWithProfile (pName, pPassword, pEmail, pProfile, response);
	}

	public void LinkFacebookAccount(string pUserName, string pAccessToken,
		App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException)
	{
		App42Response response = new App42Response(pSuccess, pException);
		socialService.LinkUserFacebookAccount (pUserName, pAccessToken, response);
	}
		
	public void RequestUser(string pUserName, App42Response.OnSuccessDelegate pOnSuccess,
		App42Response.OnExceptionDelegate pOnException)
	{
		App42Response response = new App42Response(pOnSuccess, pOnException);
		userService.GetUser (pUserName, response);
	}

	public void RequestUsersByGroup(IList<string> pUsersList, App42Response.OnSuccessDelegate pOnSuccess,
		App42Response.OnExceptionDelegate pOnException)
	{
		App42Response response = new App42Response (pOnSuccess, pOnException);
		userService.GetUsersByGroup (pUsersList, response);
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

