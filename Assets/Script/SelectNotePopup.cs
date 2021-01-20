using System;
using Doozy.Engine.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SelectNotePopup : MonoBehaviour
{
    public static Action toggleOffCallBack = null;
    public static Action closePopupCallBack = null;
    [SerializeField] public Transform content;
    [SerializeField] public NoteSelectButton selectVocabButton;
    [HideInInspector] public int vocabId = -1;

    void Start()
    {
        
    }
    public void InitPopup(FavoriteToggle toggle, VocabPanel panel)
    {
        var notelist = UserDataManager.Instance.GetNoteList();
        foreach (var d in notelist)
        {
            var o = Instantiate(selectVocabButton);
            o.transform.SetParent(content, false);
            o.GetComponent<NoteSelectButton>().label.text = d.Key;
            o.GetComponent<NoteSelectButton>().vocabId = vocabId;
            o.GetComponent<NoteSelectButton>().toggle = toggle;
            o.GetComponent<NoteSelectButton>().panel = panel;
        }

        closePopupCallBack = () =>
        {
            UIPopupManager.ClearQueue();
        };
    }
    public void ClosePopup()
    {
        if (toggleOffCallBack != null)
        {
            toggleOffCallBack();
            toggleOffCallBack = null;
        }
        UIPopupManager.ClearQueue();
    }
}
