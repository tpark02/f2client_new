using System;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewVocabDetail : MonoBehaviour
{
    [SerializeField] public Text vocab;
    [SerializeField] public Text def;
    [SerializeField] public Text ex;
    [SerializeField] public VocabPanel vocabPanel;
    public static Action showBackButtonCallBack = null;
    
    public static GameObject viewVocabDetail;

    public static bool isDetailDone = false;

    void Start()
    {
        viewVocabDetail = gameObject;
    }

    public void OnView()
    {
        showBackButtonCallBack();
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(true);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);
        
        StatusBar.statusBar.GetComponent<StatusBar>().ResetSelectedVocabScrollPos();
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

        ex.text = e1 + "<size=25>" + "\n" + t1 + "</size>" + "\n\n" + e2 + "<size=25>" +"\n" + t2 + "</size>";
        
        vocabPanel.SetColor(false);
        
        bool isVocabExist = UserDataManager.Instance.IsVocabExist(vocab.text);
        if (isVocabExist)
        {
            vocabPanel.SetColor(true);
        }
    }
}
