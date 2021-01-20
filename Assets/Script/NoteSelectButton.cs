using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSelectButton : MonoBehaviour
{
    [SerializeField] public Text label;
    [HideInInspector] public int vocabId = -1;
    [HideInInspector] public FavoriteToggle toggle = null;
    [HideInInspector] public VocabPanel panel = null;
    public void OnClickNoteSelectButton()
    {
        //NetWorkManager.Instance.selectedNote = label.text;
        if (label.text.Equals("") || vocabId < 0)
        {
            ResetCheckMark();
            NetWorkManager.Instance.ShowWarningPopup("노트의 이름이 없습니다.");
            return;
        }

        int count = UserDataManager.Instance.GetNoteCount(label.text);
        if (count >= 30)
        {
            ResetCheckMark();
            NetWorkManager.Instance.ShowWarningPopup("현재 노트에 30개 이상 추가할 수 없습니다.");
            return;
        }

        if (count < 0)
        {
            ResetCheckMark();
            NetWorkManager.Instance.ShowWarningPopup("존재 하지 않는 노트입니다.");
            return;
        }
        UserDataManager.Instance.AddMyVocabUserNote(label.text);
        NetWorkManager.Instance.AddMyVocab(label.text, vocabId);
        UserDataManager.Instance.AddUserStudyVocab(vocabId, label.text);
        SelectNotePopup.closePopupCallBack();
    }

    private void ResetCheckMark()
    {
        if (toggle != null)
        {
            toggle.CheckButtonOff();
        }

        if (panel != null)
        {
            panel.SetColor(false);
        }
    }
}
