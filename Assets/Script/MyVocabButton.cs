using System;
using System.Collections;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class MyVocabButton : MonoBehaviour
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
        StatusBar.RecordPrevTitle((int)Title.MyVocabList);
        StatusBar.SetStatusTitle((int)Title.MyVocabDetail);

        //StartCoroutine(LoadVocabDetail());
    }

    //private IEnumerator LoadVocabDetail()
    //{
    //    MyVocabDetail.myVocabDetail.SetActive(true);
    //    var d = OX_DataLoader.GetVocab(vocab.text);
    //    MyVocabDetail.isDetailDone = false;

    //    MyVocabDetail.myVocabDetail.GetComponent<MyVocabDetail>().SetVocabDetail(vocab.text
    //        , d.def
    //        , d.e1
    //        , d.t1
    //        , d.e2
    //        , d.t2);

    //    MyVocabDetail.isDetailDone = true;
    //    yield return new WaitWhile(() =>
    //    {
    //        return MyVocabDetail.isDetailDone == false;
    //    });

    //    StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

    //    GameEventMessage.SendEvent("MyVocabDetailDone");
    //}
}