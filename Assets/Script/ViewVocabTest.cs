using System;
using System.Collections;
using DG.Tweening;
using Doozy.Engine.Progress;
using UnityEngine;
using UnityEngine.UI;

public class ViewVocabTest : MonoBehaviour
{
    public static bool isVocabTestInProgress = false;
    public static Action showBackButtonCallBack = null;
    public static GameObject viewVocabTest = null;
    public static bool isTestLoadingDone = false;
    public static bool isChoiceAnimationDone = false;
    [SerializeField] public Image[] progressBox = new Image[10];
    [SerializeField] public ChoiceButton[] buttons = new ChoiceButton[4];
    [SerializeField] public GameObject progressPanel2;
    [SerializeField] public Text vocabText;
    [SerializeField] public Text debugNumberText;
    [SerializeField] public VocabPanel vocabPanel;

    private int answerChoiceIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        viewVocabTest = gameObject;
        ChoiceButton.getNextQuestionCallBack = GetNextVocabQuestion;
        ChoiceButton.moveProgressBarCallBack = MoveProgressBarAnim;
        ChoiceButton.showAnswerCallBack = ShowAnswer;

        foreach (var image in progressBox)
        {
            image.DOKill();
            image.color = new Color(255, 255, 255, 0);
        }

        buttons[0].buttonIndex = 0;
        buttons[1].buttonIndex = 1;
        buttons[2].buttonIndex = 2;
        buttons[3].buttonIndex = 3;
    }

    public void OnView()
    {
        showBackButtonCallBack();
        ChoiceButton.isClickable = true;
#if UNITY_EDITOR
        debugNumberText.gameObject.SetActive(true);
#else
        debugNumberText.gameObject.SetActive(false);
#endif
    }

    public void LoadTestList(int d)
    {
        isTestLoadingDone = false;

        OX_DataLoader.LoadVocabTest(d);
        OX_DataLoader.VocabTestShuffle();
        
        var s = OX_DataLoader.GetCurrentVocabQuestion();
        vocabText.text = s.vocab;
        vocabPanel.vocab = s.vocab;

        var choicedata = OX_DataLoader.GetCurrentAnswerChoice();
        
        foreach (var c in buttons)
        {
            c.Clear();
        }
        
        buttons[0].label.text = "     " + choicedata.c1;
        buttons[1].label.text = "     " + choicedata.c2;
        buttons[2].label.text = "     " + choicedata.c3;
        buttons[3].label.text = "     " + choicedata.c4;
        
        buttons[choicedata.answerIndex].isAnswer = true;
        
        isTestLoadingDone = true;
    }

    private void GetNextVocabQuestion()
    {
        var s = OX_DataLoader.GetCurrentVocabQuestion();
        vocabText.text = s.vocab;

        var choicedata = OX_DataLoader.GetCurrentAnswerChoice();

        foreach (var c in buttons)
        {
            c.Clear();
        }

        buttons[0].label.text = "     " + choicedata.c1;
        buttons[1].label.text = "     " + choicedata.c2;
        buttons[2].label.text = "     " + choicedata.c3;
        buttons[3].label.text = "     " + choicedata.c4;

        buttons[choicedata.answerIndex].isAnswer = true;
    }

    public void ShowAnswer(bool isCorrect, int buttonIndex)
    {
        StartCoroutine(ShowAnswerAnim(isCorrect, buttonIndex));
    }

    IEnumerator ShowAnswerAnim(bool isCorrect, int buttonIndex)
    {
        isChoiceAnimationDone = false;

        if (isCorrect)
        {
            buttons[buttonIndex].GetComponent<Image>().color = OX_DataLoader.green;
        }
        else
        {
            buttons[buttonIndex].GetComponent<Image>().color = OX_DataLoader.red;
        }

        yield return new WaitForSeconds(1f);

        buttons[buttonIndex].GetComponent<Image>().color = OX_DataLoader.white;     // button color reset

        isChoiceAnimationDone = true;
    }

    public void MoveProgressBarAnim()
    {
#if UNITY_EDITOR
        debugNumberText.text = OX_DataLoader.GetCurrentVocabTestIndex().ToString();
#endif
        float val = OX_DataLoader.GetVocabTestProgressValue();
        progressPanel2.GetComponent<Progressor>().SetValue(val);
    }
}
