using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class ViewLogin : MonoBehaviour
{
    public static Action clearSignUpLoginCallBack = null;
    [SerializeField] public GameObject loginPanel;
    [SerializeField] public GameObject signUpPanel;

    void Start()
    {
        loginPanel.GetComponent<CanvasGroup>().alpha = 1f;
        signUpPanel.GetComponent<CanvasGroup>().alpha = 0f;

        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);

        BackEndFederationAuth.loginPanelCallBack = ShowLoginPanel;
        BackEndFederationAuth.signUpPanelCallBack = ShowSignUpPanel;
    }
    public void OnClickGoBackButton()
    {
        clearSignUpLoginCallBack();
        ShowLoginPanel();
    }

    public void OnClickSignUpButton()
    {
        clearSignUpLoginCallBack();
        ShowSignUpPanel();
    }

    private void ShowLoginPanel()
    {
        StartCoroutine(ShowLoginAnim());
    }

    private void ShowSignUpPanel()
    {
        StartCoroutine(ShowSignUpAnim());
    }

    private IEnumerator ShowSignUpAnim()
    {
        loginPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        loginPanel.SetActive(false);
        signUpPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        signUpPanel.SetActive(true);
    }

    private IEnumerator ShowLoginAnim()
    {
        signUpPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        signUpPanel.SetActive(false);
        loginPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        loginPanel.SetActive(true);
    }
}
