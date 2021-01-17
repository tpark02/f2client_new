using UnityEngine;

public class ViewHome : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        FileReadWrite.Instance.PrepareUserDataJson();
#if UNITY_EDITOR
        OX_DataLoader.InitOriginalData();
        //OX_DataLoader.TestMyList();
#endif
    }

    public void OnView()
    {
        StatusBar.SetStatusTitle((int) Title.HOME);
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);
    }

    public void OnClickVocab()
    {
        UnityEngine.Debug.Log("Vocab Touched !!!");
    }
}
