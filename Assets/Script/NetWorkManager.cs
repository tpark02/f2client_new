using System.Collections;
using System.Collections.Generic;
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
    private WWWForm form = null;
    private UnityWebRequest www = null;
    private bool isLoadingDone = false;
    private bool isNoteListDone = false;
    [HideInInspector] public List<string> noteList = new List<string>();
    [HideInInspector] public List<MyVocabData> myVocabDataList = new List<MyVocabData>();
    //[HideInInspector] public string selectedNote = string.Empty;
    public void Awake()
    {

    }
    public void Start()
    {

    }
    public void ShowWarningPopup(string msg)
    {
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
    /// Reload Vocab List
    /// </summary>
    public void ReloadVocabList(string userid, int day)
    {
        StartCoroutine(ReloadVocabListCo(userid, day));
    }

    public IEnumerator ReloadVocabListCo(string userid, int day)
    {
        StartCoroutine(GetMyVocabListCo(userid));
        yield return new WaitWhile(() =>
        {
            return isLoadingDone == false;
        });
        ViewVocabList.viewVocabList.GetComponent<ViewVocabList>().LoadVocabRoutine(day);
        yield return new WaitWhile(() =>
        {
            return ViewVocabList.isListLoadingDone == false;
        });
        GameEventMessage.SendEvent("ReloadVocabListDone");
    }
    /// <summary>
    /// add vocab to the note 
    /// </summary>
    /// <param name="noteName"></param>
    /// <param name="vocabIndex"></param>
    public void SetMyVocab(string noteName, int vocabIndex)
    {
        StartCoroutine(SetMyVocabCo(noteName, vocabIndex));
    }
    public IEnumerator SetMyVocabCo(string noteName, int vocabIndex)
    {
        form = new WWWForm();
        form.AddField("userid", "tpark3546@gmail.com");
        form.AddField("noteName", noteName);
        form.AddField("vocab_index", vocabIndex);

        www = UnityWebRequest.Post("http://localhost:3000/setMyVocab", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            //Debug.Log("<color=red> set vocab failed !</color>");
            ShowWarningPopup("set vocab failed !");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("<color=yellow> set vocab success !</color>");
        }
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
        form = new WWWForm();
        form.AddField("userid", "tpark3546@gmail.com");
        form.AddField("vocab_index", vocabId);

        www = UnityWebRequest.Post("http://localhost:3000/removeMyVocab", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            ShowWarningPopup("remove vocab failed !");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("<color=yellow> remove vocab success !</color>");
        }
    }

    /// <summary>
    /// Get My Vocab List
    /// </summary>
    public void GetMyVocabList(string userid)
    {
        StartCoroutine(GetMyVocabListCo(userid));
    }

    public IEnumerator GetMyVocabListCo(string userid)
    {
        isLoadingDone = false;
        form = new WWWForm();
        form.AddField("userid", "tpark3546@gmail.com");

        www = UnityWebRequest.Post("http://localhost:3000/getMyVocabList", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            isLoadingDone = true;
            Debug.Log(www.error);
            ShowWarningPopup("get my vocab list failed !");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);

            if (data.Count <= 0)
            {
                Debug.Log("<color=red> there are no vocabs list.");
            }
            else
            {
                myVocabDataList.Clear();
                foreach (var o in data)
                {
                    string name = o.Value["note_name"];
                    int vocabId = o.Value["vocab_index"];

                    MyVocabData d = new MyVocabData(vocabId, name);
                    myVocabDataList.Add(d);
                    yield return null;
                }
                Debug.Log("<color=yellow> get my vocab list success !</color>");
            }
            isLoadingDone = true;

        }
    }
    #endregion
    public void LoadDataFromServer()
    {
        StartCoroutine(LoadDataFromServerCo());
    }

    public IEnumerator LoadDataFromServerCo()
    {
        
        StartCoroutine(GetMyVocabListCo("tpark3546@gmail.com"));
        yield return new WaitWhile(() =>
        {
            return isLoadingDone == false;
        });
        StartCoroutine(GetMyNoteListCo());
        yield return new WaitWhile(() =>
        {
            return isLoadingDone == false;
        });
        GameEventMessage.SendEvent("PrepareDataDone");
    }
    public IEnumerator GetMyNoteListCo()
    {
        isNoteListDone = false;
        form = new WWWForm();
        form.AddField("userid", "tpark3546@gmail.com");
        www = UnityWebRequest.Post("http://localhost:3000/getMyNotes", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            isNoteListDone = true;
            ShowWarningPopup("get my note list failed !");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);
            if (data.Count <= 0)
            {
                Debug.Log("<color=yellow> there are no notes. create a new note</color>");
            }
            else
            {
                noteList.Clear();
                foreach (var d in data)
                {
                    noteList.Add(d.Value["note_name"]);
                    yield return null;
                }
                Debug.Log("<color=yellow>get my note list success !</color>");
            }
            isNoteListDone = true;
        }
    }

    public IEnumerator DeleteMyNote(string noteName)
    {
        form = new WWWForm();
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
        form = new WWWForm();
        form.AddField("userid", "tpark3546@gmail.com");
        form.AddField("newNoteName", newNoteName);
        
        www = UnityWebRequest.Post("http://localhost:3000/createNewNote", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            ShowWarningPopup("create a new note failed !");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);
            Debug.Log("create new note result : " + data["result"]);
        }
    }

    public IEnumerator RenameMyNote(string newNoteName)
    {
        form = new WWWForm();
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

    public bool IsVocabInMyNote(int vocabId)
    {
        foreach (var myVocabData in myVocabDataList)
        {
            if (myVocabData.vocabId == vocabId)
            {
                return true;
            }
        }

        return false;
    }
}
