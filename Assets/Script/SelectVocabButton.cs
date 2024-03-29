﻿using System.Collections;
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
        var d = OX_DataLoader.GetVocab(vocab);

        if (isMyList)
        {
            StartCoroutine(LoadVocabDetail(d.id));
            return;
        }
        if (isTestResult == false)
        {
            StartCoroutine(LoadVocabDetail(d.id));
        }
        else
        {
            StartCoroutine(LoadVocabDetail(d.id, true));
        }
    }

    public IEnumerator LoadMyVocabDetail(int vocabId)
    {
        StartCoroutine(NetWorkManager.Instance.GetVocabDetailCo(vocabId));
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });
        var data = OX_DataLoader.GetVocabDataById(vocabId);
        MyVocabDetail.main.SetVocabDetail(data);
        StatusBar.statusBar.GetComponent<StatusBar>().OnClickSelectListButton();
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
