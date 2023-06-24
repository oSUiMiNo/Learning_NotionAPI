using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class TestAPI_Notion : MonoBehaviour
{
    private const string NotionAccessToken = "secret_GKXw55IIbLe0LkOPsWpt0IG8BCLfOokRhamOysuBhyW";
    private const string DatabaseId = "b40fd183c85b4a1da4fbb1c1beaaa6b2";

    void Start()
    {
        InputEventHandler.OnKeyDown_A += UseAPI_GET;
        InputEventHandler.OnKeyDown_B += UseAPI_POST;
    }

    //C#からでもNotionのデータベースのオブジェクト取得できるけど、ネストしたJSONを解きほぐす作業とかめんどい
    //Json.net使ったら出来るみたいだけどめんどいので気が向いたら。
    async void UseAPI_GET()
    {
        UnityWebRequest request = UnityWebRequest.Get($"https://api.notion.com/v1/databases/{DatabaseId}");
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


        string jsonStr = request.downloadHandler.text;
        JObject responseObj = JObject.Parse(jsonStr);

        Debug.Log(responseObj["properties"]["英語"]);

        // Linqで、あるブロック階層内での同じプロパティ名の値を取得してコレクションに格納する方法
        var i = responseObj["properties"]["タグ"]["multi_select"]["options"].Select(p => p["name"]);
        foreach(var a in i)
        {
            Debug.Log(a);
        }
    }


    async void UseAPI_POST()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Post($"https://api.notion.com/v1/databases/{DatabaseId}/query", form);
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


        string jsonStr = request.downloadHandler.text;

        JObject responseObj = JObject.Parse(jsonStr);

        // レスポンスの中からresultsプロパティの中身を使う
        Debug.Log(responseObj["results"][0]);

        foreach (var a in responseObj["results"])
        {
            Debug.Log(a["properties"]);
        }
    }
}
