using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp.game;
using System.Collections.Generic;

public class LeaderBoardServices_UI : MonoBehaviour 
{
	[Header("POST SCORES")]
	public Button postScoreButton;
	public InputField leaderBoardNameField;
	public InputField userNameField;
	public InputField userScoreField;

	[Header("TOP N")]
	public Button getTop3Button;
	public InputField topNLeaderBoardNameField;
	public InputField topNField;
	public Text topNresult;

	void Awake()
	{
		if (postScoreButton != null)
			postScoreButton.onClick.AddListener (OnPostScoreButton);

		if (getTop3Button != null)
			getTop3Button.onClick.AddListener (OnGetTopNButton);
	}

#region POST_SCORE
	void OnPostScoreButton()
	{
		App42LeaderBoardServices.Instance.SaveUserScore (leaderBoardNameField.text, userNameField.text,
			double.Parse(userScoreField.text), OnPostScoreSuccess, OnPostScoreException);
	}

	void OnPostScoreSuccess(object pResponse)
	{
		Game board = (Game)pResponse;

		Debug.Log ("Board Name: " + board.GetName ());
		Debug.Log ("Board Description: " + board.GetDescription());

		IList<Game.Score> scores = board.GetScoreList ();
		for(int i = 0; i < scores.Count; i++)
		{
			Debug.Log("User Name: " + scores [i].GetUserName ());
			Debug.Log("Score Value: " + scores [i].GetValue().ToString());
//			Debug.Log("Score ID: " + scores [i].GetScoreId ());
//			Debug.Log("Rank: " + scores [i].GetRank());
		}
	}

	void OnPostScoreException(System.Exception pEx)
	{
		
	}
#endregion

#region TOP X ON RANKING
	void OnGetTopNButton()
	{
		App42LeaderBoardServices.Instance.GetTopNRankers (topNLeaderBoardNameField.text, int.Parse(topNField.text), 
			OnGetTop3Success, OnGetTop3Exception);
	}

	void OnGetTop3Success(object pResponse)
	{
		if (pResponse is Game) {
			Game game = (Game)pResponse;

			topNresult.text = "";

			if (game.GetScoreList () != null) 
			{
				IList<Game.Score> scoreList = game.GetScoreList ();
				for (int i = 0; i < scoreList.Count; i++) 
				{
//					Debug.Log (string.Format ("#{0} - {1} : {2}", i + 1, scoreList [i].GetUserName (), scoreList [i].GetValue ()));
					topNresult.text += string.Format ("#{0} - {1} : {2} \n", i + 1, scoreList [i].GetUserName (), scoreList [i].GetValue ());
				}
			}
		}
		else
		{
			Debug.Log ("Is Not a game...");
		}
	}

	void OnGetTop3Exception(System.Exception pEx)
	{
	}
#endregion
}
