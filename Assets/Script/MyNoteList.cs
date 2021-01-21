using System;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class MyNoteList : MonoBehaviour
{
    public static bool isInitNoteListDone = false;
    public static Action showBackButtonCallBack = null;
    public static MyNoteList main = null;
    [SerializeField] public Transform content;
    [SerializeField] public MyNoteButton myNoteButton; 
    [SerializeField] public GameObject emptyPanel;
    [SerializeField] public GameObject scrollView;

    private Vector3 startPos;

    void Awake()
    {
        main = gameObject.GetComponent<MyNoteList>();
    }
    void Start()
    {
        //var d = OX_DataLoader.eachDayVocabCount;
        //for (int i = 0; i < d; i++)
        //{
        //    content.transform.GetChild(i).GetComponent<VocabDayButton>().SetDay("Note " + (i + 1).ToString(), i);
        //}
        
        startPos = content.localPosition;
        BackButtonController.resetVocabDayScrollPos = ResetScrollPos;
    }

    public void OnView()
    {
        showBackButtonCallBack();
        ResetScrollPos();
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(true);
    }
    public void InitNoteList()
    {
        isInitNoteListDone = false;
        //var myNoteList = MyNoteList.myNoteList.GetComponent<MyNoteList>();
        //var content = myNoteList.content;

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        var notelist = UserDataManager.Instance.GetNoteList();
        var noteCount = notelist.Count;
        if (noteCount <= 0)
        {
            scrollView.SetActive(false);
            emptyPanel.SetActive(true);
            isInitNoteListDone = true;
            return;
        }
        else
        {
            scrollView.SetActive(true);
            emptyPanel.SetActive(false);
        }
        foreach (var d in notelist)
        {
            Debug.Log(" note name :" + d);
            var b = myNoteButton;
            var o = Instantiate(b.gameObject);
            o.transform.SetParent(content, false);
            o.GetComponent<MyNoteButton>().SetNoteName(d.Key);
        }

        isInitNoteListDone = true;
        GameEventMessage.SendEvent("MyNoteListDone");
    }
    public void ResetScrollPos()
    {
        content.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }
}