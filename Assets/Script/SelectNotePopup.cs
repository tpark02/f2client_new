using System;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class SelectNotePopup : MonoBehaviour
{
    public static Action closePopupCallBack = null;
    [SerializeField] public Transform content;
    [SerializeField] public NoteSelectButton selectVocabButton;
    [HideInInspector] public int vocabId = -1;
    void Start()
    {
        
    }
    public void InitPopup()
    {
        var notelist = NetWorkManager.Instance.noteList;
        foreach (var d in notelist)
        {
            var o = Instantiate(selectVocabButton);
            o.transform.SetParent(content, false);
            o.GetComponent<NoteSelectButton>().label.text = d;
            o.GetComponent<NoteSelectButton>().vocabId = vocabId;
        }

        closePopupCallBack = ClosePopup;
    }
    public void ClosePopup()
    {
        UIPopupManager.ClearQueue();
    }
}
