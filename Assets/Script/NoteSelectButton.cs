using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSelectButton : MonoBehaviour
{
    [SerializeField] public Text label;
    [HideInInspector] public int vocabId = -1;
    public void OnClickNoteSelectButton()
    {
        //NetWorkManager.Instance.selectedNote = label.text;
        if (label.text.Equals("") || vocabId < 0)
        {
            Debug.Log("note name or vocab id is not set.");
            return;
        }
        NetWorkManager.Instance.SetMyVocab(label.text, vocabId);
        SelectNotePopup.closePopupCallBack();
    }
}
