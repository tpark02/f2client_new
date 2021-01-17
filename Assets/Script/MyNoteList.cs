using System;
using UnityEngine;
using UnityEngine.UI;

public class MyNoteList : MonoBehaviour
{
    public static GameObject myNoteList;
    [SerializeField] public Transform content;
    [SerializeField] public MyNoteButton myNoteButton; 
    public static Action showBackButtonCallBack = null;

    private Vector3 startPos;

    void Awake()
    {
        myNoteList = gameObject;
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
    public void ResetScrollPos()
    {
        content.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }
}