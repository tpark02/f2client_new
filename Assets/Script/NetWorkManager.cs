using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class NetWorkManager : Singleton<NetWorkManager>
{
    private WWWForm form;

    public IEnumerator GetMyNoteList()
    {
        form = new WWWForm();
        form.AddField("userid", "tpark3546@gmail.com");
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/getMyNotes", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);
            if (data.Count <= 0)
            {
                Debug.Log("create new note");
            }
            else
            {
                var content = MyNoteList.myNoteList.GetComponent<MyNoteList>().content;
                foreach (Transform child in content.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (var d in data)
                {
                    Debug.Log("email:" + d.Value["user_email"] + " note name :" + d.Value["note_name"]);
                    var b = MyNoteList.myNoteList.GetComponent<MyNoteList>().myNoteButton;
                    var o = Instantiate(b.gameObject);
                    o.transform.SetParent(MyNoteList.myNoteList.GetComponent<MyNoteList>().content, false);
                    o.GetComponent<MyNoteButton>().label.text = d.Value["note_name"];
                    yield return null;
                }

                GameEventMessage.SendEvent("MyNoteListDone");
            }
        }
    }

    public IEnumerator DeleteMyNote(string noteName)
    {
        form = new WWWForm();
        form.AddField("userid", "tpark@gmail.com");
        form.AddField("noteName", noteName);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/deleteMyNote", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
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
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/createNewNote", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
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

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/renameMyNote", form);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var data = JSON.Parse(www.downloadHandler.text);
            Debug.Log("create new note result : " + data["result"]);
        }
    }
}
