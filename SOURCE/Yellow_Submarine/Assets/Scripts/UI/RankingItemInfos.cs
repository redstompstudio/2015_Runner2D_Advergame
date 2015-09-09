using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankingItemInfos : MonoBehaviour
{
    public Image userImage;
    public Text userName;
    public Text userScore;

    public void SetUserInfos(Image pImage, string pUserName, string pUserScore)
    {
        userImage = pImage;
        userName.text = pUserName;
        userScore.text = pUserScore;
    }
}
