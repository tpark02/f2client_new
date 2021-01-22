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

        MyVocabDetail.main.gameObject.SetActive(true);

        StartCoroutine(LoadVocabDetail(vocabData.id));
    }

    private IEnumerator LoadVocabDetail(int vocabId)
    {
        StartCoroutine(NetWorkManager.Instance.GetVocabDetailCo(vocabId));
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });

        MyVocabDetail.main.SetVocabDetail(vocabData);

        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        GameEventMessage.SendEvent("VocabDetailLoadingDone");
    }
}