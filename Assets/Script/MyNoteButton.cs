using System.Collections;
using System.Collections.Generic;
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
}
