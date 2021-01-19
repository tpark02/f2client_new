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

        int count = UserDataManager.Instance.GetNoteCount(label.text);
        if (count >= 30)
        {
            NetWorkManager.Instance.ShowWarningPopup("현재 노트에 30개 이상 추가할 수 없습니다.");
            return;
        }

        if (count < 0)
        {
            NetWorkManager.Instance.ShowWarningPopup("존재 하지 않는 노트입니다.");
            return;
        }
        UserDataManager.Instance.AddMyVocabUserNote(label.text);
        NetWorkManager.Instance.AddMyVocab(label.text, vocabId);
        OX_DataLoader.AddToUserList(vocabId, label.text);
        SelectNotePopup.closePopupCallBack();
    }
}
