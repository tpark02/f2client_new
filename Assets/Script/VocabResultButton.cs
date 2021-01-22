using System.Collections;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class VocabResultButton : MonoBehaviour
{
    [SerializeField] public Text vocab;
    [SerializeField] public OX_DataLoader.VocabData vocabData;
    [SerializeField] public GameObject correct;

    [SerializeField] public GameObject wrong;

    public void OnClickVocabButton()
    {
        StatusBar.RecordPrevTitle((int)Title.VOCAB_TEST_RESULT);
        StatusBar.SetStatusTitle((int)Title.VOCAB_TEST_RESULT_DETAIL);

        ViewVocabResultDetail.main.gameObject.SetActive(true);

        StartCoroutine(LoadVocabDetail(vocabData.id));
    }
    public IEnumerator LoadVocabDetail(int vocabId)
    {
        StartCoroutine(NetWorkManager.Instance.GetVocabDetailCo(vocabId));
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });

        ViewVocabResultDetail.main.SetVocabDetail(vocabData);

        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        GameEventMessage.SendEvent("VocabDetailLoadingDone");
    }
}
