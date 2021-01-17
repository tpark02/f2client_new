using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class VocabDayButton : MonoBehaviour
{
    [SerializeField] public Text day;
    private int nDay;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickDayButton);
    }
 
    public void SetDay(string s, int d)
    {
        day.text = s;
        nDay = d;
    }

    public void OnClickDayButton()
    {
        StatusBar.RecordPrevTitle((int) Title.VOCAB_DAY);
        StatusBar.SetStatusTitle((int)Title.VOCAB);

        ViewVocabList.viewVocabList.SetActive(true);
        //StartCoroutine(LoadVocabList(nDay));
        OX_DataLoader.InitVocabList(nDay);
        ViewVocabList.viewVocabList.GetComponent<ViewVocabList>().LoadVocabRoutine(nDay);
    }

    //private void LoadVocabList(int d)
    //{
    //    //ViewVocabList.viewVocabList.GetComponent<ViewVocabList>().LoadVocabRoutine(d);
    //    //yield return new WaitWhile(() =>
    //    //{
    //    //    return ViewVocabList.isListLoadingDone == false;
    //    //});
    //    //GameEventMessage.SendEvent("VocabListDone");
    //}
}
