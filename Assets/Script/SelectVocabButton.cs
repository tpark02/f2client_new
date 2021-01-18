using UnityEngine;
using UnityEngine.UI;

public class SelectVocabButton : MonoBehaviour
{
    public Text label;
    private string vocab;
    [HideInInspector] public bool isTestResult = false;
    public void SetSelectVocabButton(string s)
    {
        label.text = s;
        vocab = s;
    }

    public void OnClickSelectVocabButton()
    {
        var d = OX_DataLoader.GetVocab(vocab);
        if (isTestResult == false)
        {
            ViewVocabDetail.viewVocabDetail.GetComponent<ViewVocabDetail>().SetVocabDetail(d.id, vocab
                , d.def
                , d.e1
                , d.t1
                , d.e2
                , d.t2);
            StatusBar.statusBar.GetComponent<StatusBar>().OnClickSelectListButton();
        }
        else
        {
            ViewVocabResultDetail.viewResultDetail.GetComponent<ViewVocabResultDetail>().SetVocabDetail(d.id,vocab
                , d.def
                , d.e1
                , d.t1
                , d.e2
                , d.t2);
            StatusBar.statusBar.GetComponent<StatusBar>().OnClickSelectListButton();
        }
    }
}
