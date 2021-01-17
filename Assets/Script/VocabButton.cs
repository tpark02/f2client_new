using System;
using System.Collections;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class VocabButton : MonoBehaviour
{
    [SerializeField] public Text vocab;
    [SerializeField] public UIToggle favoriteToggle;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickVocabButton);
    }

    public void SetVocabButton(string v)
    {
        vocab.text = v;
    }
    
    public void OnClickVocabButton()
    {
        StatusBar.RecordPrevTitle((int)Title.VOCAB);
        StatusBar.SetStatusTitle((int)Title.VOCAB_DETAIL);

        //StartCoroutine(LoadVocabDetail());
        LoadVocabDetail();
    }

    private void LoadVocabDetail()
    {
        ViewVocabDetail.viewVocabDetail.SetActive(true);
        var d = OX_DataLoader.GetVocab(vocab.text);
        ViewVocabDetail.isDetailDone = false;
        
        ViewVocabDetail.viewVocabDetail.GetComponent<ViewVocabDetail>().SetVocabDetail(vocab.text
            , d.def
            , d.e1
            , d.t1
            , d.e2
            , d.t2);
        
        ViewVocabDetail.isDetailDone = true;
        //yield return new WaitWhile(() =>
        //{
        //    return ViewVocabDetail.isDetailDone == false;
        //});
        
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        //GameEventMessage.SendEvent("VocabDetailDone");
    }
}
