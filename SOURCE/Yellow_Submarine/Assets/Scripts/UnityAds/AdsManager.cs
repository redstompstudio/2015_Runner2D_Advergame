using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdsManager : Singleton<AdsManager>
{
    public delegate void OnAdSkippedDelegate();
    private OnAdSkippedDelegate onAdSkippedCallback;

    public delegate void OnAdFinishedDelegate();
    public OnAdFinishedDelegate onAdFinishedCallback;

    public delegate void OnAdFailedDelegate();
    public OnAdFailedDelegate onAdFailedCallback;
    
    public bool IsShowing()
    {
        return Advertisement.isShowing;
    }

    public void ShowAd(string pZone = "")
    {
        if (Advertisement.isShowing)
            Debug.LogError("Already showing an Ad!");
        else
        {
            if (string.IsNullOrEmpty(pZone))
                pZone = null;

            if (Advertisement.IsReady(pZone))
            {
                ShowOptions showOptions = new ShowOptions();
                showOptions.resultCallback += OnAdCallback;

                Advertisement.Show(pZone, showOptions);
            }
            else
                Debug.LogError("Advertisement is not ready!");
        }
    }

    void OnAdCallback(ShowResult pResult)
    {
        switch (pResult)
        {
            case ShowResult.Finished:
                Debug.Log("Ad Finished!");

                if (onAdFinishedCallback != null)
                    onAdFinishedCallback();
                break;

            case ShowResult.Skipped:
                Debug.Log("Ad Skipped!");

                if (onAdSkippedCallback != null)
                    onAdSkippedCallback();
                break;

            case ShowResult.Failed:
                Debug.LogError("Ad Failed!");

                if (onAdFailedCallback != null)
                    onAdFailedCallback();
                break;
        }
    }
}
