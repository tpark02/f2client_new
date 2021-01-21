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
    private OX_DataLoader.VocabData vocabData;
    private bool isDetailLoadingDone = false;
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
        StatusBar.RecordPrevTitle((int)Title.VOCAB_LIST);
        StatusBar.SetStatusTitle((int)Title.VOCAB_DETAIL);

        ViewVocabDetail.main.gameObject.SetActive(true);

        StartCoroutine(LoadVocabDetail(vocabData.id));
    }

    public IEnumerator LoadVocabDetail(int vocabId)
    {
        StartCoroutine(NetWorkManager.Instance.GetVocabDetailCo(vocabId));
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });
        
        ViewVocabDetail.main.SetVocabDetail(vocabData);

        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        GameEventMessage.SendEvent("VocabDetailLoadingDone");
    }
}
