using System;
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
    public void ShowWarningPopup(string msg)
    {
        UIPopupManager.ClearQueue();
        var p = UIPopupManager.GetPopup("WarningPopup");
        if (p == null)
        {
            Debug.Log("<color=red>warning popup is null</color>");
            return;
        }
        p.Data.SetButtonsCallbacks(() =>
        {
            UIPopupManager.ClearQueue();
        });
        p.Data.SetLabelsTexts("Error", msg);
        UIPopupManager.ShowPopup(p, p.AddToPopupQueue, false, "Popup");
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
        form.AddField("userid", "tpark@gmail.com");
        form.AddField("noteName", noteName);

        www = UnityWebRequest.Post("http://localhost:3000/deleteMyNote", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            ShowWarningPopup("Delete my note error.");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);
            Debug.Log("create new note result : " + data["result"]);
        }
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

    public IEnumerator RenameMyNote(string newNoteName)
    {
        WWWForm packet = new WWWForm();
        form.AddField("userid", "tpark@gmail.com");
        form.AddField("newNoteName", newNoteName);

        www = UnityWebRequest.Post("http://localhost:3000/renameMyNote", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            ShowWarningPopup("rename my note failed !");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);
            Debug.Log("create new note result : " + data["result"]);
        }
    }

    public void ShowSelectNotePopup(int vocabId)
    {
        var p = UIPopupManager.GetPopup("SelectNotePopup");
        if (p == null)
        {
            return;
        }

        p.GetComponent<SelectNotePopup>().vocabId = vocabId;
        p.GetComponent<SelectNotePopup>().InitPopup();
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
        UnPack((int)PacketType.MY_NOTE_LIST);
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
        UnPack((int)PacketType.MY_VOCAB_LIST);
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
            case (int)PacketType.MY_VOCAB_LIST:
                SetMyVocabList(data);
                break;
            case (int)PacketType.MY_NOTE_LIST:
                SetMyNoteList(data);
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
            UserDataManager.Instance.InitUserNote(d.Value["note_name"]);
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
