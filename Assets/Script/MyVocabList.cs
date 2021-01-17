using System;
using System.Collections.Generic;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;


public class MyVocabList : MonoBehaviour
{
    [SerializeField] public Transform content;
    [SerializeField] public MyVocabButton myVocabButton;

    public static Action showBackButtonCallBack = null;
    public static Action InitSelectVocabScrollListCallBack = null;
    public static Action<List<string>> InitSelectVocabBySortTypeCallBack = null;

    public static GameObject myVocabList;
    public static bool isListLoadingDone = false;

    private Vector3 startPos;
    private int selectedDay = 0;        // for sorting function

    void Start()
    {
        myVocabList = gameObject;
        startPos = content.localPosition;
        BackButtonController.resetVocabListScrollPos = ResetScrollPos;
        StatusBar.sortAlphabeticallyCallBack = SortAlphabetically;
        StatusBar.sortByTypeCallBack = SortByType;
    }
    public void OnView()
    {
        var bar = StatusBar.statusBar.GetComponent<StatusBar>();
        bar.sortButton.transform.GetChild(0).GetComponent<Text>().text = "Sort List  ";
        bar.sortButton.SetActive(true);

        showBackButtonCallBack();

        ResetScrollPos();

        // init selected vocab scroll list
        InitSelectVocabScrollListCallBack();
    }
    //public void LoadVocabRoutine(int d)
    //{
    //    selectedDay = d;
    //    OX_DataLoader.currentDay = d;

    //    isListLoadingDone = false;

    //    int i = 0;
    //    var list = UserDataManager.Instance.GetUserStudyVocabList();
    //    foreach (var s in list)
    //    {
    //        var vocab = s.Key;
    //        content.transform.GetChild(i).GetComponent<MyVocabButton>().SetVocabButton(vocab);
    //        content.transform.GetChild(i).gameObject.SetActive(true);
    //        i++;
    //    }

    //    isListLoadingDone = true;
    //}

    //public void ReloadVocabList()
    //{
    //    for (int j = 0; j < OX_DataLoader.eachDayVocabCount; j++)
    //    {
    //        content.transform.GetChild(j).GetComponent<MyVocabButton>().SetVocabButton("");
    //    }

    //    int i = 0;
    //    var list = UserDataManager.Instance.GetUserStudyVocabList();
    //    foreach (var s in list)
    //    {
    //        var vocab = s.Key;
    //        content.transform.GetChild(i).GetComponent<MyVocabButton>().SetVocabButton(vocab);
    //        content.transform.GetChild(i).gameObject.SetActive(true);
    //        i++;
    //    }
    //}
    public void ResetScrollPos()
    {
        content.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }
    // sort ALPHABET
    public void SortAlphabetically()
    {
        isListLoadingDone = false;

        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < eachDayVocabCount; i++)
        {
            content.transform.GetChild(i).GetComponent<MyVocabButton>().SetVocabButton("");
            content.transform.GetChild(i).gameObject.SetActive(false);
        }

        List<string> list = new List<string>();

        var mylist = UserDataManager.Instance.GetUserStudyVocabList();
        foreach (var s in mylist)
        {
            list.Add(s.Key);
        }

        list.Sort();

        for (int i = 0; i < list.Count; i++)
        {
            content.transform.GetChild(i).gameObject.SetActive(true);
            content.transform.GetChild(i).GetComponent<MyVocabButton>().SetVocabButton(list[i]);
        }

        InitSelectVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
    // Sort By Type
    public void SortByType(string type)
    {
        isListLoadingDone = false;
        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int j = 0; j < eachDayVocabCount; j++)
        {
            content.transform.GetChild(j).GetComponent<MyVocabButton>().SetVocabButton("");
        }

        List<string> list = new List<string>();

        int i = 0;
        var mylist = UserDataManager.Instance.GetUserStudyVocabList();
        foreach (var s in mylist)
        {
            var d = OX_DataLoader.GetVocab(s.Key);
            var psList = d.type.Split(new string[] { ".:" }, StringSplitOptions.None);
            foreach(var p in psList)
            {
                if (p.Equals(type))
                {
                    list.Add(s.Key);
                    break;
                }
            }

            i++;
        }
        list.Sort();

        for (int j = 0; j < list.Count; j++)
        {
            content.transform.GetChild(j).gameObject.SetActive(true);
            content.transform.GetChild(j).GetComponent<MyVocabButton>().SetVocabButton(list[j]);
        }

        InitSelectVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
}
