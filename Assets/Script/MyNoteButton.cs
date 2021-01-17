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

        LoadVocabList();
    }
    private void LoadVocabList()
    {
        //OX_DataLoader.InitVocabList(d);

        //ViewVocabTest.viewVocabTest.GetComponent<ViewVocabTest>().LoadTestList(d);
    }
    void SetNoteName(string s)
    {
        label.text = s;
    }
}
