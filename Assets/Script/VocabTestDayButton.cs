using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

class VocabTestDayButton : MonoBehaviour
{
    
    [SerializeField] public Text testday;
    private int nTestDay;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickDayTestButton);
    }
    public void SetTestDay(string s, int d)
    {
        testday.text = s;
        nTestDay = d;
    }
    public void OnClickDayTestButton()
    {
        StatusBar.RecordPrevTitle((int)Title.VOCAB_TEST_DAY);
        StatusBar.SetStatusTitle((int)Title.VOCAB_TEST);

        OX_DataLoader.resultList.Clear();
        OX_DataLoader.InitVocabList(nTestDay);
        ViewVocabTest.viewVocabTest.GetComponent<ViewVocabTest>().LoadTestList(nTestDay);
    }
}
