﻿using System;
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
    public void SetVocabDetail(OX_DataLoader.VocabData data, int vocabId, string v, string d, string e1, string t1, string e2, string t2)
    {
        var deflist = d.Split(new string[] { "[t]" }, StringSplitOptions.None);
        vocab.text = v;
        vocabPanel.vocab = v;
        //vocabPanel.vocabId = vocabId;
        vocabPanel.vocabData = data;

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

        //bool isVocabExist = UserDataManager.Instance.IsVocabExist(vocab.text);
        //if (isVocabExist)
        //{
        //    vocabPanel.SetColor(true);
        //}

        bool isExist = UserDataManager.Instance.IsVocabExist(vocabId);

        if (isExist)
        {
            vocabPanel.SetColor(true);
        }
    }
}
