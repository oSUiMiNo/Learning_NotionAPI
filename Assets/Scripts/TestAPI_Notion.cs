using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

// このNotionページにDB置いてある。
//https://www.notion.so/6795dd70753a495897cb85e0abce95fe

public class TestAPI_Notion : MonoBehaviour
{
    private const string NotionAccessToken = "secret_GKXw55IIbLe0LkOPsWpt0IG8BCLfOokRhamOysuBhyW";
    private const string DatabaseID = "b40fd183c85b4a1da4fbb1c1beaaa6b2";

    void Start()
    {
        InputEventHandler.OnKeyDown_A += UseAPI_GET;
        InputEventHandler.OnKeyDown_B += UseAPI_POST;
        InputEventHandler.OnKeyDown_C += UseAPI_Add;
        InputEventHandler.OnKeyDown_D += UseAPI_DowiloadFile;
    }

    //C#からでもNotionのデータベースのオブジェクト取得できるけど、ネストしたJSONを解きほぐす作業とかめんどい
    //Json.net使ったら出来るみたいだけどめんどいので気が向いたら。
    async void UseAPI_GET()
    {
        string jsonStr = string.Empty;

        using (UnityWebRequest request = UnityWebRequest.Get($"https://api.notion.com/v1/databases/{DatabaseID}"))
        {
            request.SetRequestHeader("Authorization", $"Bearer {NotionAccessToken}");
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            request.SetRequestHeader("Notion-Version", "2022-02-22");

            await request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("リクエスト中");
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("リクエスト成功");
                    break;

                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log(
                        @"サーバとの通信に失敗。
                        リクエストが接続できなかった、
                        セキュリティで保護されたチャネルを確立できなかったなど。");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(
                        @"サーバがエラー応答を返した。
                        サーバとの通信には成功したが、
                        接続プロトコルで定義されているエラーを受け取った。");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(
                        @"データの処理中にエラーが発生。
                        リクエストはサーバとの通信に成功したが、
                        受信したデータの処理中にエラーが発生。
                        データが破損しているか、正しい形式ではないなど。");
                    Debug.LogError(request.error);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            jsonStr = request.downloadHandler.text;
        }
        
        JObject responseObj = JObject.Parse(jsonStr);

        Debug.Log($"レスポンスオブジェクト\n{responseObj}");
        Debug.Log($"プロパティ\n{responseObj["properties"]}");
        Debug.Log($"プロパティ_英語\n{responseObj["properties"]["英語"]}");

        // Linqで、あるブロック階層内での同じプロパティ名の値を全て取得してコレクションに格納する方法
        IEnumerable<JToken> i = responseObj["properties"]["タグ"]["multi_select"]["options"].Select(p => p["name"]);
        foreach(var a in i)
        {
            Debug.Log(a);
        }
    }


    async void UseAPI_POST()
    {
        WWWForm form = new WWWForm();

        string jsonStr = string.Empty;
        using (UnityWebRequest request = UnityWebRequest.Post($"https://api.notion.com/v1/databases/{DatabaseID}/query", form))
        {
            request.SetRequestHeader("Authorization", $"Bearer {NotionAccessToken}");
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            request.SetRequestHeader("Notion-Version", "2022-02-22");

            await request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("リクエスト中");
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("リクエスト成功");
                    break;

                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log(
                        @"サーバとの通信に失敗。
                        リクエストが接続できなかった、
                        セキュリティで保護されたチャネルを確立できなかったなど。");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(
                        @"サーバがエラー応答を返した。
                        サーバとの通信には成功したが、
                        接続プロトコルで定義されているエラーを受け取った。");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(
                        @"データの処理中にエラーが発生。
                        リクエストはサーバとの通信に成功したが、
                        受信したデータの処理中にエラーが発生。
                        データが破損しているか、正しい形式ではないなど。");
                    Debug.LogError(request.error);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            jsonStr = request.downloadHandler.text;
        }

        JObject responseObj = JObject.Parse(jsonStr);

        // レスポンスの中からresultsプロパティの中身を使う
        Debug.Log(responseObj["results"][0]);

        foreach (var a in responseObj["results"])
        {
            Debug.Log(a["properties"]);
        }
    }





    async void UseAPI_DowiloadFile()
    {
        WWWForm form = new WWWForm();

        string DatabaseID = "e8777c71cf694fa8bd135fb1d7cb1728";

        string jsonStr = string.Empty;
        using (UnityWebRequest request = UnityWebRequest.Post($"https://api.notion.com/v1/databases/{DatabaseID}/query", form))
        {
            request.SetRequestHeader("Authorization", $"Bearer {NotionAccessToken}");
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            request.SetRequestHeader("Notion-Version", "2022-02-22");

            await request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("リクエスト中");
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("リクエスト成功");
                    break;

                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log(
                        @"サーバとの通信に失敗。
                        リクエストが接続できなかった、
                        セキュリティで保護されたチャネルを確立できなかったなど。");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(
                        @"サーバがエラー応答を返した。
                        サーバとの通信には成功したが、
                        接続プロトコルで定義されているエラーを受け取った。");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(
                        @"データの処理中にエラーが発生。
                        リクエストはサーバとの通信に成功したが、
                        受信したデータの処理中にエラーが発生。
                        データが破損しているか、正しい形式ではないなど。");
                    Debug.LogError(request.error);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            jsonStr = request.downloadHandler.text;
        }

        JObject responseObj = JObject.Parse(jsonStr);

        // レスポンスの中からresultsプロパティの中身を使う
        //Debug.Log(responseObj["results"][0]);
        //foreach (var a in responseObj["results"])
        //{
        //    Debug.Log(a["properties"]);
        //}


        // データベースから取得したダウンロードリンク
        string downloadURL = responseObj["results"][0]["properties"]["ファイル"]["files"][0]["file"]["url"].ToString();
        DebugView.Log(downloadURL);

        // データベースから取得したファイル名
        string fileName = responseObj["results"][0]["properties"]["ファイル"]["files"][0]["name"].ToString();

        // 新規作成するファイルのパスと名前
        string newFilePath = @$"C:\Users\osuim\Downloads\{fileName}";

        byte[] results;

        //ファイルダウンロード
        using (UnityWebRequest request = new UnityWebRequest(downloadURL))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                // jsonとかのテキストファイルならそのまま得ることも可能
                Debug.Log(request.downloadHandler.text);

                // テキストファイル以外もダウンロードしたいのでバイナリデータから復元する方式にしたい
                results = request.downloadHandler.data;
                // バイトからファイル復元
                using (FileStream fs = new FileStream(newFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    // 新規ファイルにバイト配列の0文字目から最後まで書き込み
                    fs.Write(results, 0, results.Length);
                }
            }
        }
    }



    string P =
        "{\r\n  \"parent\": {\r\n    \"type\": \"database_id\",\r\n    \"database_id\": \"b40fd183c85b4a1da4fbb1c1beaaa6b2\"\r\n  },\r\n  \"properties\": {\r\n    \"名前\": {\r\n      \"title\": [\r\n        {\r\n          \"text\": {\r\n            \"content\": \"一条\"\r\n          }\r\n        }\r\n      ]\r\n    },\r\n    \"タグ\": {\r\n      \"type\": \"multi_select\",\r\n      \"multi_select\": [\r\n        {\r\n          \"name\": \"合格\"\r\n        }\r\n      ]\r\n    },\r\n    \"国語\": {\r\n      \"type\": \"number\",\r\n      \"number\": 30\r\n    },\r\n    \"数学\": {\r\n      \"type\": \"number\",\r\n      \"number\": 30\r\n    },\r\n    \"英語\": {\r\n      \"type\": \"number\",\r\n      \"number\": 30\r\n    },\r\n  }\r\n}";


    // パースエラー出て使えん
    async void UseAPI_Add()
    {
        NotionAdditionalData additionalData = new NotionAdditionalData()
        {
            databaseID = DatabaseID,
            score_English = 30,
            score_Japanese = 30,
            score_Math = 30,
            sum = 90,
            studentName = "一条",
            tag = new List<string>
            {
               "合格"
            }
        };
        additionalData.SetProps();
        string payload = JsonConvert.SerializeObject(additionalData, Formatting.Indented);

        DebugView.Log($"{payload}");

        JObject jsonObject = JObject.Parse(payload);
        //DebugView.Log($"{jsonObject["properties"]["数学"]["number"]}");
        string payload0 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        DebugView.Log($"{payload0}");

        DebugView.Log(P);

        UnityWebRequest request = UnityWebRequest.Post($"https://api.notion.com/v1/pages", payload);
        request.SetRequestHeader("Authorization", $"Bearer {NotionAccessToken}");
        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Notion-Version", "2022-02-22");
       

        await request.SendWebRequest();



        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(
                    @"サーバとの通信に失敗。
                        リクエストが接続できなかった、
                        セキュリティで保護されたチャネルを確立できなかったなど。");
                Debug.LogError(request.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"サーバがエラー応答を返した。
                        サーバとの通信には成功したが、
                        接続プロトコルで定義されているエラーを受け取った。");
                Debug.LogError(request.error);
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"データの処理中にエラーが発生。
                        リクエストはサーバとの通信に成功したが、
                        受信したデータの処理中にエラーが発生。
                        データが破損しているか、正しい形式ではないなど。");
                Debug.LogError(request.error);
                break;

            default: throw new ArgumentOutOfRangeException();
        }
    }
}






[System.Serializable]
class NotionAdditionalData
{
    public Parent parent = new Parent();
    public Properties properties = new Properties();

    [JsonIgnore] public string databaseID;
    [JsonIgnore] public int score_English;
    [JsonIgnore] public int score_Math;
    [JsonIgnore] public int score_Japanese;
    [JsonIgnore] public int sum;
    [JsonIgnore] public string studentName;
    [JsonIgnore] public List<string> tag;

    public NotionAdditionalData()
    {
       
    }
    public void SetProps()
    {
        parent.database_id = databaseID;
        
        //properties.英語.number = score_English;
        //properties.数学.number = score_Math;
        //properties.国語.number = score_Japanese;
        //properties.合計.formula.number = sum;
        //tag.ForEach(a => properties.タグ.AddElem(a));
        properties.名前.title[0].text.content = studentName;
    }

    [System.Serializable]
    public class Parent
    {
        public string type = "database_id";
        public string database_id;
    }

    [System.Serializable]
    public class Properties
    {
        public Title 名前 = new Title();
        //public MultiSelect タグ = new MultiSelect();
        //public Number 国語 = new Number();
        //public Number 数学 = new Number();
        //public Number 英語 = new Number();
        //public Formula 合計 = new Formula();

        [System.Serializable]
        public class Number
        {
            //public string type = "number";
            public int number;
        }

        [System.Serializable]
        public class Title
        {
            public TitleElem[] title = new TitleElem[1] {new TitleElem()};

            [System.Serializable]
            public class TitleElem
            {
                public TextElem text = new TextElem();

                [System.Serializable]
                public class TextElem
                {
                    public string content;
                }
            }
        }

        [System.Serializable]
        public class Formula
        {
            //public string type = "formula";
            public Number formula = new Number();
        }


        [System.Serializable]
        public class MultiSelect
        {
            //public string type = "multi_select";
            public List<MultiSelectElement> multi_select = new List<MultiSelectElement>();

            [System.Serializable]
            public class MultiSelectElement
            {
                public string color = "green";
                public string name;
            }

            public void AddElem(string elem)
            {
                multi_select.Add(new MultiSelectElement { name = elem });
            }
        }
    }
}





