using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp.user;

public class UserServices_GetUserProfile : MonoBehaviour 
{
	public Button searchButton;

	public InputField searchField;

	public Text userName;
	public Text userEmail;

	void Awake()
	{
		if (searchButton != null)
			searchButton.onClick.AddListener (OnSearchButton);
	}

	void OnSearchButton()
	{
		App42UserServices.Instance.RequestUser(searchField.text, OnRequestUserSuccess, OnRequestUserFailed);
	}

	void OnRequestUserSuccess(object pResponse)
	{
		if(pResponse != null)
		{
			User user = (User)pResponse;

			userName.text = user.GetUserName ();
			userEmail.text = user.GetEmail ();
		}
	}

	void OnRequestUserFailed(System.Exception pEx)
	{
		
	}

}
