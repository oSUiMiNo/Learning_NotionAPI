using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Web;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Linq;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

// 改良を始めて途中で保留してしまった。ので修復がめんどくなって放置中


public enum DataType
{   
    Default,
    Table,
    Classification,
    Value,
    Type,
    FilteredTable
}
public enum ClassificationType
{
    Default,
    英語
}
public enum ValueType
{
    Default,
    number
}
public enum ComparisonOperator
{
    Default,
    greater_than_or_equal_to
}


public class TestAPI_Notion_WithGAS : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(GetNotionInfo(DataType.Value, DataType.Property));
        //StartCoroutine(GetNotionInfo(DataType.Type, DataType.Default, DataType.Classification));
        //StartCoroutine(GetNotionInfo(DataType.Table));
        //StartCoroutine(GetNotionInfo(DataType.Classification));
        //StartCoroutine(GetNotionInfo(DataType.Value));
        //StartCoroutine(GetNotionInfo(DataType.Type));
        //StartCoroutine(Notion());

        //StartCoroutine(GetNotionInfo(DataType.FilteredTable, ClassificationType.英語, ValueType.number, ComparisonOperator.greater_than_or_equal_to, 94));
    }
     

    private const string URL = "https://script.google.com/macros/s/AKfycbxKiVq4H0gGlqK7bNWsFAnnHqDeTUQ06EINsJZlNXKyo1EaCuk1fUqJ37pB5yUapcHWug/exec";
    Uri CreateURI(string dataTypes_United)
    {
        var queryString = HttpUtility.ParseQueryString("");
        
        queryString.Add("datatype", dataTypes_United);

        var uriBuilder = new UriBuilder(URL) { Query = queryString.ToString()};
        return uriBuilder.Uri;
    }



    IEnumerator GetNotionInfo(params DataType[] dataTypes)
    {
        string dataTypes_United = string.Empty;
        foreach (DataType a in dataTypes)
        {
            dataTypes_United += $"_{a.ToString()}_";  //データタイプを複数指定できるように指定したタイプを全部羅列してGASに渡すため。
            //GAS側で、データタイプを指定する文字列に〇〇が含まれているか検索をかける際、間違いが起こりにくいようにテキトーに「 _ 」を両側につけただけ。
        }
        Debug.Log($"データタイプたち{dataTypes_United}");
        
        
        UnityWebRequest request = UnityWebRequest.Get(CreateURI(dataTypes_United));


        yield return request.SendWebRequest();


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
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"サーバがエラー応答を返した。
                        サーバとの通信には成功したが、
                        接続プロトコルで定義されているエラーを受け取った。");
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"データの処理中にエラーが発生。
                        リクエストはサーバとの通信に成功したが、
                        受信したデータの処理中にエラーが発生。
                        データが破損しているか、正しい形式ではないなど。");
                break;

            default: throw new ArgumentOutOfRangeException();
        }

 
        string response_Json = request.downloadHandler.text;   
        
        Debug.Log(response_Json);

        ResponseData response_Object = JsonUtility.FromJson<ResponseData>(response_Json);


        //ログに出力*********************************
        foreach (string a in response_Object.Table)
        {
            Debug.Log(a[0]);
        }
        foreach (string a in response_Object.Classification)
        {
            Debug.Log(a);
        }
        foreach (string a in response_Object.Value)
        {
            Debug.Log(a);
        }
        foreach (string a in response_Object.Type)
        {
            Debug.Log(a);
        }
        //ログに出力*********************************
    }

    Uri CreateURI_WithFilter(string filter_JSON)
    {   
        var queryString = HttpUtility.ParseQueryString("");
        queryString.Add("filter", filter_JSON);

        var uriBuilder = new UriBuilder(URL) { Query = queryString.ToString() };
        return uriBuilder.Uri;
    }



    IEnumerator GetNotionInfo<T>(DataType dataTypes, ClassificationType classificationType, ValueType valueType, ComparisonOperator comparisonOperator, T comparison)
    {
        Debug.Log(classificationType.ToString());
        Debug.Log(valueType.ToString());
        Debug.Log(comparisonOperator.ToString());
        Debug.Log(comparison);
        Filter<T> filter = new Filter<T>()
        {
            classificationType = classificationType.ToString(),
            valueType = valueType.ToString(),
            comparisonOperator = comparisonOperator.ToString(),
            comparison = comparison
        };
        string filter_JSON = JsonUtility.ToJson(filter);
        Debug.Log(filter_JSON);

        UnityWebRequest request = UnityWebRequest.Get(CreateURI_WithFilter(filter_JSON));


        yield return request.SendWebRequest();


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
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"サーバがエラー応答を返した。
                        サーバとの通信には成功したが、
                        接続プロトコルで定義されているエラーを受け取った。");
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"データの処理中にエラーが発生。
                        リクエストはサーバとの通信に成功したが、
                        受信したデータの処理中にエラーが発生。
                        データが破損しているか、正しい形式ではないなど。");
                break;

            default: throw new ArgumentOutOfRangeException();
        }


        string response_Json = request.downloadHandler.text;  Debug.Log(response_Json);


        ResponseData response_Object = JsonUtility.FromJson<ResponseData>(response_Json);  Debug.Log(response_Object);
        Debug.Log(response_Object.GetType());

        //ログに出力*********************************
        foreach (string a in response_Object.FilteredTable) 
        {

            Debug.Log(a.ToString());
        }
        //ログに出力*********************************
    }


    [System.Serializable]
    public class ResponseData
    {
        public List<string> Table;
        public List<string> Classification;
        public List<string> Type;
        public List<string> Value;
        public List<string> FilteredTable;
    }

    public class Filter<T>
    {
        public string classificationType;
        public string valueType;
        public string comparisonOperator;
        public T comparison;
    }


    //C#からでもNotionのデータベースのオブジェクト取得できるけど、ネストしたJSONを解きほぐす作業とかめんどい
    //Json.net使ったら出来るみたいだけどめんどいので気が向いたら。
    //IEnumerator Notion()
    //{
    //    UnityWebRequest request = UnityWebRequest.Get("https://api.notion.com/v1/databases/13e95b2fc57c4bcfa28cbb0d24e6f11e");
    //    request.SetRequestHeader("Authorization", "Bearer " + "secret_GKXw55IIbLe0LkOPsWpt0IG8BCLfOokRhamOysuBhyW");
    //    request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
    //    request.SetRequestHeader("Notion-Version", "2022-02-22");

    //    yield return request.SendWebRequest();

    //    if (request.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError(request.error);
    //    }

    //    var json = request.downloadHandler.text;
    //    Debug.Log(json);
    //    ResponseData2 J = JsonUtility.FromJson<ResponseData2>(json);

    //    Debug.Log(J.properties);

    //    foreach (var a in J.properties)
    //    {
    //        Debug.Log("プロパティ" + a);
    //    }
    //}

    //[System.Serializable]
    //public class ResponseData2
    //{
    //    public List<string> properties;
    //}
}

