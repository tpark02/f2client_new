using System;
using System.Collections;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public static Action getNextQuestionCallBack = null;
    public static Action moveProgressBarCallBack = null;
    public static Action<bool, int> showAnswerCallBack = null;
    public Text label;
    public int buttonIndex = 0;
    public bool isAnswer = false;

    public static bool isClickable = true;

    // Start is called before the first frame update
    public void Clear()
    {
        label.text = string.Empty;
        isAnswer = false;
    }

    public void OnClickChoiceButton()
    {
        if (isClickable == false)
        {
            Debug.Log("Still click in progress !!!");
            return;
        }

        if (isAnswer)
        {
            Debug.Log("Answer !!!");
        }
        else
        {
            Debug.Log("False !!!");
        }
        OX_DataLoader.SetVocabTestResult(isAnswer);
        StartCoroutine(ClickChoiceButtonAnim());
    }

    IEnumerator ClickChoiceButtonAnim()
    {
        isClickable = false;

        moveProgressBarCallBack();

        showAnswerCallBack(isAnswer, buttonIndex);

        yield return new WaitWhile(() =>
        {
            return ViewVocabTest.isChoiceAnimationDone == false;
        });
        
        if (OX_DataLoader.IsVocabTestFinished())
        {
            Debug.Log("<color=yellow>Vocab Test Finished !!!</color>");
            GameEventMessage.SendEvent("VocabTestFinish");
            yield break;
        }

        OX_DataLoader.IncreaseVocabTestIndex();
        getNextQuestionCallBack();

        isClickable = true;
    }
}
