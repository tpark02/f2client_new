using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewVocabList : MonoBehaviour
{
    [SerializeField] public Transform content;
    
    public static Action showBackButtonCallBack = null;
    public static Action InitSelectVocabScrollListCallBack = null;
    public static Action<List<string>> InitSelectVocabBySortTypeCallBack = null;

    public static GameObject viewVocabList;
    public static bool isListLoadingDone = false;
    
    private Vector3 startPos;
    private int selectedDay = 0;        // for sorting function

    void Start()
    {
        viewVocabList = gameObject;
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

        foreach (Transform c in content.transform)
        {
            //c.GetComponent<VocabButton>().favoriteToggle = false;
        }

        StatusBar.sortAlphabeticallyCallBack = SortAlphabetically;
        StatusBar.sortByTypeCallBack = SortByType;
        // init selected vocab scroll list
        InitSelectVocabScrollListCallBack();
    }
    public void LoadVocabRoutine(int d)
    {
        selectedDay = d;
        OX_DataLoader.currentDay = d;

        isListLoadingDone = false;
        
        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < OX_DataLoader.eachDayVocabCount; i++)
        {
            var s = OX_DataLoader.GetVocabList(i);
            var button = content.transform.GetChild(i).GetComponent<VocabButton>();

            button.favoriteToggle.SetCheck(false);
            button.favoriteToggle.vocabData = s;

            button.SetVocabButton(s.vocab);
            content.transform.GetChild(i).gameObject.SetActive(true);

            foreach (var data in UserDataManager.Instance.GetUserStudyVocabList())
            {
                if (s.id == data.Key)
                {
                    button.favoriteToggle.SetCheck(true);
                    break;
                }
            }
        }

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

        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < eachDayVocabCount; i++)
        {
            content.transform.GetChild(i).GetComponent<VocabButton>().SetVocabButton("");
            content.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        List<string> list = new List<string>();
        

        for (int i = selectedDay * eachDayVocabCount; i < (selectedDay * eachDayVocabCount) + eachDayVocabCount; i++)
        {
            list.Add(OX_DataLoader.originalData[i]["vocab"].ToString());
        }
        
        list.Sort();

        for (int i = 0; i < list.Count; i++)
        {
            content.transform.GetChild(i).gameObject.SetActive(true);
            content.transform.GetChild(i).GetComponent<VocabButton>().SetVocabButton(list[i]);
        }
        InitSelectVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
    // Sort By Type
    public void SortByType(string type)
    {
        isListLoadingDone = false;
        var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < eachDayVocabCount; i++)
        {
            content.transform.GetChild(i).GetComponent<VocabButton>().SetVocabButton("");
            content.transform.GetChild(i).gameObject.SetActive(false);
        }

        List<string> list = new List<string>();
        

        for (int i = selectedDay * eachDayVocabCount; i < (selectedDay * eachDayVocabCount) + eachDayVocabCount; i++)
        {
            var ps = OX_DataLoader.originalData[i]["ps"].ToString();
            var psList = ps.Split(new string[] { ".:" }, StringSplitOptions.None);
            
            foreach (var s in psList)
            {
                if (s.Equals(type))
                {
                    list.Add(OX_DataLoader.originalData[i]["vocab"].ToString());
                    break;
                }
            }
        }

        list.Sort();

        for (int i = 0; i < list.Count; i++)
        {
            content.transform.GetChild(i).gameObject.SetActive(true);
            content.transform.GetChild(i).GetComponent<VocabButton>().SetVocabButton(list[i]);
        }

        InitSelectVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
}
