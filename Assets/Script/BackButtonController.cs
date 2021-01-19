using System;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour
{
    public static int selectedVocabDay = -1;
    public static Action resetVocabListScrollPos = null;
    public static Action resetVocabDayScrollPos = null;
    [SerializeField] private GameObject homeButton = null;
    void Start()
    {
        gameObject.SetActive(false);

        ViewVocabList.showBackButtonCallBack = ShowBackButton;
        ViewVocabDay.showBackButtonCallBack = ShowBackButton;
        ViewVocabTestDay.showBackButtonCallBack = ShowBackButton;
        ViewVocabDetail.showBackButtonCallBack = ShowBackButton;
        ViewVocabTest.showBackButtonCallBack = ShowBackButton;
        ViewVocabTestDay.showBackButtonCallBack = ShowBackButton;
        ViewVocabTestResult.showHomeButtonCallBack = ShowHomeButton;
        ViewVocabResultDetail.showBackButtonCallBack = ShowBackButton;
        DrawerLeft.hideBackButtonCallBack = HideBackButton;

        MyNoteList.showBackButtonCallBack = ShowBackButton;
        MyVocabList.showBackButtonCallBack = ShowBackButton;
        MyVocabDetail.showBackButtonCallBack = ShowBackButton;
    }
    public void OnClickBackButton()
    {
        int prevPage = StatusBar.prevTitle.Peek();
        if (prevPage == (int)Title.HOME)
        {
            HideBackButton();
        }
        StatusBar.SetStatusTitle(prevPage);
        StatusBar.prevTitle.Pop();
        
        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);

        if (prevPage == (int) Title.VOCAB_LIST
        || prevPage == (int) Title.MyVocabList)      // sort button active or not
        {
            var bar = StatusBar.statusBar.GetComponent<StatusBar>();
            //bar.sortButton.transform.GetChild(0).GetComponent<Text>().text = "Sort List ";
            bar.sortButton.SetActive(true);
        }
        else
        {
            StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        }

        if (prevPage == (int) Title.VOCAB_DETAIL)
        {
            StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(true);
        }
        else
        {
            StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        }

        if (prevPage == (int) Title.MyNoteList)
        {
            StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(true);
        }
        else
        {
            StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);
        }

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        //NetWorkManager.Instance.GetMyVocabList("tpark3546@gmail.com");

        if (prevPage == (int) Title.VOCAB_LIST)
        {
            ViewVocabList.viewVocabList.GetComponent<ViewVocabList>().LoadVocabRoutine(selectedVocabDay);
            var bar = StatusBar.statusBar.GetComponent<StatusBar>();
            bar.sortButton.transform.GetChild(0).GetComponent<Text>().text = "Sort List  ";
        }

        if (prevPage == (int) Title.MyVocabList)
        {
            MyVocabList.myVocabList.GetComponent<MyVocabList>().LoadVocabRoutine(OX_DataLoader.currentNoteName);
            var bar = StatusBar.statusBar.GetComponent<StatusBar>();
            bar.sortButton.transform.GetChild(0).GetComponent<Text>().text = "Sort List  ";
        }
    }

    public void ShowHomeButton()
    {
        gameObject.SetActive(false);
        homeButton.SetActive(true);
    }
    public void ShowBackButton()
    {
        gameObject.SetActive(true);
        homeButton.SetActive(false);
    }

    public void HideBackButton()
    {
        gameObject.SetActive(false);
        homeButton.SetActive(false);
    }
}
