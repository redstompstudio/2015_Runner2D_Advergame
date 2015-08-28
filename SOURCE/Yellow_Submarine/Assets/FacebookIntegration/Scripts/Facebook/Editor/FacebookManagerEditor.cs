using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(FacebookManager))]
public class FacebookManagerEditor : Editor
{
	private FacebookManager facebookManager;

	void OnEnable()
	{
		facebookManager = (FacebookManager)target;
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		base.OnInspectorGUI ();
	}

	void Permissions()
	{
		
	}
}
