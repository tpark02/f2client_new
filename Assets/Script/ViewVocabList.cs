using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ViewVocabList : MonoBehaviour
{
    [SerializeField] public Transform content;
    [SerializeField] public Scrollbar scrollBar;

    public static Action showBackButtonCallBack = null;
    public static Action<float> enableBackButtonCallBack = null;
    public static Action InitSelectVocabScrollListCallBack = null;
    public static Action<Dictionary<string, OX_DataLoader.VocabData>> InitSelectVocabBySortTypeCallBack = null;

    public static GameObject main = null;
    public static bool isListLoadingDone = false;
    
    private Vector3 startPos;
    private int selectedDay = 0;        // for sorting function

    void Start()
    {
        main = gameObject;
        startPos = content.localPosition;
        BackButtonController.resetVocabListScrollPos = ResetScrollPos;
        StatusBar.sortAlphabeticallyCallBack = SortAlphabetically;
        StatusBar.sortByTypeCallBack = SortByType;

        scrollBar.onValueChanged.AddListener((v) =>
        {
            Debug.Log(v.ToString());
            
            if (v < 0.5f)
            {
                
                enableBackButtonCallBack(v);
                return;
            }
            enableBackButtonCallBack(v);
            
        });
    }
    public void OnView()
    {
        var bar = StatusBar.statusBar.GetComponent<StatusBar>();
        bar.sortButton.transform.GetChild(0).GetComponent<Text>().text = "Sort List  ";
        bar.sortButton.SetActive(true);

        showBackButtonCallBack();
        
        ResetScrollPos();

        //foreach (Transform c in content.transform)
        //{
        //    //c.GetComponent<VocabButton>().favoriteToggle = false;
        //}

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
        
        var list = OX_DataLoader.GetCurrentDayVocabList();
        for (int i = 0; i < list.Count; i++)
        {
            var data = list[i];
            var button = content.transform.GetChild(i).GetComponent<VocabButton>();

            button.favoriteToggle.SetCheck(false);
            button.favoriteToggle.vocabData = data;

            button.SetVocabButton(data.vocab, data);
            content.transform.GetChild(i).gameObject.SetActive(true);
            var userlist = UserDataManager.Instance.GetUserStudyVocabList();
            foreach (var u in userlist)
            {
                if (data.id == u.Key)
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

        var rowCount = OX_DataLoader.eachDayVocabCount;

        for (int i = 0; i < rowCount; i++)
        {
            var c = content.transform.GetChild(i);
            {
                var emptydata = OX_DataLoader.GetEmptyVocabData();
                c.GetComponent<VocabButton>().SetVocabButton("", emptydata);
                c.GetComponent<VocabButton>().favoriteToggle.SetCheck(false);
                c.GetComponent<VocabButton>().favoriteToggle.vocabData = emptydata;
                c.gameObject.SetActive(false);
            }
        }
        
        Dictionary<string, OX_DataLoader.VocabData> list = new Dictionary<string, OX_DataLoader.VocabData>();

        var vocablist = OX_DataLoader.GetCurrentDayVocabList();
        for (int i = 0; i < vocablist.Count; i++)
        {
            var data = vocablist[i];
            list.Add(data.vocab, data);
        }

        var l = list.Keys.ToList();
        l.Sort();

        var userlist = UserDataManager.Instance.GetUserStudyVocabList();

        for (int i = 0; i < list.Count; i++)
        {
            var vocabData = list[l[i]];

            var c = content.transform.GetChild(i);
            {
                c.gameObject.SetActive(true);
                c.GetComponent<VocabButton>().SetVocabButton(l[i], vocabData);
                c.GetComponent<VocabButton>().favoriteToggle.vocabData = vocabData;
            }

            foreach (var data in userlist)
            {
                if (vocabData.id == data.Key)
                {
                    c.GetComponent<VocabButton>().favoriteToggle.SetCheck(true);
                    break;
                }
            }
        }
        InitSelectVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
    // Sort By Type
    public void SortByType(string type)
    {
        isListLoadingDone = false;
        //var eachDayVocabCount = OX_DataLoader.eachDayVocabCount;
        var vocabList = OX_DataLoader.GetCurrentDayVocabList();
        for (int i = 0; i < vocabList.Count; i++)
        {
            var emptydata = OX_DataLoader.GetEmptyVocabData();
            var c = content.transform.GetChild(i);
            c.GetComponent<VocabButton>().SetVocabButton("", emptydata);
            c.GetComponent<VocabButton>().favoriteToggle.SetCheck(false);
            c.GetComponent<VocabButton>().favoriteToggle.vocabData = emptydata;
            c.gameObject.SetActive(false);
        }

        Dictionary<string, OX_DataLoader.VocabData> list = new Dictionary<string, OX_DataLoader.VocabData>();


        for (int i = 0; i < vocabList.Count; i++)
        {
            var data = vocabList[i];
            var ps = data.type;
            var psList = ps.Split(new string[] { ".:" }, StringSplitOptions.None);
            
            foreach (var s in psList)
            {
                if (s.Equals(type))
                {
                    //var data = OX_DataLoader.GetVocabDataById(i);
                    list.Add(data.vocab, data);
                    break;
                }
            }
        }

        var l = list.Keys.ToList();
        l.Sort();

        var userlist = UserDataManager.Instance.GetUserStudyVocabList();

        for (int i = 0; i < list.Count; i++)
        {
            var vocabData = list[l[i]];

            var c = content.transform.GetChild(i);
            {
                c.gameObject.SetActive(true);
                c.GetComponent<VocabButton>().SetVocabButton(l[i], vocabData);
                c.GetComponent<VocabButton>().favoriteToggle.vocabData = vocabData;
            }

            foreach (var data in userlist)
            {
                if (vocabData.id == data.Key)
                {
                    c.GetComponent<VocabButton>().favoriteToggle.SetCheck(true);
                    break;
                }
            }
        }

        InitSelectVocabBySortTypeCallBack(list);
        isListLoadingDone = true;
    }
}
