using System;
using System.Collections;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class MyVocabButton : MonoBehaviour
{
    [SerializeField] public Text vocab;
    [HideInInspector] public OX_DataLoader.VocabData vocabData;
    [SerializeField] public FavoriteToggle favoriteToggle;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickVocabButton);
    }

    public void SetVocabButton(string v, OX_DataLoader.VocabData data)
    {
        vocab.text = v;
        vocabData = data;
    }

    public void OnClickVocabButton()
    {
        StatusBar.RecordPrevTitle((int)Title.MyVocabList);
        StatusBar.SetStatusTitle((int)Title.MyVocabDetail);

        StartCoroutine(LoadVocabDetail());
    }

    private IEnumerator LoadVocabDetail()
    {
        MyVocabDetail.myVocabDetail.SetActive(true);
        
        MyVocabDetail.isDetailDone = false;

        MyVocabDetail.myVocabDetail.GetComponent<MyVocabDetail>().SetVocabDetail(vocabData, vocabData.vocab
            , vocabData.def
            , vocabData.e1
            , vocabData.t1
            , vocabData.e2
            , vocabData.t2);

        MyVocabDetail.isDetailDone = true;
        yield return new WaitWhile(() =>
        {
            return MyVocabDetail.isDetailDone == false;
        });

        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        GameEventMessage.SendEvent("MyVocabDetailDone");
    }
}