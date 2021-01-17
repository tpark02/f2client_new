using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;


public class FacebookManager : MonoBehaviour
{
    public GameObject facebooklogin;
    public GameObject debugtext;
    public GameObject DialogPrifilePic;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void SetInit()
    {
        Debug.Log("FB init donw");

        if (FB.IsLoggedIn)
        {
            Debug.Log("Fb Logged In");
        }
        else
        {
            Debug.Log("Fb Not Logged In");
        }

        //DealWidthFBMenu (FB.IsLoggedIn);
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
        if (isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FacebookLogin()
    {

        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("email");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Fb Logged In AuthCallBack");

            }
            else
            {
                Debug.Log("Fb Not Logged In AuthCallBack");
            }

            DealWidthFBMenu(FB.IsLoggedIn);
        }
    }



    void DealWidthFBMenu(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            facebooklogin.SetActive(true);

            FB.API("/me?fields=name,email", HttpMethod.GET, DialogUserNameCallBack);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DialogUserProfileImgCallBack);
        }
    }


    void DialogUserNameCallBack(IResult result)
    {
        Text UsetName = debugtext.GetComponent<Text>();

        if (result.Error == null)
        {
            UsetName.text = "Hi there, " + result.ResultDictionary["name"] + " : \n" + result.ResultDictionary["email"];
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DialogUserProfileImgCallBack(IGraphResult result)
    {


        if (result.Texture != null)
        {
            Image ProfilePic = DialogPrifilePic.GetComponent<Image>();
            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
        else
        {
            Debug.Log(result.Error);
        }
    }





}
