using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using Doozy.Engine.UI;
using UnityEngine.SceneManagement;

public class BackEndAuthentication : MonoBehaviour
{
    delegate bool D(string s);
    [SerializeField] public InputField emailInput;
    [SerializeField] public InputField passwordInput;
    [SerializeField] public InputField SignUpEmailInput;
    [SerializeField] public InputField SignUpPasswordInput;
    [SerializeField] public InputField confirmPasswordInput;
    [SerializeField] public GameObject loadingView;
    public SceneLoader loader;
    private D emailCheck, passwordCheck;
    private bool isOXDataLoadingDone = false;
    void Start()
    {
        ViewLogin.clearSignUpLoginCallBack = ClearSingupAndLogin;
        BackEndFederationAuth.setSignEmailCallBack = SetSignEmail;
        BackEndFederationAuth.showPopup = ShowNoticePopup;
    }
    // 회원가입1 - 동기 방식
    public void OnClickSignUp()
    {
        if (EmailCheck(SignUpEmailInput.text) == false)
        {
            return;
        }

        if (PasswordCheck(SignUpPasswordInput.text) == false)
        {
            return;
        }

        if (PasswordCheck(confirmPasswordInput.text, true) == false)
        {
            return;
        }
        if (SignUpPasswordInput.text.Equals(confirmPasswordInput.text) == false)
        {
            ShowNoticePopup("비밀번호가 일치하지 않습니다.");
            return;
        }
        // 회원 가입을 한뒤 결과를 BackEndReturnObject 타입으로 반환한다.
        string error = Backend.BMember.CustomSignUp(SignUpEmailInput.text, SignUpPasswordInput.text, "Test1").GetErrorCode();

        // 회원 가입 실패 처리
        switch (error)
        {
            case "DuplicatedParameterException":
                Debug.Log("중복된 customId 가 존재하는 경우");
                ShowNoticePopup("가입 되어있는 이메일 입니다.");
                break;
            default:
                Debug.Log("회원 가입 완료");
                ShowNoticePopup("가입이 완료되었습니다.\n 로그인 해주시기 바랍니다.");
                BackEndFederationAuth.loginPanelCallBack();
                break;
        }

        Debug.Log("동기 방식============================================= ");

    }
    public void OnClickLogin1()
    {
        string error = Backend.BMember.CustomLogin(emailInput.text, passwordInput.text).GetErrorCode();

        // 로그인 실패 처리
        switch (error)
        {
            // 아이디 또는 비밀번호가 틀렸을 경우
            case "BadUnauthorizedException":
                Debug.Log("아이디 또는 비밀번호가 틀렸다.");
                ShowNoticePopup("이메일 또는 비밀번호가 맞지 않습니다.");
                break;
            case "BadPlayer":  //  이 경우 콘솔에서 입력한 차단된 사유가 에러코드가 된다.
                Debug.Log("차단된 유저");
                break;
            case "UndefinedParameterException":
                Debug.Log("가입 되지 않는 유저");
                ShowNoticePopup("가입 되어있지 않은 이메일 입니다.");
                break;
            default:
                Debug.Log("로그인 완료");
                //loader.LoadSceneAsyncSingle(1);
                //loader.SetAllowSceneActivation(true);
                loadingView.SetActive(true);
                StartCoroutine(LoadMainScene());
                break;
        }
        Debug.Log("동기 방식============================================= ");
    }

    IEnumerator LoadMainScene()
    {
        loader.SetAllowSceneActivation(false);
        loader.LoadSceneAsyncSingle(1);
        OX_DataLoader.InitOriginalData();

        while (loader.CurrentAsyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        loader.SetAllowSceneActivation(true);
    }

    public bool EmailCheck(string value)
    {
        emailCheck = (str) =>
        {
            if (!Regex.IsMatch(str,
                @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?"))
            {
                return false;
            }

            return true;
        };

        bool isCorrect = emailCheck(value);
        if (isCorrect == false)
        {
            ShowNoticePopup("이메일을 입력해 주세요.");
            return false;
        }

        return true;
    }
    public bool PasswordCheck(string value, bool isConfirm = false)
    {
        //최소 비밀번호 길이: 8
        //최소 소문자 수: 1
        //대문자의 최소 수: 1
        //최소 숫자 수: 1
        passwordCheck = (str) =>
        {
            Regex rxPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,}$",
                RegexOptions.IgnorePatternWhitespace);
            if (rxPassword.IsMatch(str) == false)
            {
                return false;
            }
            return true;
        };

        bool isCorrect = passwordCheck(value);
        if (isCorrect == false)
        {
            if (isConfirm)
            {
                ShowNoticePopup("Confirm Password도\n 영문자로 길이 최소 8, 대문자 1개, 숫자\n 조합을 입력해야 합니다.");
                return false;
            }
            ShowNoticePopup("영문자로 길이 최소 8, 대문자 1개, 숫자\n 조합을 입력해야 합니다.");
            return false;
        }

        return true;
    }

    public void ShowNoticePopup(string str)
    {
        //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
        var p = UIPopupManager.GetPopup("PasswordNotice");
        
        //make sure that a popup clone was actually created
        if (p == null)
        {
            return;
        }

        p.Data.SetLabelsTexts("", str);
        p.Data.SetButtonsCallbacks(ClearPopupQueue);
        UIPopupManager.ShowPopup(p, p.AddToPopupQueue, false, "Popup");
    }
    public void ClearPopupQueue()
    {
        UIPopupManager.ClearQueue();
    }

    public void ClearSingupAndLogin()
    {
        emailInput.text = string.Empty;
        passwordInput.text = string.Empty;
        SignUpEmailInput.text = string.Empty;
        SignUpPasswordInput.text = string.Empty;
        confirmPasswordInput.text = string.Empty;
    }

    public void SetSignEmail(string s)
    {
        SignUpEmailInput.text = s;
        SignUpPasswordInput.text = string.Empty;
        confirmPasswordInput.text = string.Empty;
    }
}