using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private float userSentenceFinalScore = 0f;
    private float userOXFinalScore = 0f;
    private int userOXScore = 0;
    private int userSentenceScore = 0;
    private Dictionary<int, OXUserData> oxProgressList = new Dictionary<int, OXUserData>();
    private Dictionary<int, SentenceUserData> sentenceProgressList = new Dictionary<int, SentenceUserData>();
    private Dictionary<string, string> userStudyVocabList = new Dictionary<string, string>();
    private Dictionary<int, string> userSentenceList = new Dictionary<int, string>();

    public void SetUserSentenceList(Dictionary<int, string> d)
    {
        userSentenceList = d;
    }
    public bool IsSentenceDataExist(int n)
    {
        if (userSentenceList.ContainsKey(n))
        {
            return true;
        }
        return false;
    }
    public void DeleteUserSentenceList(int n)
    {
        userSentenceList.Remove(n);
    }
    public void AddUserSentenceList(int n)
    {
        userSentenceList.Add(n, string.Empty);
    }
    public Dictionary<int, string> GetUserSentenceList()
    {
        return userSentenceList;
    }
    public void SetUserStudyVocabList(Dictionary<string, string> d)
    {
        userStudyVocabList = d;
    }
    public void DeleteUserStudyVocab(string s)
    {
        userStudyVocabList.Remove(s);
    }
    public void AddUserStudyVocab(string s)
    {
        if (userStudyVocabList.ContainsKey(s) == false)
        {
            userStudyVocabList.Add(s, string.Empty);
        }
    }
    public bool IsVocabExist(string s)
    {
        if (userStudyVocabList.ContainsKey(s))
        {
            return true;
        }
        return false;
    }
    public Dictionary<string, string> GetUserStudyVocabList()
    {
        return userStudyVocabList;
    }
    public OXUserData GetOXProgressListByIndex(int n)
    {
        return oxProgressList[n];
    }

    public int OxProgressListTrueCount()
    {
        return oxProgressList.Count(i => i.Value.isUnlock.Equals("true"));
    }

    public int SentenceProgressListTrueCount()
    {
        return sentenceProgressList.Count(i => i.Value.isUnlock.Equals("true"));
    }
    public SentenceUserData GetSentenceProgressListByIndex(int n)
    {
        return sentenceProgressList[n];
    }
    public bool SetOXNextDayUnlock(int n)
    {
        if (n <= 0)
        {
            Debug.Log("<color=red>index is 0 !!!<color>");
            return false;
        }
        if (n >= 20)
        {
            Debug.Log("<color=red>All days are done !!!</color>");
            return false;
        }
        var d = oxProgressList[n];
        if (d.isCheck.Equals("true"))
        {
            oxProgressList[n + 1].isUnlock = "true";
            return true;
        }
        return false;
    }
    public bool SetSentenceNextDayUnlock(int n)
    {
        if (n <= 0)
        {
            Debug.Log("<color=red>index is 0 !!!<color>");
            return false;
        }
        if (n >= 20)
        {
            Debug.Log("<color=red>All days are done !!!</color>");
            return false;
        }
        var d = sentenceProgressList[n];
        if (d.isCheck.Equals("true"))
        {
            sentenceProgressList[n + 1].isUnlock = "true";
            return true;
        }
        return false;
    }
    public void SetUserOXDayResult(int n, float f)
    {
        var d = oxProgressList[n];
        if (f >= 70f)
        {
            //d.isUnlock = "true";
            d.isCheck= "true";            
        }
        else
        {
            //d.isUnlock = "false";
            d.isCheck = "false";
        }
    }
    public void SetUserSentenceDayResult(int n, float f)
    {
        var d = sentenceProgressList[n];
        if (f >= 70f)
        {
            //d.isUnlock = "true";
            d.isCheck = "true";
        }
        else
        {
            //d.isUnlock = "false";
            d.isCheck = "false";
        }
    }
    public void SetOXProgressList(Dictionary<int, OXUserData> list)
    {
        oxProgressList = list;
    }
    public void SetSentenceProgressList(Dictionary<int, SentenceUserData> list)
    {
        sentenceProgressList = list;
    }
    public void AddUserOXProgressList(int n, OXUserData d)
    {
        oxProgressList.Add(n, d);
    }
    public void AddSentenceUserProgressList(int n, SentenceUserData d)
    {
        sentenceProgressList.Add(n, d);
    }
    public Dictionary<int, OXUserData> GetUserOXProgressList()
    {
        return oxProgressList;
    }
    public Dictionary<int, SentenceUserData> GetUserSentenceProgressList()
    {
        return sentenceProgressList;
    }
    public void SetUserOXScore(int n)
    {
        userOXScore = Mathf.Max(n, userOXScore);
    }

    public void SetUserSentenceScore(int n)
    {
        userSentenceScore = Mathf.Max(n, userSentenceScore);
    }

    public void IncreaseUserOXScore()
    {
        userOXScore++;
    }

    public void IncreaseUserSentenceScore()
    {
        userSentenceScore++;
    }

    public void SetUserOXFinalScore(float f)
    {
        userOXFinalScore = f;
    }
    public void SetUserSentenceFinalScore(float f)
    {
        userSentenceFinalScore = f;
    }
    public float GetUserOXFinalScore()
    {
        return userOXFinalScore;
    }
}
