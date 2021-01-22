using System;
using UnityEngine;

public class ViewVocabTestResult : MonoBehaviour
{
    public static Action showHomeButtonCallBack = null;
    [SerializeField] VocabResultButton[] buttons = new VocabResultButton[10];
    // Start is called before the first frame update
    public void OnView()
    {
        StatusBar.RecordPrevTitle((int)Title.HOME);
        StatusBar.SetStatusTitle((int)Title.VOCAB_TEST_RESULT);

        for (int i = 0; i < buttons.Length; i++)
        {
            var d = OX_DataLoader.resultList[i];
            buttons[i].vocab.text = d.vocab;
            buttons[i].vocabData = d;

            if (d.isCorrect)
            {
                buttons[i].correct.SetActive(true);
                buttons[i].wrong.SetActive(false);
            }
            else
            {
                buttons[i].correct.SetActive(false);
                buttons[i].wrong.SetActive(true);
            }
        }

        showHomeButtonCallBack();
    }
}
