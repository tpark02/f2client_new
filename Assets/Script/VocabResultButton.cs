using System.Collections;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class VocabResultButton : MonoBehaviour
{
    [SerializeField] public Text vocab;

    [SerializeField] public GameObject correct;

    [SerializeField] public GameObject wrong;

    public void OnClickVocabButton()
    {
        StatusBar.RecordPrevTitle((int)Title.VOCAB_TEST_RESULT);
        StatusBar.SetStatusTitle((int)Title.VOCAB_TEST_RESULT_DETAIL);

        StartCoroutine(LoadVocabDetail());
        //LoadVocabDetail();
    }
    private IEnumerator LoadVocabDetail()
    {
        ViewVocabResultDetail.main.gameObject.SetActive(true);
        var d = OX_DataLoader.GetVocab(vocab.text);
        
        ViewVocabResultDetail.isDetailDone = false;

        //ViewVocabResultDetail.main.SetDetail(d.id);

        //yield return new WaitWhile(() =>
        //{
        //    return ViewVocabResultDetail.main.isDetailLoadingDone == false;
        //});

        ViewVocabResultDetail.main.SetVocabDetail(
            d
            , d.id
            , vocab.text
            , d.def
            , d.e1
            , d.t1
            , d.e2
            , d.t2);

        ViewVocabResultDetail.isDetailDone = true;
        yield return new WaitWhile(() =>
        {
            return ViewVocabResultDetail.isDetailDone == false;
        });

        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);
    }
}
