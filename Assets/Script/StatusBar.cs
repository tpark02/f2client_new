using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

enum Title
{
    HOME = 0,
    VOCAB_DAY = 1,
    VOCAB_DETAIL = 2,
    VOCAB_LIST = 3,
    VOCAB_TEST_DAY = 4,
    VOCAB_TEST = 5,
    VOCAB_TEST_RESULT = 6,
    VOCAB_TEST_RESULT_DETAIL = 7,
    MyNoteList = 8,
    MyVocabList = 9,
    MyVocabDetail = 10,
}

enum SortType
{
    ALPHABET = 0,
    NOUN = 1,
    VERB = 2,
    ADJ = 3,
    ADV = 4,
    CONJ = 5,
    PREP = 6
}
public class StatusBar : MonoBehaviour
{
    public static Action sortAlphabeticallyCallBack = null;
    public static Action<string> sortByTypeCallBack = null;

    public static GameObject statusBar = null;
    public static Stack<int> prevTitle = null;
    [SerializeField] public Text title;
    [SerializeField] public GameObject sortPanel;
    [SerializeField] public GameObject sortButton;
    [SerializeField] public GameObject selectVocabScroll;
    [SerializeField] public GameObject selectVocabButton;
    [SerializeField] public Transform selectedVocabScrollContent;
    [SerializeField] public GameObject addNewNoteButton;

    private Vector2 startPos = Vector2.zero;
    void Start()
    {
        prevTitle = new Stack<int>();
        statusBar = gameObject;
        sortPanel.SetActive(false);
        sortButton.SetActive(false);

        selectVocabScroll.SetActive(false);
        selectVocabButton.SetActive(false);

        ViewVocabList.InitSelectVocabScrollListCallBack = InitSelectVocabScrollList;
        ViewVocabList.InitSelectVocabBySortTypeCallBack = InitSelectVocabScrollBySortType;
        ViewVocabResultDetail.initVocabTestResultListCallBack = InitVocabTestResultList;

        MyVocabList.InitSelectVocabScrollListCallBack = InitSelectVocabScrollList;
        MyVocabList.InitSelectVocabBySortTypeCallBack = InitSelectVocabScrollBySortType;

        startPos = selectedVocabScrollContent.transform.localPosition;
    }
    public void SetTitle(int n)
    {
        switch (n)
        {
            case 0:
                title.text = "Home";
                break;
            case 1:
                title.text = "Vocab Day";
                break;
            case 2:
                title.text = "Vocab Detail";
                break;
            case 3:
                title.text = "Vocab List";
                break;
            case 4:
                title.text = "Test Day";
                break;
            case 5:
                title.text = "Test";
                break;
            case 6:
                title.text = "Test Result";
                break;
            case 7:
                title.text = "Test Result Detail";
                break;
            case 8:
                title.text = "My Notes";
                break;
            case 9:
                title.text = "My List";
                break;
            case 10:
                title.text = "My Vocab Detail";
                break;
            default:
                title.text = "error";
                break;
        }
    }

    public static void SetStatusTitle(int n)
    {
        statusBar.GetComponent<StatusBar>().SetTitle(n);
    }

    public void OnClickSortButton()
    {
        if (sortPanel.activeSelf)
        {
            sortPanel.SetActive(false);
        }
        else
        {
            sortPanel.SetActive(true);
        }
    }
    public void OnClickSelectListButton()
    {
        if (selectVocabScroll.activeSelf)
        {
            selectVocabScroll.SetActive(false);
        }
        else
        {
            selectVocabScroll.SetActive(true);
        }
    }
    public void OnClickSortSelect(int n)
    {
        UnityEngine.Debug.Log("<color=yellow>Sort Button Pressed!</color>");
        string sortButtonLabel = string.Empty;
        switch (n)
        {
            case 0:
                UnityEngine.Debug.Log("Alphabet");
                sortButtonLabel = "A-Z";
                sortAlphabeticallyCallBack();
                break;
            case 1:
                UnityEngine.Debug.Log("Noun");
                sortButtonLabel = "Noun";
                sortByTypeCallBack("n");
                break;
            case 2:
                UnityEngine.Debug.Log("Verb");
                sortButtonLabel = "Verb";
                sortByTypeCallBack("v");
                break;
            case 3:
                UnityEngine.Debug.Log("Adj");
                sortButtonLabel = "Adj";
                sortByTypeCallBack("adj");
                break;
            case 4:
                UnityEngine.Debug.Log("Adv");
                sortButtonLabel = "Adv";
                sortByTypeCallBack("adv");
                break;
            case 5:
                UnityEngine.Debug.Log("Conj");
                sortButtonLabel = "Conj";
                sortByTypeCallBack("conj");
                break;
            case 6:
                UnityEngine.Debug.Log("Prep");
                sortButtonLabel = "Prep";
                sortByTypeCallBack("prep");
                break;
            default:
                UnityEngine.Debug.Log("default");
                break;
        }
        sortButton.transform.GetChild(0).GetComponent<Text>().text = sortButtonLabel + "      ";
        sortPanel.SetActive(false);
    }
    public static void RecordPrevTitle(int n)
    {
        if (prevTitle.Count <= 0)
        {
            prevTitle.Push(n);
        }
        else if (prevTitle.Count > 0 && prevTitle.Peek() != n)
        {
            prevTitle.Push(n);
        }
    }

    public void InitVocabTestResultList()
    {
        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;
        for (int i = 0; i < eachDayVocabCount; i++)
        {
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().isTestResult = false;
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().SetSelectVocabButton("");
            selectedVocabScrollContent.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < OX_DataLoader.resultList.Count; i++)
        {
            string s = OX_DataLoader.resultList[i].vocab;
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().isTestResult = true;
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().SetSelectVocabButton(s);
            selectedVocabScrollContent.transform.GetChild(i).gameObject.SetActive(true);
        }

        selectedVocabScrollContent.transform.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }
    public void InitSelectVocabScrollList()
    {
        var d = OX_DataLoader.currentDay;
        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < eachDayVocabCount; i++)
        {
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().isTestResult = false;
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().SetSelectVocabButton("");
            selectedVocabScrollContent.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < OX_DataLoader.eachDayVocabCount; i++)
        {
            var s = OX_DataLoader.GetVocabList(i);
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().SetSelectVocabButton(s.vocab);
            selectedVocabScrollContent.transform.GetChild(i).gameObject.SetActive(true);
        }
        selectedVocabScrollContent.transform.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }

    public void InitSelectVocabScrollBySortType(List<string> l)
    {
        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < eachDayVocabCount; i++)
        {
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().isTestResult = false;
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().SetSelectVocabButton("");
            selectedVocabScrollContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < l.Count; i++)
        {
            selectedVocabScrollContent.transform.GetChild(i).gameObject.SetActive(true);
            selectedVocabScrollContent.transform.GetChild(i).GetComponent<SelectVocabButton>().SetSelectVocabButton(l[i]);
        }
        selectedVocabScrollContent.transform.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }

    public void ResetSelectedVocabScrollPos()
    {
        selectedVocabScrollContent.transform.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }

    public void OnClickAddNewNote()
    {
        var p = UIPopupManager.GetPopup("NewNotePopup");
        //make sure that a popup clone was actually created
        if (p == null)
            return;

        //show the popup
        UIPopupManager.ShowPopup(p, p.AddToPopupQueue, false);
    }
}
