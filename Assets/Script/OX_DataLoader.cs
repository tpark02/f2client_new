using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Doozy.Engine;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class OX_DataLoader
{
    public struct VocabData
    {
        public VocabData(int n
            , string v
            , string d
            , string ex1
            , string tr1
            , string ex2
            , string tr2
            , string s
            , string a
            , string type)
        {
            id = n;
            vocab = v;
            def = d;
            e1 = ex1;
            t1 = tr1;
            e2 = ex2;
            t2 = tr2;
            sym = s;
            aym = a;
            this.type = type;
            this.isCorrect = false;
            this.day = 0;
            this.isUserCheck = false;
        }

        void SetCorrect(bool b)
        {
            this.isCorrect = b;
        }
        public int id;
        public string vocab;
        public string def;
        public string e1;
        public string t1;
        public string e2;
        public string t2;
        public string sym;
        public string aym;
        public string type;
        public bool isCorrect;
        public int day;
        public bool isUserCheck;
    }

    public struct VocabAnswerChoiceData
    {
        public VocabAnswerChoiceData(int n)
        {
            answerIndex = 0;
            c1 = string.Empty;
            c2 = string.Empty;
            c3 = string.Empty;
            c4 = string.Empty;
        }
        public int answerIndex;
        public string c1;

        public string c2;

        public string c3;

        public string c4;

    }

    public static List<Dictionary<string, object>> originalData = null;
    public static System.Random
        random = new System.Random(); // 난수 생성은 한번만 해야, 같은 숫자가 생성안된다. 참고 : https://crynut84.tistory.com/15

    private static List<VocabData> vocablist = new List<VocabData>();
    public static string currentNoteName = string.Empty;
    public static int currentDay = 0;
    public static int eachDayVocabCount = 30;
    private static int vocabTestSize = 10;
    private static List<VocabData> testList = new List<VocabData>();
    private static int vocabTestIndex = 0;
    public static Color32 green = new Color32(55, 71, 79, 180);     // green
    public static Color32 red = new Color32(255, 0, 0, 180);        // red
    public static Color32 grey = new Color32(70, 70, 70, 180);
    public static Color32 white = new Color32(255, 255, 255, 180);
    public static List<VocabData> resultList = new List<VocabData>();
    public static void InitOriginalData() // Start is called before the first frame update
    {
        string path = "Data/testingCSV";
        originalData = CSVReader.Read(path);

        if (originalData.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red> OX Original Data is not Loaded !!!</color>");
        }
    }

    public static void InitVocabList(int d)
    {
        string errorStr = string.Empty;

        //if (vocablist == null)
        //{
        //    vocablist = new List<VocabData>();
        //}
        vocablist.Clear();
        //for (var i = 0; i < originalData.Count; i++)
        //for (var i = 0; i < 1000; i++)
        for (int i = d * eachDayVocabCount; i < (d * eachDayVocabCount) + eachDayVocabCount; i++)
        {
            VocabData v = new VocabData(-1
                , "empty"
                , "empty"
                , "empty"
                , "empty"
                , "empty"
                , "empty"
                , "empty"
                , "empty"
                , "empty");

            v.id = ((int)originalData[i]["id"]);
            v.vocab = CheckDataEmpty((string)originalData[i]["vocab"]);
            v.def = CheckDataEmpty((string)originalData[i]["def"]);
            v.e1 = CheckDataEmpty((string)originalData[i]["e1"]);
            v.t1 = CheckDataEmpty((string)originalData[i]["t1"]);
            v.e2 = CheckDataEmpty((string)originalData[i]["e2"]);
            v.t2 = CheckDataEmpty((string)originalData[i]["t2"]);
            v.sym = CheckDataEmpty((string)originalData[i]["sym"]);
            v.aym = CheckDataEmpty((string)originalData[i]["aym"]);
            v.type = CheckDataEmpty((string)originalData[i]["ps"]);
            v.day = (i / eachDayVocabCount) + 1;
            v.isUserCheck = false;

            v.e1 = ColorVocab(v.e1.ToLower(), v.vocab);
            v.e2 = ColorVocab(v.e2.ToLower(), v.vocab);

            if (v.e1[0] != '<')
            {
                v.e1 = Char.ToUpper(v.e1[0]) + v.e1.Substring(1);
            }

            if (v.e2[0] != '<')
            {
                v.e2 = Char.ToUpper(v.e2[0]) + v.e2.Substring(1);
            }

            v.e1 = v.e1.Trim();
            v.e2 = v.e2.Trim();
#if DEBUG
            if (v.e1.Contains("<color") == false || v.e2.Contains("<color") == false)
            {
                Debug.Log("error e1: " + v.vocab);
                errorStr += v.vocab + "\n";
            }
#endif
            vocablist.Add(v);
        }
#if DEBUG
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\test\error.txt"))
        {
            file.WriteLine(errorStr);
        }
#endif
    }

    public static  void SetMyVocab()
    {
        
    }
    private static string ColorVocab(string e, string v)
    {
        string example = string.Empty;
        var ex1 = e.Split(new string[] { " " }, StringSplitOptions.None);
        for (int i = 0; i < ex1.Length; i++)
        {
            if (ex1[i].Equals(v)
                || ex1[i].Equals(v + "ed")      // entitled
                || ex1[i].Equals(v + "d")       
                || ex1[i].Equals(v + "ned")     // banned
                || ex1[i].Equals(v + "s")
                || ex1[i].Equals(v + "ing")
                || ex1[i].Equals(v + "es"))
            {
                example += "<color=red>" + ex1[i] + "</color>";
            }
            else if (ex1[i].Equals(v + "d,"))
            {
                example += "<color=red>" + v + "d" + "</color>,";
            }
            else if (ex1[i].Equals(v + "ed."))
            {
                example += "<color=red>" + v + "ed" + "</color>.";
            }
            else if (ex1[i].Equals(v + "es."))
            {
                example += "<color=red>" + v + "es" + "</color>.";
            }
            else if (ex1[i].Equals(v + "?"))
            {
                example += "<color=red>" + v + "</color>?";
            }
            else if (ex1[i].Equals(v + "s?"))
            {
                example += "<color=red>" + v + "s" + "</color>?";
            }
            else if (ex1[i].Equals(v + "."))
            {
                example += "<color=red>" + v + "</color>.";
            }
            else if (ex1[i].Equals(v + "s."))
            {
                example += "<color=red>" + v + "s" + "</color>.";
            }
            else if (ex1[i].Equals(v + ","))
            {
                example += "<color=red>" + v + "</color>,";
            }
            else if (ex1[i].Equals(v + "ed,"))
            {
                example += "<color=red>" + v + "</color>,";
            }
            else if (ex1[i].Equals(v + "s,"))
            {
                example += "<color=red>" + v + "s" + "</color>,";
            }
            else if (ex1[i].Equals(v + "!"))
            {
                example += "<color=red>" + v + "</color>!";
            }
            else if (ex1[i].Equals("accordingto"))      // according to
            {
                example += "<color=red>" + "according to" + "</color>";
            }
            else if (ex1[i].Equals("oceanliner"))
            {
                example += "<color=red>" + "ocean liner" + "</color>";
            }
            else if (ex1[i].Equals("zipcode"))
            {
                example += "<color=red>" + "zip code" + "</color>";
            }
            else if (ex1[i].Equals("politicalfigure."))
            {
                example += "<color=red>" + "political figure" + "</color>.";
            }
            else if (ex1[i].Equals("politicalfigures"))
            {
                example += "<color=red>" + "political figures" + "</color>";
            }
            else if (ex1[i].Equals("inspiteof"))
            {
                example += "<color=red>" + "in spite of" + "</color>";
            }
            else if (ex1[i].Equals("humanrightsactivist"))      // human rights activist
            {
                example += "<color=red>" + "human rights activist" + "</color>";
            }
            else if (ex1[i].Equals("foreignaffairs"))
            {
                example += "<color=red>" + "foreign affairs" + "</color>";
            }
            else if (ex1[i].Equals("nucleicacid") || ex1[i].Equals("nucleicacids"))
            {
                example += "<color=red>" + "nucleic acid" + "</color>";
            }
            else if (ex1[i].Equals("geneticinformation"))
            {
                example += "<color=red>" + "genetic information" + "</color>";
            }
            else if (ex1[i].Equals("genetic-information"))
            {
                example += "<color=red>" + "Genetic information" + "</color>";
            }
            else if (ex1[i].Equals("wind-chill-factor"))
            {
                example += "<color=red>" + "Windchill factor" + "</color>";
            }
            else if (ex1[i].Equals("pointofview"))
            {
                example += "<color=red>" + "point of view" + "</color>";
            }
            else if (ex1[i].Equals("rainyseason,"))
            {
                example += "<color=red>" + "rainy season" + "</color>,";
            }
            else if (ex1[i].Equals("rainy-season"))
            {
                example += "<color=red>" + "Rainy season" + "</color>";
            }
            else if (ex1[i].Equals("hiding-place."))
            {
                example += "<color=red>" + "hiding place" + "</color>.";
            }
            else if (ex1[i].Equals("hidingplace"))
            {
                example += "<color=red>" + "hiding place" + "</color>";
            }
            else if (ex1[i].Equals("unionmember,"))
            {
                example += "<color=red>" + "union member" + "</color>,";
            }
            else if (ex1[i].Equals("union-members"))
            {
                example += "<color=red>" + "Union members" + "</color>";
            }
            else if (ex1[i].Equals("civilservice"))
            {
                example += "<color=red>" + "civil service" + "</color>";
            }
            else if (ex1[i].Equals("humanbeings"))
            {
                example += "<color=red>" + "human beings" + "</color>";
            }
            else if (ex1[i].Equals("growup"))
            {
                example += "<color=red>" + "grow up" + "</color>";
            }
            else if (ex1[i].Equals("leaveamanbehind."))
            {
                example += "<color=red>" + "leave" + "</color>" + "a man " + "<color=red>" + "behind" + "</color>.";
            }
            else if (ex1[i].Equals("leaveyourluggageorpersonalbelongingsbehind"))
            {
                example += "<color=red>" + "leave " + "</color>" + "your luggage or personal belongings "+ "<color=red>" + "behind" + "</color>";
            }
            else if (ex1[i].Equals("blood-pressure"))
            {
                example += "<color=red>" + "Blood pressure" + "</color>";
            }
            else if (ex1[i].Equals("bloodpressure"))
            {
                example += "<color=red>" + "blood pressure" + "</color>";
            }
            else if (ex1[i].Equals("townoffice"))
            {
                example += "<color=red>" + "town office" + "</color>";
            }
            
            else if (ex1[i].Equals("zipcode."))
            {
                example += "<color=red>" + "zip code" + "</color>.";
            }
            else if (v.Equals("engage") && ex1[i].Equals("engaging"))      
            {
                example += "<color=red>" + "engaging" + "</color>";
            }
            else if (v.Equals("accompany") && ex1[i].Equals("accompanied"))      
            {
                example += "<color=red>" + "accompanied" + "</color>";
            }
            else if (v.Equals("intensify") && ex1[i].Equals("intensifies"))      
            {
                example += "<color=red>" + "intensifies" + "</color>";
            }
            else if (v.Equals("thrive") && ex1[i].Equals("thriving"))
            {
                example += "<color=red>" + "thriving" + "</color>";
            }
            else if (v.Equals("adversity") && ex1[i].Equals("adversities"))
            {
                example += "<color=red>" + "adversities" + "</color>";
            }
            else if (v.Equals("processing") && ex1[i].Equals("image-processing"))
            {
                example +=  "image-" + "<color=red>" + "processing" + "</color>";
            }
            else if (v.Equals("priority") && ex1[i].Equals("priorities"))
            {
                example += "<color=red>" + "priorities" + "</color>";
            }
            else if (v.Equals("emergency") && ex1[i].Equals("emergencies"))
            {
                example += "<color=red>" + "emergencies" + "</color>";
            }
            else if (v.Equals("ship") && ex1[i].Equals("shipped"))
            {
                example += "<color=red>" + "shipped" + "</color>";
            }
            else if (v.Equals("thrive") && ex1[i].Equals("thriving."))
            {
                example += "<color=red>" + "thriving" + "</color>.";
            }
            else if (v.Equals("commodity") && ex1[i].Equals("commodities"))
            {
                example += "<color=red>" + "commodities" + "</color>";
            }
            else if (v.Equals("achieve") && ex1[i].Equals("achieving"))
            {
                example += "<color=red>" + "achieving" + "</color>";
            }
            else if (v.Equals("capability") && ex1[i].Equals("capabilities."))
            {
                example += "<color=red>" + "achieving" + "</color>";
            }
            else if (v.Equals("relocate") && ex1[i].Equals("relocating"))
            {
                example += "<color=red>" + "Relocating" + "</color>";
            }
            else if (v.Equals("encourage") && ex1[i].Equals("encouraging"))
            {
                example += "<color=red>" + "encouraging" + "</color>";
            }
            else if (v.Equals("evaluate") && ex1[i].Equals("evaluated."))
            {
                example += "<color=red>" + "evaluated" + "</color>.";
            }
            else if (v.Equals("encouragement") && ex1[i].Equals("encouragement!"))
            {
                example += "<color=red>" + "encouragement!" + "</color>!";
            }
            else if (v.Equals("opportunity") && ex1[i].Equals("opportunities"))
            {
                example += "<color=red>" + "opportunities" + "</color>";
            }
            else if (v.Equals("study") && ex1[i].Equals("studies"))
            {
                example += "<color=red>" + "studies" + "</color>";
            }
            else if (v.Equals("restore") && ex1[i].Equals("restoring"))
            {
                example += "<color=red>" + "restoring" + "</color>";
            }
            else if (v.Equals("recycle") && ex1[i].Equals("recycled"))
            {
                example += "<color=red>" + "recycled" + "</color>";
            }
            else if (v.Equals("vary") && ex1[i].Equals("varies"))
            {
                example += "<color=red>" + "varies" + "</color>";
            }
            else if (v.Equals("deplete") && ex1[i].Equals("depleting"))
            {
                example += "<color=red>" + "depleting" + "</color>";
            }
            else if (v.Equals("prescribe") && ex1[i].Equals("prescribing"))
            {
                example += "<color=red>" + "prescribing" + "</color>";
            }
            else if (v.Equals("inquisition") && ex1[i].Equals("inquisition's"))
            {
                example += "<color=red>" + "inquisition's" + "</color>";
            }
            else if (v.Equals("facility") && ex1[i].Equals("facilities"))
            {
                example += "<color=red>" + "facilities" + "</color>";
            }
            else if (v.Equals("ambiguity") && ex1[i].Equals("ambiguities-c"))
            {
                example += "<color=red>" + "Ambiguities" + "</color>";
            }
            
            else
            {
                example += ex1[i];
            }
            if (i == ex1.Length - 1)
            {
                break;
            }
            example += " ";
        }

        return example;
    }
    private static string CheckDataEmpty(string s)
    {
        if (s.Equals(""))
        {
            return "empty";
        }

        return s.Trim();
    }
   
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static VocabData GetVocab(string s)
    {
        foreach (var v in vocablist)
        {
            if (v.vocab.Equals(s))
            {
                return v;
            }
        }
        VocabData empty = new VocabData();
        empty.vocab = "empty";
        return empty;
    }
    
    public static string GetVocabById(int n)
    {
        foreach (var d in originalData)
        {
            if ((int) d["id"] == n)
                return (string) d["vocab"];

        }
        return string.Empty;
    }

    public static VocabData GetVocabDataByVocab(string s)
    {
        foreach (var d in originalData)
        {
            if (s.Equals((string) d["vocab"]))
            {
                VocabData v = new VocabData(-1
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty");
                v.id = ((int)d["id"]);
                v.vocab = CheckDataEmpty((string)d["vocab"]);
                v.def = CheckDataEmpty((string)d["def"]);
                v.e1 = CheckDataEmpty((string)d["e1"]);
                v.t1 = CheckDataEmpty((string)d["t1"]);
                v.e2 = CheckDataEmpty((string)d["e2"]);
                v.t2 = CheckDataEmpty((string)d["t2"]);
                v.sym = CheckDataEmpty((string)d["sym"]);
                v.aym = CheckDataEmpty((string)d["aym"]);
                v.type = CheckDataEmpty((string)d["ps"]);
                //v.day = (i / eachDayVocabCount) + 1;
                v.isUserCheck = false;

                v.e1 = ColorVocab(v.e1.ToLower(), v.vocab);
                v.e2 = ColorVocab(v.e2.ToLower(), v.vocab);

                if (v.e1[0] != '<')
                {
                    v.e1 = Char.ToUpper(v.e1[0]) + v.e1.Substring(1);
                }

                if (v.e2[0] != '<')
                {
                    v.e2 = Char.ToUpper(v.e2[0]) + v.e2.Substring(1);
                }

                v.e1 = v.e1.Trim();
                v.e2 = v.e2.Trim();
                return v;
            }
        }
        VocabData enpmtydata = new VocabData(-1
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty");

        return enpmtydata;
    }

    public static VocabData GetVocabDataById(int n)
    {
        foreach (var d in originalData)
        {
            if ((int) d["id"] == n)
            {
                VocabData v = new VocabData(-1
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty"
                    , "empty");
                v.id = ((int)d["id"]);
                v.vocab = CheckDataEmpty((string)d["vocab"]);
                v.def = CheckDataEmpty((string)d["def"]);
                v.e1 = CheckDataEmpty((string)d["e1"]);
                v.t1 = CheckDataEmpty((string)d["t1"]);
                v.e2 = CheckDataEmpty((string)d["e2"]);
                v.t2 = CheckDataEmpty((string)d["t2"]);
                v.sym = CheckDataEmpty((string)d["sym"]);
                v.aym = CheckDataEmpty((string)d["aym"]);
                v.type = CheckDataEmpty((string)d["ps"]);
                //v.day = (i / eachDayVocabCount) + 1;
                v.isUserCheck = false;

                v.e1 = ColorVocab(v.e1.ToLower(), v.vocab);
                v.e2 = ColorVocab(v.e2.ToLower(), v.vocab);

                if (v.e1[0] != '<')
                {
                    v.e1 = Char.ToUpper(v.e1[0]) + v.e1.Substring(1);
                }

                if (v.e2[0] != '<')
                {
                    v.e2 = Char.ToUpper(v.e2[0]) + v.e2.Substring(1);
                }

                v.e1 = v.e1.Trim();
                v.e2 = v.e2.Trim();
                return v;
            }

        }

        VocabData enpmtydata = new VocabData(-1
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty");

        return enpmtydata;
    }

    public static VocabData GetEmptyVocabData()
    {
        VocabData enpmtydata = new VocabData(-1
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty"
            , "empty");

        return enpmtydata;
    }
    public static void VocabTestShuffle()
    {
        int n = testList.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1); // 0보다 크거나 같고 MaxValue보다 작은 부호 있는 32비트 정수입니다.
            var value = testList[k];
            testList[k] = testList[n];
            testList[n] = value;
        }
    }

    public static void LoadVocabTest(int d)
    {
        testList.Clear();
        
        vocabTestIndex = 0;

        //for (int i = d * eachDayVocabCount; i < (d * eachDayVocabCount) + vocabTestSize; i++)
        for (int i = 0; i < vocablist.Count; i++)
        {
            var s = vocablist[i];
            Debug.Log(" ox_dataloader.cs : Test " + d.ToString() + " day, " + "vocab - " + s.vocab);
            testList.Add(vocablist[i]);
        }
    }

    public static VocabData GetCurrentVocabQuestion()
    {
        return testList[vocabTestIndex];
    }

    public static VocabAnswerChoiceData GetCurrentAnswerChoice()
    {
        List<int> indexlist = new List<int>();

        while (indexlist.Count < 3)
        {
            int rInt = random.Next(0, vocabTestSize);
            if (vocabTestIndex != rInt && indexlist.Contains(rInt) == false)
            {
                indexlist.Add(rInt);
            }
        }
        indexlist.Add(vocabTestIndex);

        int n = 4;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1); // 0보다 크거나 같고 MaxValue보다 작은 부호 있는 32비트 정수입니다.
            var value = indexlist[k];
            indexlist[k] = indexlist[n];
            indexlist[n] = value;
        }

        VocabAnswerChoiceData data = new VocabAnswerChoiceData(0);

        var l1 = testList[indexlist[0]].def.Split(new string[] { "[t]" }, StringSplitOptions.None);
        var l2 = testList[indexlist[1]].def.Split(new string[] { "[t]" }, StringSplitOptions.None);
        var l3 = testList[indexlist[2]].def.Split(new string[] { "[t]" }, StringSplitOptions.None);
        var l4 = testList[indexlist[3]].def.Split(new string[] { "[t]" }, StringSplitOptions.None);
        
        data.c1 = l1[0];
        data.c2 = l2[0];
        data.c3 = l3[0];
        data.c4 = l4[0];

        for (int i = 0; i < indexlist.Count; i++)
        {
            if (indexlist[i] == vocabTestIndex)
            {
                data.answerIndex = i;
                break;
            }
        }
        return data;
    }

    public static int GetCurrentVocabTestIndex()
    {
        return vocabTestIndex;
    }
    public static int IncreaseVocabTestIndex()
    {
        vocabTestIndex++;
        return vocabTestIndex;
    }

    public static bool IsVocabTestFinished()
    {
        return vocabTestIndex >= vocabTestSize - 1 ? true : false;
    }

    public static float GetVocabTestProgressValue()
    {
        return (float) (vocabTestIndex + 1f) / vocabTestSize;
    }

    public static void SetVocabTestResult(bool isAnswer)
    {
        var d = testList[vocabTestIndex];
        d.isCorrect = isAnswer;
        resultList.Add(d);
    }

    //public static void AddToUserList(int vocabId, string noteName)
    //{
    //    UserDataManager.Instance.AddUserStudyVocab(vocabId, noteName);

    //    string filename = FileReadWrite.Instance.GetStudyVocabFileName();
    //    FileReadWrite.Instance.WriteUserData(filename);
    //}

    //public static void RemoveFromUserList(int vocabId)
    //{
    //    var isVocabExist = UserDataManager.Instance.IsVocabExist(vocabId);
        
    //    if (isVocabExist)
    //    {
    //        UserDataManager.Instance.DeleteUserStudyVocab(vocabId);
    //    }

    //    string filename = FileReadWrite.Instance.GetStudyVocabFileName();
    //    FileReadWrite.Instance.WriteUserData(filename);
    //}
    public static List<VocabData> GetCurrentDayVocabList()
    {
        return vocablist;
    }
    public static VocabData GetCurrentDayVocabDataByIndex(int index)
    {
        return vocablist[index];
    }
}
