using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
    public Text text;

    void Awake()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestServerAuthCode(false)
            .RequestEmail()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();

        text.text = "no Login";
#endif
    }
    public void GoogleLogin()
    {
#if UNITY_ANDROID
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    Debug.Log("GetServerAuthCode - " + PlayGamesPlatform.Instance.GetServerAuthCode());
                    Debug.Log("GetIdToken - " + PlayGamesPlatform.Instance.GetIdToken());
                    Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                    Debug.Log("GoogleId - " + Social.localUser.id);
                    Debug.Log("UserName - " + Social.localUser.userName);
                    Debug.Log("UserName - " + PlayGamesPlatform.Instance.GetUserDisplayName());
                    text.text = Social.localUser.userName + " : " + ((PlayGamesLocalUser)Social.localUser).Email;
                }
                else
                {
                    Debug.Log("Fall");
                    text.text = "Fail";
                }
            });
        }
#endif
    }

    public void OnLogOut()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
        text.text = "Logout";
#endif
    }
}
