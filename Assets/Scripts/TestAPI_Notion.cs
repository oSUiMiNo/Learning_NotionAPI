using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;

// ����Notion�y�[�W��DB�u���Ă���B
//https://www.notion.so/6795dd70753a495897cb85e0abce95fe

public class TestAPI_Notion : MonoBehaviour
{
    //private const string NotionAccessToken = "secret_GKXw55IIbLe0LkOPsWpt0IG8BCLfOokRhamOysuBhyW";
    private const string NotionAccessToken = "secret_OIxSWO69mxnD9FNbmL2US0pcsLWCUmsaglBZBCWPWrC"; //�V������

    void Start()
    {
        InputEventHandler.OnDown_A += UseAPI_GET;
        InputEventHandler.OnDown_B += UseAPI_POST;
        InputEventHandler.OnDown_C += UseAPI_Add;
        InputEventHandler.OnDown_D += UseAPI_DowiloadFile;
        InputEventHandler.OnDown_E += UseAPI_Add_Test;
    }

    //void Oauth()
    //{
    //    var client = new RestClient("https://api.notion.com/v1/oauth/token _");
    //    client.Timeout = -1;
    //    var request = new RestRequest(Method.POST);
    //    client.UserAgent = "Apidog/1.0.0 (https://apidog.com)";
    //    request.AddHeader("Content-Type", "application/json");
    //    var body = @"{" + "\n" +
    //    @"    ""grant_type"": ""string""," + "\n" +
    //    @"    ""code"": ""string""," + "\n" +
    //    @"    ""redirect_uri"": ""string""," + "\n" +
    //    @"    ""external_account"": {" + "\n" +
    //    @"        ""key"": ""string""," + "\n" +
    //    @"        ""name"": ""string""" + "\n" +
    //    @"    }" + "\n" +
    //    @"}";
    //    request.AddParameter("application/json", body, ParameterType.RequestBody);
    //    IRestResponse response = client.Execute(request);
    //    Console.WriteLine(response.Content);
    //}



    //C#����ł�Notion�̃f�[�^�x�[�X�̃I�u�W�F�N�g�擾�ł��邯�ǁA�l�X�g����JSON�������ق�����ƂƂ��߂�ǂ�
    //Json.net�g������o����݂��������ǂ߂�ǂ��̂ŋC����������B
    async void UseAPI_GET()
    {
        string DatabaseID = "b40fd183c85b4a1da4fbb1c1beaaa6b2";
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
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(
                        @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(
                        @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
                    Debug.LogError(request.error);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            jsonStr = request.downloadHandler.text;
        }
        
        JObject responseObj = JObject.Parse(jsonStr);

        Debug.Log($"���X�|���X�I�u�W�F�N�g\n{responseObj}");
        Debug.Log($"�v���p�e�B\n{responseObj["properties"]}");
        Debug.Log($"�v���p�e�B_�p��\n{responseObj["properties"]["�p��"]}");

        // Linq�ŁA����u���b�N�K�w���ł̓����v���p�e�B���̒l��S�Ď擾���ăR���N�V�����Ɋi�[������@
        IEnumerable<JToken> i = responseObj["properties"]["�^�O"]["multi_select"]["options"].Select(p => p["name"]);
        foreach(var a in i)
        {
            Debug.Log(a);
        }
    }


    async void UseAPI_POST()
    {
        string DatabaseID = "b40fd183c85b4a1da4fbb1c1beaaa6b2";
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
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(
                        @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(
                        @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
                    Debug.LogError(request.error);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            jsonStr = request.downloadHandler.text;
        }

        JObject responseObj = JObject.Parse(jsonStr);

        // ���X�|���X�̒�����results�v���p�e�B�̒��g���g��
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
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(
                        @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                    Debug.LogError(request.error);
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(
                        @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
                    Debug.LogError(request.error);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            jsonStr = request.downloadHandler.text;
        }

        JObject responseObj = JObject.Parse(jsonStr);

        // ���X�|���X�̒�����results�v���p�e�B�̒��g���g��
        //Debug.Log(responseObj["results"][0]);
        //foreach (var a in responseObj["results"])
        //{
        //    Debug.Log(a["properties"]);
        //}


        // �f�[�^�x�[�X����擾�����_�E�����[�h�����N
        string downloadURL = responseObj["results"][0]["properties"]["�t�@�C��"]["files"][0]["file"]["url"].ToString();
        DebugView.Log(downloadURL);

        // �f�[�^�x�[�X����擾�����t�@�C����
        string fileName = responseObj["results"][0]["properties"]["�t�@�C��"]["files"][0]["name"].ToString();

        // �V�K�쐬����t�@�C���̃p�X�Ɩ��O
        string newFilePath = @$"C:\Users\osuim\Downloads\{fileName}";

        byte[] results;

        //�t�@�C���_�E�����[�h
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
                // json�Ƃ��̃e�L�X�g�t�@�C���Ȃ炻�̂܂ܓ��邱�Ƃ��\
                Debug.Log(request.downloadHandler.text);

                // �e�L�X�g�t�@�C���ȊO���_�E�����[�h�������̂Ńo�C�i���f�[�^���畜����������ɂ�����
                results = request.downloadHandler.data;
                // �o�C�g����t�@�C������
                using (FileStream fs = new FileStream(newFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    // �V�K�t�@�C���Ƀo�C�g�z���0�����ڂ���Ō�܂ŏ�������
                    fs.Write(results, 0, results.Length);
                }
            }
        }
    }



    string P =
        "{\r\n  \"parent\": {\r\n    \"type\": \"database_id\",\r\n    \"database_id\": \"b40fd183c85b4a1da4fbb1c1beaaa6b2\"\r\n  },\r\n  \"properties\": {\r\n    \"���O\": {\r\n      \"title\": [\r\n        {\r\n          \"text\": {\r\n            \"content\": \"���\"\r\n          }\r\n        }\r\n      ]\r\n    },\r\n    \"�^�O\": {\r\n      \"type\": \"multi_select\",\r\n      \"multi_select\": [\r\n        {\r\n          \"name\": \"���i\"\r\n        }\r\n      ]\r\n    },\r\n    \"����\": {\r\n      \"type\": \"number\",\r\n      \"number\": 30\r\n    },\r\n    \"���w\": {\r\n      \"type\": \"number\",\r\n      \"number\": 30\r\n    },\r\n    \"�p��\": {\r\n      \"type\": \"number\",\r\n      \"number\": 30\r\n    },\r\n  }\r\n}";


    // �p�[�X�G���[�o�Ďg����
    async void UseAPI_Add()
    {
        const string DatabaseID = "b40fd183c85b4a1da4fbb1c1beaaa6b2";

        NotionAdditionalData additionalData = new NotionAdditionalData()
        {
            databaseID = DatabaseID,
            score_English = 30,
            score_Japanese = 30,
            score_Math = 30,
            sum = 90,
            studentName = "���",
            tag = new List<string>
            {
               "���i"
            }
        };
        additionalData.SetProps();
        string payload = JsonConvert.SerializeObject(additionalData, Formatting.Indented);

        DebugView.Log($"{payload}");

        JObject jsonObject = JObject.Parse(payload);
        //DebugView.Log($"{jsonObject["properties"]["���w"]["number"]}");
        string payload0 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        DebugView.Log($"{payload0}");

        DebugView.Log(P);

        UnityWebRequest request = UnityWebRequest.Post($"https://api.notion.com/v1/pages", payload);
        request.SetRequestHeader("Authorization", $"Bearer {NotionAccessToken}");
        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Notion-Version", "2022-02-22");


        byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();


        await request.SendWebRequest();



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
                Debug.LogError(request.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                Debug.LogError(request.error);
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
                Debug.LogError(request.error);
                break;

            default: throw new ArgumentOutOfRangeException();
        }
    }



    class A
    {
        public Dictionary<string, string> parent = new Dictionary<string, string>()
        {
            { "database_id", "cf2343f8bb934f51b3cddd99640fc790" }
        };
       
    }



    // �p�[�X�G���[�o�Ďg����
    async void UseAPI_Add_Test()
    {
        string DatabaseID = "cf2343f8bb934f51b3cddd99640fc790";

        EditableJSON eJson_Payload = new EditableJSON($@"{Application.dataPath}\Payload.json");
        eJson_Payload.Obj["parent"]["database_id"] = DatabaseID;
        string payload = eJson_Payload.Json;
        
        DebugView.Log($"{payload}");

        EditableJSON eJson_Opts = new EditableJSON($@"{Application.dataPath}\Opts.json");
        eJson_Opts.Obj["headers"]["Notion-Version"] = "2022-06-28";
        eJson_Opts.Obj["headers"]["Authorization"] = $"Bearer {NotionAccessToken}";
        eJson_Opts.Obj["headers"]["Content-Type"] = "application/json; charset=UTF-8";
        eJson_Opts.Obj["payload"] = payload;


        string opts = eJson_Opts.Json;


        UnityWebRequest request = UnityWebRequest.Post($"https://api.notion.com/v1/pages", opts);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(opts);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();


        await request.SendWebRequest();



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
                Debug.LogError(request.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(
                    @"�T�[�o���G���[������Ԃ����B
                        �T�[�o�Ƃ̒ʐM�ɂ͐����������A
                        �ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B");
                Debug.LogError(request.error);
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(
                    @"�f�[�^�̏������ɃG���[�������B
                        ���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
                        ��M�����f�[�^�̏������ɃG���[�������B
                        �f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB");
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
        
        //properties.�p��.number = score_English;
        //properties.���w.number = score_Math;
        //properties.����.number = score_Japanese;
        //properties.���v.formula.number = sum;
        //tag.ForEach(a => properties.�^�O.AddElem(a));
        properties.���O.title[0].text.content = studentName;
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
        public Title ���O = new Title();
        //public MultiSelect �^�O = new MultiSelect();
        //public Number ���� = new Number();
        //public Number ���w = new Number();
        //public Number �p�� = new Number();
        //public Formula ���v = new Formula();

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





