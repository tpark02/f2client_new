using UnityEngine;
using UnityEngine.UI;

public class VocabTouchNotice : MonoBehaviour
{
    [SerializeField] private Text label;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLabel(string s)
    {
        label.text = s;
    }
}
