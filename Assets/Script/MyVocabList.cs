using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;


public class MyVocabList : MonoBehaviour
{
    [SerializeField] public Transform content;
    [SerializeField] public MyVocabButton myVocabButton;

    public static Action showBackButtonCallBack = null;
    public static Action InitSelectVocabScrollListCallBack = null;
    //public static Action<List<string>> InitSelectVocabBySortTypeCallBack = null;
    public static Action<Dictionary<string, OX_DataLoader.VocabData>> InitSelectMyVocabBySortTypeCallBack = null;
    public static GameObject myVocabList;
    public static bool isListLoadingDone = false;

    private Vector3 startPos;
    //private int selectedDay = 0;        // for sorting function
    private string selectedNoteName = string.Empty;

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

        StatusBar.sortAlphabeticallyCallBack = SortAlphabetically;
        StatusBar.sortByTypeCallBack = SortByType;
        // init selected vocab scroll list
        InitSelectVocabScrollListCallBack();
    }

    public void LoadVocabRoutine(string noteName)
    {
        selectedNoteName = noteName;
        OX_DataLoader.currentNoteName = noteName;

        isListLoadingDone = false;

        var list = UserDataManager.Instance.GetUserStudyVocabList();
        for (int j = 0; j < OX_DataLoader.eachDayVocabCount; j++)
        {
            var c = content.transform.GetChild(j).GetComponent<MyVocabButton>();
            {
                c.gameObject.SetActive(false);
                c.favoriteToggle.SetCheck(false);
            }
        }

        int i = 0;
        foreach (var s in list)
        {
            if (s.Value.Equals(noteName) == false)
            {
                continue;
            }
            
            OX_DataLoader.VocabData data = OX_DataLoader.GetVocabDataById(s.Key);

            var c = content.transform.GetChild(i).GetComponent<MyVocabButton>();
            {
                c.favoriteToggle.vocabData = data;
                c.SetVocabButton(data.vocab, data);
                c.gameObject.SetActive(true);
                c.favoriteToggle.SetCheck(true);
            }
            i++;
        }
        InitSelectVocabScrollListCallBack();
        isListLoadingDone = true;
    }

    public void ResetScrollPos()
    {
        content.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }
    // sort ALPHABET
    public void SortAlphabetically()
    {
        isListLoadingDone = false;

        var rowCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < rowCount; i++)
        {
            var c = content.transform.GetChild(i);
            c.gameObject.SetActive(false);
        }

        Dictionary<string, OX_DataLoader.VocabData> list = new Dictionary<string, OX_DataLoader.VocabData>();

        var mylist = UserDataManager.Instance.GetCurrentNoteVocabList();
        
        foreach (var s in mylist)
        {
            var data = OX_DataLoader.GetVocabDataById(s.Key);
            list.Add(data.vocab, data);
        }

        var l = list.Keys.ToList();
        l.Sort();

        for (int j = 0; j < l.Count; j++)
        {
            var vocabData = list[l[j]];
            var c = content.transform.GetChild(j);
            c.gameObject.SetActive(true);
            c.GetComponent<MyVocabButton>().SetVocabButton(l[j], vocabData);
            c.GetComponent<MyVocabButton>().favoriteToggle.SetCheck(true);
            c.GetComponent<MyVocabButton>().favoriteToggle.vocabData = vocabData;
        }

        InitSelectMyVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
    // Sort By Type
    public void SortByType(string type)
    {
        isListLoadingDone = false;
        var rowCount = OX_DataLoader.eachDayVocabCount;

        for (int j = 0; j < rowCount; j++)
        {
            var c = content.transform.GetChild(j).GetComponent<MyVocabButton>();
            c.gameObject.SetActive(false);
        }

        Dictionary<string, OX_DataLoader.VocabData> list = new Dictionary<string, OX_DataLoader.VocabData>();

        int i = 0;
        var mylist = UserDataManager.Instance.GetCurrentNoteVocabList();
        
        foreach (var s in mylist)
        {
            var data = OX_DataLoader.GetVocabDataById(s.Key);
            var psList = data.type.Split(new string[] { ".:" }, StringSplitOptions.None);
            foreach(var p in psList)
            {
                if (p.Equals(type))
                {
                    list.Add(data.vocab, data);
                    break;
                }
            }
            i++;
        }

        var l = list.Keys.ToList();
        l.Sort();
       
        for (int j = 0; j < l.Count; j++)
        {
            var vocabData = list[l[j]];
            var c = content.transform.GetChild(j);
            {
                c.gameObject.SetActive(true);
                c.GetComponent<MyVocabButton>().SetVocabButton(l[j], vocabData);
                c.GetComponent<MyVocabButton>().favoriteToggle.SetCheck(true);
                c.GetComponent<MyVocabButton>().favoriteToggle.vocabData = vocabData;
            }
        }

        InitSelectMyVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
}
