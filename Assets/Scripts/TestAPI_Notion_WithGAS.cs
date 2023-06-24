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

// ���ǂ��n�߂ēr���ŕۗ����Ă��܂����B�̂ŏC�����߂�ǂ��Ȃ��ĕ��u��


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
    �p��
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

        //StartCoroutine(GetNotionInfo(DataType.FilteredTable, ClassificationType.�p��, ValueType.number, ComparisonOperator.greater_than_or_equal_to, 94));
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
            dataTypes_United += $"_{a.ToString()}_";  //�f�[�^�^�C�v�𕡐��w��ł���悤�Ɏw�肵���^�C�v��S�����񂵂�GAS�ɓn�����߁B
            //GAS���ŁA�f�[�^�^�C�v���w�肷�镶����ɁZ�Z���܂܂�Ă��邩������������ہA�ԈႢ���N����ɂ����悤�Ƀe�L�g�[�Ɂu _ �v�𗼑��ɂ��������B
        }
        Debug.Log($"�f�[�^�^�C�v����{dataTypes_United}");
        
        
        UnityWebRequest request = UnityWebRequest.Get(CreateURI(dataTypes_United));


        yield return request.SendWebRequest();


        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(
                    @"�T�[�o�Ƃ̒ʐM�Ɏ��s�B
                        ���N�G�X�g���ڑ��ł��Ȃ������A
                        �Z�L�����e�B�ŕی삳�ꂽ�`���l�����m���ł��Ȃ������ȂǁB");
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
                break;

            default: throw new ArgumentOutOfRangeException();
        }

 
        string response_Json = request.downloadHandler.text;   
        
        Debug.Log(response_Json);

        ResponseData response_Object = JsonUtility.FromJson<ResponseData>(response_Json);


        //���O�ɏo��*********************************
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
        //���O�ɏo��*********************************
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
                Debug.Log("���N�G�X�g��");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(
                    @"�T�[�o�Ƃ̒ʐM�Ɏ��s�B
                        ���N�G�X�g���ڑ��ł��Ȃ������A
                        �Z�L�����e�B�ŕی삳�ꂽ�`���l�����m���ł��Ȃ������ȂǁB");
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
                break;

            default: throw new ArgumentOutOfRangeException();
        }


        string response_Json = request.downloadHandler.text;  Debug.Log(response_Json);


        ResponseData response_Object = JsonUtility.FromJson<ResponseData>(response_Json);  Debug.Log(response_Object);
        Debug.Log(response_Object.GetType());

        //���O�ɏo��*********************************
        foreach (string a in response_Object.FilteredTable) 
        {

            Debug.Log(a.ToString());
        }
        //���O�ɏo��*********************************
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


    //C#����ł�Notion�̃f�[�^�x�[�X�̃I�u�W�F�N�g�擾�ł��邯�ǁA�l�X�g����JSON�������ق�����ƂƂ��߂�ǂ�
    //Json.net�g������o����݂��������ǂ߂�ǂ��̂ŋC����������B
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
    //        Debug.Log("�v���p�e�B" + a);
    //    }
    //}

    //[System.Serializable]
    //public class ResponseData2
    //{
    //    public List<string> properties;
    //}
}

