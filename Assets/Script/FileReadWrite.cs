using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[Serializable]
public class UserStudyVocab
{
    [SerializeField] public string vocab;
    UserStudyVocab(string s)
    {
        vocab = s;
    }
}

[Serializable]
public class UserData
{
    [SerializeField] public int dayId;
    [SerializeField] public string isUnlock;
    [SerializeField] public string isCheck;
}

[Serializable]
public class OXUserData : UserData
{

    public OXUserData(int n, string s, string unlock)
    {
        dayId = n;
        isUnlock = s;
        isCheck = unlock;
    }
}
[Serializable]
public class SentenceUserData : UserData
{
    public SentenceUserData(int n, string s, string unlock)
    {
        dayId = n;
        isUnlock = s;
        isCheck = unlock;
    }
}

public class FileReadWrite : Singleton<FileReadWrite>
{
    // Use this for initialization
    private string oxfilename = "OXUserData.json";
    private string sentencefilename = "SentenceUserData.json";
    private string userstudyvocabfilename = "UserStudyVocabData.json";
    private string userStudySentenceFileName = "UserStudySentenceData.json";

    public string GetStudySentencefileName()
    {
        return userStudySentenceFileName;
    }
    public string GetStudyVocabFileName()
    {
        return userstudyvocabfilename;
    }
    public string GetSentenceFileName()
    {
        return sentencefilename;
    }
    public string GetOXFileName()
    {
        return oxfilename;
    }
    public void PrepareUserDataJson()
    {
        string str = string.Empty;

        if (FileCheck(oxfilename) == false)
        {
            InitUserDataFile(oxfilename);
        }
        else
        {
            str = ReadUserData(oxfilename);
            if (str.Equals(""))
            {
                Debug.Log("<color=red>" + oxfilename + " is empty. </color>");
                return;
            }
            Dictionary<int, OXUserData> oxrlist = JsonUtility.FromJson<Serialization<int, OXUserData>>(str).ToDictionary();
            UserDataManager.Instance.SetOXProgressList(oxrlist);
        }

        if (FileCheck(sentencefilename) == false)
        {
            InitUserDataFile(sentencefilename);
        }
        else
        {
            str = ReadUserData(sentencefilename);
            if (str.Equals(""))
            {
                Debug.Log("<color=red>" + sentencefilename + " is empty. </color>");
                return;
            }
            Dictionary<int, SentenceUserData> xrlist = JsonUtility.FromJson<Serialization<int, SentenceUserData>>(str).ToDictionary();
            UserDataManager.Instance.SetSentenceProgressList(xrlist);
        }

        if (FileCheck(userstudyvocabfilename) == false)
        {
            InitUserDataFile(userstudyvocabfilename);
        }
        else
        {
            str = ReadUserData(userstudyvocabfilename);
            Dictionary<string, string> ulist = JsonUtility.FromJson<Serialization<string, string>>(str).ToDictionary();
            UserDataManager.Instance.SetUserStudyVocabList(ulist);
        }

        if (FileCheck(userStudySentenceFileName) == false)
        {
            InitUserDataFile(userStudySentenceFileName);
        }
        else
        {
            str = ReadUserData(userStudySentenceFileName);
            //if (str.Equals(""))
            //{
            //    Debug.Log("<color=red>" + userStudySentenceFileName + " is empty. </color>");
            //    return;
            //}
            Dictionary<int, string> ulist = JsonUtility.FromJson<Serialization<int, string>>(str).ToDictionary();
            UserDataManager.Instance.SetUserSentenceList(ulist);
        }
    }
    private void InitUserDataFile(string filename)
    {
        if (filename.Equals("OXUserData.json"))
        {
            for (int i = 1; i < 21; i++)
            {
                OXUserData u = null;
                if (i == 1)
                {
                    u = new OXUserData(i, "true", "false");
                }
                else
                {
                    u = new OXUserData(i, "false", "false");

                }

                if (u != null)
                {
                    UserDataManager.Instance.AddUserOXProgressList(i, u);
                }
            }
        }

        if (filename.Equals("SentenceUserData.json"))
        {
            for (int i = 1; i < 21; i++)
            {
                SentenceUserData uu = null;
                if (i == 1)
                {
                    uu = new SentenceUserData(i, "true", "false");
                }
                else
                {
                    uu = new SentenceUserData(i, "false", "false");
                }
                if (uu != null)
                {
                    UserDataManager.Instance.AddSentenceUserProgressList(i, uu);
                }
            }
        }
        if (filename.Equals("UserStudyVocabData.json"))
        {

        }
        if (filename.Equals("UserStudySentenceData.json"))
        {

        }
        string str = string.Empty;
        if (filename.Equals("OXUserData.json"))
        {
            var list = UserDataManager.Instance.GetUserOXProgressList();
            str = JsonUtility.ToJson(new Serialization<int, OXUserData>(list));
        }
        if (filename.Equals("SentenceUserData.json"))
        {
            var list = UserDataManager.Instance.GetUserSentenceProgressList();
            str = JsonUtility.ToJson(new Serialization<int, SentenceUserData>(list));
        }
        if (filename.Equals("UserStudyVocabData.json"))
        {
            var list = UserDataManager.Instance.GetUserStudyVocabList();
            str = JsonUtility.ToJson(new Serialization<string, string>(list));
        }
        if (filename.Equals("UserStudySentenceData.json"))
        {
            var list = UserDataManager.Instance.GetUserSentenceList();
            str = JsonUtility.ToJson(new Serialization<int, string>(list));
        }
        Debug.Log(str);
        CreateJsonFile(str, filename);
    }

    public void WriteUserData(string filename)
    {
        string str = string.Empty;
        if (filename.Equals("OXUserData.json"))
        {
            var list = UserDataManager.Instance.GetUserOXProgressList();
            str = JsonUtility.ToJson(new Serialization<int, OXUserData>(list));
        }
        else if (filename.Equals("SentenceUserData.json"))
        {
            var list = UserDataManager.Instance.GetUserSentenceProgressList();
            str = JsonUtility.ToJson(new Serialization<int, SentenceUserData>(list));
        }
        else if (filename.Equals("UserStudyVocabData.json"))
        {
            var list = UserDataManager.Instance.GetUserStudyVocabList();
            str = JsonUtility.ToJson(new Serialization<string, string>(list));
        }
        else if (filename.Equals("UserStudySentenceData.json"))
        {
            var list = UserDataManager.Instance.GetUserSentenceList();
            str = JsonUtility.ToJson(new Serialization<int, string>(list));
        }
        CreateJsonFile(str, filename);
    }
    string ReadUserData(string filename)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.persistentDataPath + "/", filename), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        return Encoding.UTF8.GetString(data);
    }
    void CreateJsonFile(string str, string filename)
    {
        FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + filename);
        if (fi.Exists == false)
        {
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Create)))
            {
                txtWriter.Write(str);
            }
        }
        else
        {
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate)))
            {
                txtWriter.Write(str);
            }
        }
    }
    public bool FileCheck(string filename)
    {
        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
            Debug.Log("<color=blue>File Exists." + filename + "</color>");
            return true;
        }
        Debug.Log("<color=red>File Does Not Exist." + filename + "</color>");
        return false;
    }
}

[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary()
    {
        return target;
    }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}