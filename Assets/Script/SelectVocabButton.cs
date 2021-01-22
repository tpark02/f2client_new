using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class SelectVocabButton : MonoBehaviour
{
    public Text label;
    private string vocab;
    private bool isDetailLoadingDone = false;
    [HideInInspector] public bool isTestResult = false;
    [HideInInspector] public bool isMyList = false;
    public void SetSelectVocabButton(string s)
    {
        label.text = s;
        vocab = s;
    }

    public void OnClickSelectVocabButton()
    {
       
        if (isMyList)
        {
            var data = OX_DataLoader.GetVocabDataByVocab(vocab);
            MyVocabDetail.myVocabDetail.GetComponent<MyVocabDetail>().SetVocabDetail(data, vocab
                , data.def
                , data.e1
                , data.t1
                , data.e2
                , data.t2);
            StatusBar.statusBar.GetComponent<StatusBar>().OnClickSelectListButton();
            return;
        }

        var d = OX_DataLoader.GetVocab(vocab);
        if (isTestResult == false)
        {
            StartCoroutine(LoadVocabDetail(d.id));
        }
        else
        {
            //ViewVocabResultDetail.main.GetComponent<ViewVocabResultDetail>().SetVocabDetail(d.id);
            StartCoroutine(LoadVocabDetail(d.id, true));
        }
    }
    public IEnumerator LoadVocabDetail(int vocabId, bool isTestReslt = false)
    {
        StartCoroutine(NetWorkManager.Instance.GetVocabDetailCo(vocabId));
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });

        var data = OX_DataLoader.GetVocabDataById(vocabId);
        if (isTestReslt == false)
        {
            ViewVocabDetail.main.SetVocabDetail(data);
        }
        else
        {
            ViewVocabResultDetail.main.SetVocabDetail(data);
        }
        StatusBar.statusBar.GetComponent<StatusBar>().OnClickSelectListButton();
    }
}
