﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Doozy.Engine;
using Doozy.Engine.UI;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public struct MyVocabData
{
    public MyVocabData(int v, string s)
    {
        vocabId = v;
        notename = s;
    }
    public int vocabId;
    public string notename;
}

public class NetWorkManager : Singleton<NetWorkManager>
{
    private string serverURL = "http://localhost:3000/";
    private WWWForm form = null;
    private UnityWebRequest www = null;
    public bool isLoadingDone = false;
    public bool isNoteListDone = false;
    public bool isJsonDone = false;
    private string userEmail = "tpark3546@gmail.com";
    private string vocablistUrl = "getMyVocabList";
    private string noteListUrl = "getMyNotes";
    private string addVocabUrl = "addMyVocab";
    private string removeVocabUrl = "removeMyVocab";
    private string createNewNoteUrl = "createNewNote";
    private string deleteMyNoteUrl = "deleteMyNote";
    private string deleteAllMyVocabs = "deleteAllMyVocabs";
    private string getVocabDetail = "getVocabDetail";
    private string getTodayVocabDetail = "getTodayVocabDetail";

    private string recvData = string.Empty;
    
    //public List<MyVocabData> myVocabDataList = new List<MyVocabData>();
    //public List<string> noteList = new List<string>();
    //[HideInInspector] public string selectedNote = string.Empty;
    public void Awake()
    {

    }
    public void Start()
    {

    }
    public void ShowWarningPopup(string msg, Action callBack = null)
    {
        UIPopupManager.ClearQueue();
        var p = UIPopupManager.GetPopup("WarningPopup");
        if (p == null)
        {
            Debug.Log("<color=red>warning popup is null.</color>");
            return;
        }
        p.Data.SetButtonsCallbacks(() =>
        {
            if (callBack != null)
            {
                callBack();
            }
            UIPopupManager.ClearQueue();
        });
        p.Data.SetLabelsTexts("Error", msg);
        UIPopupManager.ShowPopup(p, p.AddToPopupQueue, false, "Popup");
    }

    public void GetVocabDetail(int vocabId)
    {
        StartCoroutine(GetVocabDetailCo(vocabId));
    }

    public IEnumerator GetVocabDetailCo(int vocabId)
    {
        WWWForm packet = new WWWForm();
        packet.AddField("vocab_index", vocabId);

        StartCoroutine(MesseageLoop(getVocabDetail, packet, RecvPacket));

        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
        UnPack((int)PacketType.GET_VOCAB_DETAIL);
    }
    public IEnumerator GetTodayVocabDetailCo()
    {
        WWWForm packet = new WWWForm();
        //packet.AddField("vocab_index", vocabId);

        StartCoroutine(MesseageLoop(getTodayVocabDetail, packet, RecvPacket));

        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
        UnPack((int)PacketType.GET_TODAY_VOCAB_DETAIL);
    }
    #region My Vocab Functions


    /// <summary>
    /// add vocab to the note 
    /// </summary>
    /// <param name="noteName"></param>
    /// <param name="vocabIndex"></param>
    public void AddMyVocab(string noteName, int vocabIndex)
    {
        StartCoroutine(AddMyVocabCo(noteName, vocabIndex));
    }
    public IEnumerator AddMyVocabCo(string noteName, int vocabIndex)
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", userEmail);
        packet.AddField("noteName", noteName);
        packet.AddField("vocab_index", vocabIndex);

        StartCoroutine(MesseageLoop(addVocabUrl, packet, RecvPacket));

        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
    }
    /// <summary>
    /// remove vocab from a selected note
    /// </summary>
    /// <param name="vocabId"></param>
    public void RemoveMyVocab(int vocabId)
    {
        StartCoroutine(RemoveMyVocabCo(vocabId));
    }

    public IEnumerator RemoveMyVocabCo(int vocabId)
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", "tpark3546@gmail.com");
        packet.AddField("vocab_index", vocabId);
        StartCoroutine(MesseageLoop(removeVocabUrl, packet, RecvPacket));

        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
    }

    #endregion
    public IEnumerator DeleteMyNote(string noteName)
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", userEmail);
        packet.AddField("noteName", noteName);

        StartCoroutine(MesseageLoop(deleteMyNoteUrl, packet, RecvPacket));
        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
    }
    public IEnumerator DeleteAllMyVocabs(string noteName)
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", userEmail);
        packet.AddField("noteName", noteName);

        StartCoroutine(MesseageLoop(deleteAllMyVocabs, packet, RecvPacket));
        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
    }
    public IEnumerator CreateNewNote(string newNoteName)
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", userEmail);
        packet.AddField("newNoteName", newNoteName);
        StartCoroutine(MesseageLoop(createNewNoteUrl, packet, RecvPacket));
        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
    }

    public void ShowSelectNotePopup(int vocabId
        , FavoriteToggle toggle
        , VocabPanel panel)
    {
        var p = UIPopupManager.GetPopup("SelectNotePopup");
        if (p == null)
        {
            return;
        }

        p.GetComponent<SelectNotePopup>().vocabId = vocabId;
        p.GetComponent<SelectNotePopup>().InitPopup(toggle, panel);
        UIPopupManager.ShowPopup(p, p.AddToPopupQueue, false, "Popup");
    }

    public IEnumerator GetMyNoteList()
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", userEmail);
        StartCoroutine(MesseageLoop(noteListUrl, packet, RecvPacket));
        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
        UnPack((int)PacketType.GET_MY_NOTE_LIST);
    }
    public IEnumerator GetVocabList()
    {
        WWWForm packet = new WWWForm();
        packet.AddField("userid", userEmail);
        StartCoroutine(MesseageLoop(vocablistUrl, packet, RecvPacket));
        yield return new WaitWhile(() =>
        {
            return false == isLoadingDone;
        });
        UnPack((int)PacketType.GET_MY_VOCAB_LIST);
    }

    public void UnPack(int type)
    {
        var data = JSON.Parse(recvData);
        while (data == null)
        {
            Debug.Log("recvData is null.");
            return;
        }

        switch (type)
        {
            case (int)PacketType.GET_MY_VOCAB_LIST:
                SetMyVocabList(data);
                break;
            case (int)PacketType.GET_MY_NOTE_LIST:
                SetMyNoteList(data);
                break;
            case (int)PacketType.GET_VOCAB_DETAIL:
                SetVocabDetail(data);
                break;
            case (int)PacketType.GET_TODAY_VOCAB_DETAIL:
                SetTodayVocabDetail(data);
                break;
        }
    }

    public void SetMyVocabList(JSONNode data)
    {
        isJsonDone = false;
        //myVocabDataList.Clear();
        foreach (var d in data)
        {
            MyVocabData vocabData = new MyVocabData(d.Value["vocab_index"]
                , d.Value["note_name"]);
            //myVocabDataList.Add(vocabData);
            UserDataManager.Instance.AddUserStudyVocab(vocabData.vocabId, vocabData.notename);
        }
        isJsonDone = true;
    }

    public void SetMyNoteList(JSONNode data)
    {
        isJsonDone = false;
        //noteList.Clear();
        foreach (var d in data)
        {
            //noteList.Add(d.Value["note_name"]);
            UserDataManager.Instance.AddUserNote(d.Value["note_name"]);
        }
        isJsonDone = true;
    }
    public void SetTodayVocabDetail(JSONNode data)
    {
        isJsonDone = false;
        //noteList.Clear();
        foreach (var d in data)
        {
            UserDataManager.Instance.todayVocabData = new TodayVocab(
                Int32.Parse(d.Value["id"]),
                d.Value["vocab"],
                d.Value["def"],
                d.Value["type"],
                d.Value["example1"],
                d.Value["translate1"],
                d.Value["example2"],
                d.Value["translate2"]);
        }
        isJsonDone = true;
    }
    public void SetVocabDetail(JSONNode data)
    {
        isJsonDone = false;
        //noteList.Clear();
        foreach (var d in data)
        {
            UserDataManager.Instance.vocabData = new CurrentVocab(d.Value["def"],
                d.Value["type"],
                d.Value["example1"],
                d.Value["translate1"],
                d.Value["example2"],
                d.Value["translate2"]);
        }
        isJsonDone = true;
    }
    public void RecvPacket(string data)
    {
        recvData = data;
    }
    public IEnumerator MesseageLoop(string url, WWWForm packet, Action<string> recvCallBack)
    {
        isLoadingDone = false;
        www = UnityWebRequest.Post(serverURL + url, packet);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            isLoadingDone = true;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            string data = www.downloadHandler.text;
            recvCallBack(data);
            isLoadingDone = true;
        }
    }
}
