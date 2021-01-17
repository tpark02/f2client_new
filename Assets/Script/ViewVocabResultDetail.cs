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

    public static GameObject viewResultDetail = null;

    public static bool isDetailDone = false;
    void Start()
    {
        viewResultDetail = gameObject;
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
    public void SetVocabDetail(string v, string d, string e1, string t1, string e2, string t2)
    {
        var deflist = d.Split(new string[] { "[t]" }, StringSplitOptions.None);
        vocab.text = v;
        vocabPanel.vocab = v;

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

        ex.text = e1 + "<size=25>" + "\n" + t1 + "</size>" + "\n\n" + e2 + "<size=25>" + "\n" + t2 + "</size>";

        vocabPanel.SetColor(false);

        bool isVocabExist = UserDataManager.Instance.IsVocabExist(vocab.text);
        if (isVocabExist)
        {
            vocabPanel.SetColor(true);
        }
    }
}
