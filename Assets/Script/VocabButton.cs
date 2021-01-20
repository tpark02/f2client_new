using System;
using System.Collections;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class VocabButton : MonoBehaviour
{
    [SerializeField] public Text vocab;

    [SerializeField] public FavoriteToggle favoriteToggle;
    //[SerializeField] public Toggle favoriteToggle;
    private OX_DataLoader.VocabData vocabData;
    //[HideInInspector] public bool startToggleFunction = false;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickVocabButton);
    }

    public void SetVocabButton(string v, OX_DataLoader.VocabData data)
    {
        vocab.text = v;
        vocabData = data;
        //vocabData = OX_DataLoader.GetVocab(vocab.text);
    }
    
    public void OnClickVocabButton()
    {
        StatusBar.RecordPrevTitle((int)Title.VOCAB_LIST);
        StatusBar.SetStatusTitle((int)Title.VOCAB_DETAIL);

        //StartCoroutine(NetWorkManager.Instance.GetVocabList());

        StartCoroutine(LoadVocabDetail());
    }

    private IEnumerator LoadVocabDetail()
    {
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isLoadingDone;
        });
        
        ViewVocabDetail.isDetailDone = false;
        ViewVocabDetail.viewVocabDetail.GetComponent<ViewVocabDetail>().SetVocabDetail(vocabData, vocabData.id, vocabData.vocab
            , vocabData.def
            , vocabData.e1
            , vocabData.t1
            , vocabData.e2
            , vocabData.t2);
        
        ViewVocabDetail.isDetailDone = true;

        yield return new WaitWhile(() =>
        {
            return ViewVocabDetail.isDetailDone == false;
        });


        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        //ViewVocabDetail.viewVocabDetail.SetActive(true);

        GameEventMessage.SendEvent("LoadingVocabListDone");
    }
}
