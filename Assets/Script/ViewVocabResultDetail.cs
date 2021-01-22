using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewVocabResultDetail : MonoBehaviour
{
    [SerializeField] public Text vocab;
    [SerializeField] public Text def;
    [SerializeField] public Text ex;
    [SerializeField] public VocabPanel vocabPanel;

    public static Action initVocabTestResultListCallBack = null;
    public static Action showBackButtonCallBack = null;

    public static ViewVocabResultDetail main = null;

    public static bool isDetailDone = false;
    void Start()
    {
        main = GetComponent<ViewVocabResultDetail>();
    }
    public void OnView()
    {
        showBackButtonCallBack();
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(true);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);
        
        StatusBar.statusBar.GetComponent<StatusBar>().ResetSelectedVocabScrollPos();

        initVocabTestResultListCallBack();
    }

    public void AfterView()
    {
        ViewVocabTestResult.showHomeButtonCallBack();
    }
    //public void SetVocabDetail(OX_DataLoader.VocabData data, int vocabId, string v, string d, string e1, string t1, string e2, string t2)
    public void SetVocabDetail(OX_DataLoader.VocabData data)
    {
        var vocabdata = UserDataManager.Instance.vocabData;
        vocab.text = data.vocab;
        vocabPanel.vocab = data.vocab;
        vocabPanel.vocabData = data;

        if (vocabdata.def.Equals("empty"))
        {
            return;
        }
        var deflist = vocabdata.def.Split(new string[] { "[t]" }, StringSplitOptions.None);

        def.text = string.Empty;
        for (int i = 0; i < deflist.Length; i++)
        {
            def.text += deflist[i];
            if (i == deflist.Length - 1)
            {
                break;
            }
            def.text += "\n";
        }

        var ee1 = OX_DataLoader.ColorVocab(vocabdata.e1.ToLower(), data.vocab);
        var tt1 = vocabdata.t1;
        var ee2 = OX_DataLoader.ColorVocab(vocabdata.e2.ToLower(), data.vocab);
        var tt2 = vocabdata.t2;

        ex.text = ee1
                  + "<size=25>" + "\n"
                  + tt1
                  + "</size>" + "\n\n"
                  + ee2
                  + "<size=25>" + "\n"
                  + tt2 + "</size>";

        vocabPanel.SetColor(false);

        bool isExist = UserDataManager.Instance.IsVocabExist(data.id);

        if (isExist)
        {
            vocabPanel.SetColor(true);
        }
    }
}
