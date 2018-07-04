using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Twity.DataModels.Responses;
using Twity.DataModels.Core;

public class Twitwi : MonoBehaviour {

    public Text twetext;
    string[] onetime;
    string[] onetimeload = new string[100];
    string loadTex;
    string[] lasts;
    string[][] nouns = new string[100][];
    int nounnum;
    string[][] preps = new string[100][];
    int[] prepnum = new int[100];
    string tweetmesages;
    int i = 0;
    int count;
    int randnum;
    bool contenue;
    System.DateTime now;

    List<int> numList = new List<int>();  //使った単語の番号を入れるリスト

    //Dctionary<string, string> myTable = new Dictionary<string, string>();

    // Use this for initialization
    void Start () {
        tweetmesages = "";
        i = 0;
        count = 0;
        randnum = 0;
        contenue = true;
        First();
        twetext.text = "";
        //UnityEngine.Debug.Log(preps[3][3]);
    }

	// Update is called once per frame
	void Update () {
        now = System.DateTime.Now;
        if (Input.GetKeyDown("t")){
            contenue = true;
            CreateMes();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["status"] = tweetmesages;  // ツイートするテキスト
            StartCoroutine(Twity.Client.Post("statuses/update", parameters, this.Callback));
            Debug.Log(tweetmesages);
        }
    }

    void Callback(bool success, string response)
    {
        if (success)
        {
            //Twity.Tweet tweet = JsonUtility.FromJson<Twity.Tweet>(response); // 投稿したツイートが返ってくる
            UnityEngine.Debug.Log("success");
        }
        else
        {
            UnityEngine.Debug.Log(response);
        }
    }

    void First()
    {
        //Twitter認証
        Twity.Oauth.consumerKey = "";
        Twity.Oauth.consumerSecret = "";
        Twity.Oauth.accessToken = "";
        Twity.Oauth.accessTokenSecret = "";

        //文末取得
        loadTex = (Resources.Load("lastword") as TextAsset).text;
        lasts = loadTex.Split('\n');

        //名詞取得
        loadTex = (Resources.Load("noun") as TextAsset).text;
        onetimeload = loadTex.Split('\n');
        while (onetimeload[i] != "")
        {
            nouns[i] = onetimeload[i].Split(',');
            i++;
        }
        nounnum = i;
        i = 0;

        //前置詞取得
        loadTex = (Resources.Load("preposition") as TextAsset).text;
        onetimeload = loadTex.Split('\n');
        while (onetimeload[i] != "")
        {
            preps[i] = onetimeload[i].Split(',');
            prepnum[i] = preps[i].Length;
            i++;
        }
        i = 0;

    }

    void CreateMes()
    {
        numList.Clear();

        //タグ付け
        tweetmesages = "#にわかちゃれんじ\n";

        count = tweetmesages.Length;
        randnum = Random.Range(0, nounnum);
        Nwaka();

        while (contenue)
        {
            count = tweetmesages.Length;
            randnum = Random.Range(0, nounnum + 1);
            if (count > 30 || randnum == nounnum || numList.Contains(randnum)) contenue = false;
            else
            {
                Nwaka();
            }

        }
        tweetmesages += lasts[Random.Range(0, (lasts.Length - 1))].Trim();

        twetext.text = tweetmesages;
        //numList.Clear();

    }

    void Nwaka()
    {
        tweetmesages += nouns[randnum][0].Trim();
        numList.Add(randnum);
        switch (nouns[randnum][1].Trim())
        {
            case "person":
                tweetmesages += preps[0][Random.Range(0, prepnum[0])].Trim();
                //Debug.Log("person");
                break;
            case "place":
                tweetmesages += preps[1][Random.Range(0, prepnum[1])].Trim();
                //Debug.Log("place");
                break;
            case "food":
                tweetmesages += preps[2][Random.Range(0, prepnum[2])].Trim();
                //Debug.Log("");
                break;
            case "other":
                tweetmesages += preps[3][Random.Range(0, prepnum[3])].Trim();
                //Debug.Log("person");
                break;
        }
    }

    public void Challenge()
    {
        contenue = true;
        CreateMes();
        //Dictionary<string, string> parameters = new Dictionary<string, string>();
        //parameters["status"] = tweetmesages;  // ツイートするテキスト
        //StartCoroutine(Twity.Client.Post("statuses/update", parameters, this.Callback));
    }

}
