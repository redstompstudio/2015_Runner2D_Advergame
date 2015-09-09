using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using com.shephertz.app42.paas.sdk.csharp.user;

public class UserServices_CreateUser : MonoBehaviour 
{
	public InputField userField;
	public InputField passwordField;
	public InputField emailField;

	public Button createButton;

	void Awake()
	{
		createButton.onClick.AddListener (OnCreateButton);
	}

	void OnCreateButton()
	{
		App42UserServices.Instance.CreateUser (userField.text, passwordField.text, 
			emailField.text, OnCreateUserSuccess, OnCreateUserException);
	}

	void OnCreateUserSuccess(object pResponse)
	{
		User user = (User)pResponse;

		Debug.Log (user.GetUserName());
		Debug.Log (user.GetEmail());
	}

	void OnCreateUserException(System.Exception pEx)
	{
		Debug.Log ("Something is wrong!");
	}
}
