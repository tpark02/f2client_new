using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct CurrentVocab
{
    public string def; 
    public string type; 
    public string e1; 
    public string t1; 
    public string e2; 
    public string t2;

    public CurrentVocab(string d, string t, string e1, string t1, string e2, string t2)
    {
        def = d.Equals("") ? "empty" : d;
        type = t.Equals("") ? "empty" : t;
        this.e1 = e1.Equals("") ? "empty" : e1;
        this.t1 = t1.Equals("") ? "empty" : t1;
        this.e2 = e2.Equals("") ? "empty" : e2;
        this.t2 = t2.Equals("") ? "empty" : t2;
    }
}

public class UserDataManager : Singleton<UserDataManager>
{
    private Dictionary<int, string> userStudyVocabList = new Dictionary<int, string>();
    private Dictionary<string, int> userNoteCount = new Dictionary<string, int>();
    public CurrentVocab vocabData = new CurrentVocab("empty", "empty", "empty", "empty", "empty", "empty");
    public string todaysExample = string.Empty;
    //public void SetUserStudyVocabList(Dictionary<int, string> d)
    //{
    //    userStudyVocabList = d;
    //}
    //public void DeleteUserStudyVocab(int s)
    //{
    //    userStudyVocabList.Remove(s);
    //}
    public void AddUserStudyVocab(int s, string noteName)
    {
        if (userStudyVocabList.ContainsKey(s) == false)
        {
            userStudyVocabList.Add(s, noteName);
        }
    }
    public bool IsVocabExist(int vocabId)
    {
        if (userStudyVocabList.ContainsKey(vocabId))
        {
            return true;
        }
        return false;
    }
    public Dictionary<int, string> GetUserStudyVocabList()
    {
        return userStudyVocabList;
    }

    public Dictionary<int, string> GetCurrentNoteVocabList()
    {
        Dictionary<int, string> l = new Dictionary<int, string>();

        foreach (var v in userStudyVocabList)
        {
            if (OX_DataLoader.currentNoteName.Equals(v.Value))
            {
                l.Add(v.Key, v.Value);
            }
        }

        return l;
    }
    public void AddUserNote(string noteName)
    {
        if (userNoteCount.ContainsKey(noteName) == false)
        {
            userNoteCount.Add(noteName, 0);
        }
    }

    public void InitUserNoteCount()
    {
        foreach (var v in userStudyVocabList)
        {
            if (userNoteCount.ContainsKey(v.Value))
            {
                userNoteCount[v.Value]++;
            }
        }
    }

    public bool CreateMyNote(string noteName)
    {
        if (userNoteCount.ContainsKey(noteName) == false)
        {
            userNoteCount.Add(noteName, 0);
            return true;
        }

        return false;
    }
    public void AddMyVocabUserNote(string noteName)
    {
        if (userNoteCount.ContainsKey(noteName))
        {
            userNoteCount[noteName]++;
        }
    }

    public void DeleteMyNote(string noteName)
    {
        if (userNoteCount.ContainsKey(noteName))
        {
            userNoteCount.Remove(noteName);
        }
        Dictionary<int, string> dic = new Dictionary<int, string>();
        
        foreach (var v in userStudyVocabList)
        {
            if (v.Value.Equals(noteName) == false)
            {
                dic.Add(v.Key, v.Value);
            }
        }

        userStudyVocabList = dic;
    }
    public void RemoveMyVocabUserNote(int vocabId)
    { 
        if (userStudyVocabList.ContainsKey(vocabId))
        {
            var noteName = userStudyVocabList[vocabId];
            if (userNoteCount.ContainsKey(noteName))
            {
                userNoteCount[noteName]--;
            }
            userStudyVocabList.Remove(vocabId);
        }
    }
    public int GetNoteCount(string s)
    {
        if (userNoteCount.ContainsKey(s))
        {
            return userNoteCount[s];
        }

        return -1;
    }

    public Dictionary<string, int> GetNoteList()
    {
        return userNoteCount;
    }
}
