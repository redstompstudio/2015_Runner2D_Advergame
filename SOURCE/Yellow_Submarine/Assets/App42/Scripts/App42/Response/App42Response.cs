using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;

public class App42Response : App42CallBack
{
	public delegate void OnSuccessDelegate(object pResponse);
	public OnSuccessDelegate onSuccessCallback;

	public delegate void OnExceptionDelegate(System.Exception pEx);
	public OnExceptionDelegate onExceptionCallback;

	public App42Response(OnSuccessDelegate pSuccessCallback, OnExceptionDelegate pExceptionCallback)
	{
		onSuccessCallback += pSuccessCallback;
		onExceptionCallback += pExceptionCallback;
	}

	#region App42CallBack implementation
	/// <summary>
	/// Return the request result object.
	/// Can be the following types:
	/// - User
	/// - IList<User>
	/// 
	/// If there is something wrong, throws an exception.
	/// </summary>
	/// <param name="response">Response.</param>
	public void OnSuccess (object pResponse)
	{
		try
		{
#if UNITY_EDITOR 
			HandleResultExample(pResponse);
#endif
			if(onSuccessCallback != null)
				onSuccessCallback (pResponse);

			onSuccessCallback = null;
		}
		catch (App42Exception e)
		{
			Debug.LogError ("App42Exception : "+ e);
		}
	}

	public void OnException (System.Exception ex)
	{
		Debug.LogError (ex.Message);

		if(onExceptionCallback != null)
			onExceptionCallback (ex);

		onExceptionCallback = null;
	}
	#endregion

	void HandleResultExample(object pResult)
	{
		try
		{
			User user = null;
			IList<User> usersList = null;

			if (pResult is User)
				user = (User)pResult;
			else
				usersList = (IList<User>)user;
		}
		catch (App42Exception e)
		{
			Debug.LogError ("App42Exception : "+ e);
		}
	}
}
