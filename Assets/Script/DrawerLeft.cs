using System;
using System.Collections;
using BackEnd;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class DrawerLeft : MonoBehaviour
{
    public static DrawerLeft main = null;
    public static Action hideBackButtonCallBack = null;
    private bool isMyListLoadingDone = false;
    private bool isCreateNewNote = false;

    void Start()
    {
        main = gameObject.GetComponent<DrawerLeft>();
    }
    public void OnClickHome()
    {
        ClearTitleRecord();
        StatusBar.SetStatusTitle((int) Title.HOME);
        hideBackButtonCallBack();
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);

        ViewHome.main.homeVocabTouchNotice.SetLabel("단어를 더블 터치하면단어장에 추가됩니다.");
    }
    public void OnClickVocab()
    {
        ClearTitleRecord();
        StatusBar.RecordPrevTitle((int) Title.HOME);
        StatusBar.SetStatusTitle((int)Title.VOCAB_DAY);

        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);
    }
    //public void ResetOtherPages()
    //{
    //    if (BackButtonController.resetVocabDayScrollPos != null)
    //    {
    //        BackButtonController.resetVocabDayScrollPos();
    //    }

    //    if (BackButtonController.resetVocabListScrollPos != null)
    //    {
    //        BackButtonController.resetVocabListScrollPos();
    //    }
    //}

    public void OnClickVocabTest()
    {
        ClearTitleRecord();
        StatusBar.RecordPrevTitle((int)Title.HOME);
        StatusBar.SetStatusTitle((int)Title.VOCAB_TEST_DAY);

        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);
    }

    private void ClearTitleRecord()
    {
        for (int i = 0; i < StatusBar.prevTitle.Count; i++)
        {
            StatusBar.prevTitle.Pop();
        }
    }

    public void OnClickLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();
        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetMessage());
        }
        else
        {
            
        }

        /* 비동기
         * Update() 에서 처리할 거 없음.  
        Backend.BMember.Logout((callback) => {
            // 이후 처리
        });
        */
    }

    public void OnClickMyList()
    {
        ClearTitleRecord();
        StatusBar.RecordPrevTitle((int)Title.HOME);
        StatusBar.SetStatusTitle((int)Title.MyVocabList);

        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(true);
        
        MyNoteList.main.vocabTouchNotice.SetLabel("최대 10개의 노트를 만들 수 있습니다.");
        MyNoteList.main.InitNoteList();
    }
}
