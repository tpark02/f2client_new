using System;
using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class BackEndFederationAuth : MonoBehaviour
{
    public static Action loginPanelCallBack = null;
    public static Action signUpPanelCallBack = null;
    public static Action<string> showPopup = null;
    public static Action<string> setSignEmailCallBack = null;
    private string facebookToken;
    string facebookEmail = string.Empty;
    string facebookName = string.Empty;
    [SerializeField] private FederationType federationType;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // 페이스북 SDK 초기화
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // 이미 초기화 되어 있음
            FB.ActivateApp();
        }
    }
    // custome GPGS set and activate
    void Start()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .AddOauthScope("email")
            .AddOauthScope("https://www.googleapis.com/auth/games")
            .AddOauthScope("https://www.googleapis.com/auth/plus.login")
            .RequestServerAuthCode(false)
            .RequestIdToken()
            .RequestEmail()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // 앱을 활성화 시킨다.
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Facebook SDK 초기화 실패");
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // 앱을 일시 정지 시킨다.
            Time.timeScale = 0;
        }
        else
        {
            // 앱을 다시 플레이 시킨다.
            Time.timeScale = 1;
        }
    }
    public void FBLogin()
    {
        // 읽어올 권한을 설정
        // 뒤끝 콘솔 > 유저관리에서 회원의 이메일 정보를 알 수 있습니다.
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    private void AuthCallback(ILoginResult result)
    {
        // 페이스북에 로그인 성공
        if (FB.IsLoggedIn)
        {
            // 토큰 정보 참조
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            // 토큰을 string 으로 변환
            facebookToken = aToken.TokenString;

            // 뒤끝 서버에 획득한 페이스북 토큰으로 가입요청
            // 동기 방법으로 가입 요청
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(facebookToken, FederationType.Facebook, "페이스북 로그인");

            if (BRO.IsSuccess())
            {
                Debug.Log("페북 토큰으로 뒤끝서버 로그인 성공 - 동기 방식-");
                
                FB.API("/me?fields=name,email", HttpMethod.GET, DialogUserNameCallBack);
            }
            else
            {
                switch (BRO.GetStatusCode())
                {
                    case "200":
                        Debug.Log("이미 회원가입된 회원");
                        showPopup("가입 되어있는 이메일 입니다.");
                        break;

                    case "403":
                        Debug.Log("차단된 사용자 입니다. 차단 사유 : " + BRO.GetErrorCode());
                        break;

                    default:
                        Debug.Log("서버 공통 에러 발생" + BRO.GetMessage());
                        break;
                }
            }

        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    void DialogUserNameCallBack(IResult result)
    {
        
        if (result != null)
        {
            facebookName = result.ResultDictionary["name"].ToString();
            facebookEmail = result.ResultDictionary["email"].ToString();

            Debug.Log("Facebook name and email : " + facebookName + " : " + facebookEmail);

            setSignEmailCallBack(facebookEmail);
            signUpPanelCallBack();
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    // 구글에 로그인하기
    private void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    Debug.Log("구글 로그인 실패");
                    return;
                }

                // 로그인이 성공되었습니다.
                Debug.Log("GetIdToken - " + PlayGamesPlatform.Instance.GetIdToken());
                Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("GoogleId - " + Social.localUser.id);
                Debug.Log("UserName - " + Social.localUser.userName);
                Debug.Log("UserName - " + PlayGamesPlatform.Instance.GetUserDisplayName());
            });
        }
    }
    // 구글 토큰 받아오기
    private string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. 잠시 후 다시 시도하세요.");
            GoogleAuth();
            return string.Empty;
        }
    }
    // 구글토큰으로 뒤끝서버 로그인하기 - 동기 방식
    public void OnClickGPGSLogin()
    {
        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs로 만든계정");
        if (BRO.IsSuccess())
        {
            Debug.Log("구글 토큰으로 뒤끝서버 로그인 성공 - 동기 방식-");
            signUpPanelCallBack();
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "200":
                    Debug.Log("이미 회원가입된 회원");
                    showPopup("가입 되어있는 이메일 입니다.");
                    break;

                case "403":
                    Debug.Log("차단된 사용자 입니다. 차단 사유 : " + BRO.GetErrorCode());
                    break;
            }
        }
    }
    public void OnLogOut()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
    }
}