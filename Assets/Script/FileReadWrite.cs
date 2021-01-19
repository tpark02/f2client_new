using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[Serializable]
public class UserStudyVocab
{
    [SerializeField] public int vocabId;
    UserStudyVocab(int id)
    {
        vocabId = id;
    }
}

public class FileReadWrite : Singleton<FileReadWrite>
{

    private string userstudyvocabfilename = "UserStudyVocabData.json";

    public string GetStudyVocabFileName()
    {
        return userstudyvocabfilename;
    }
    
    public void PrepareUserDataJson()
    {
        string str = string.Empty;

        if (FileCheck(userstudyvocabfilename) == false)
        {
            InitUserDataFile(userstudyvocabfilename);
        }
        else
        {
            //str = ReadUserData(userstudyvocabfilename);
            //Dictionary<int, string> ulist = JsonUtility.FromJson<Serialization<int, string>>(str).ToDictionary();
            //UserDataManager.Instance.SetUserStudyVocabList(ulist);
        }
    }
    private void InitUserDataFile(string filename)
    {
        if (filename.Equals("UserStudyVocabData.json"))
        {

        }
       
        string str = string.Empty;

        if (filename.Equals("UserStudyVocabData.json"))
        {
            var list = UserDataManager.Instance.GetUserStudyVocabList();
            str = JsonUtility.ToJson(new Serialization<int, string>(list));
        }

        Debug.Log(str);
        CreateJsonFile(str, filename);
    }

    public void WriteUserData(string filename)
    {
        string str = string.Empty;

        if (filename.Equals("UserStudyVocabData.json"))
        {
            var list = UserDataManager.Instance.GetUserStudyVocabList();
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