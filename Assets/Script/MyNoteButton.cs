using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class MyNoteButton : MonoBehaviour
{
    [SerializeField] public Text label;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickVocabButton);
    }

    public void OnClickVocabButton()
    {
        StatusBar.RecordPrevTitle((int)Title.MyNoteList);
        StatusBar.SetStatusTitle((int)Title.MyVocabList);
        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.SetActive(false);
        LoadVocabList();
    }
    private void LoadVocabList()
    {
        MyVocabList.myVocabList.GetComponent<MyVocabList>().LoadVocabRoutine(label.text);
    }
    public void SetNoteName(string s)
    {
        label.text = s;
    }

    public void OnClickDeleteButton()
    {
        var p = UIPopupManager.GetPopup("DeleteNotePopup");
        if (p == null)
        {
            Debug.Log("<color=red>Delete popup is null.</color>");
            return;
        }

        p.GetComponent<DeleteNotePopup>().noteName = label.text;
        
        string str = string.Format("<color=red><size=50>{0}</size></color>\n 선택한 노트를 삭제할까요?", label.text);
        p.Data.SetLabelsTexts("Delete Note", str);
        UIPopupManager.ShowPopup(p, p.AddToPopupQueue, false, "Popup");
    }
}
