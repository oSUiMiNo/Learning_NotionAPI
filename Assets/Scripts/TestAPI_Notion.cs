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

    //C#����ł�Notion�̃f�[�^�x�[�X�̃I�u�W�F�N�g�擾�ł��邯�ǁA�l�X�g����JSON�������ق�����ƂƂ��߂�ǂ�
    //Json.net�g������o����݂��������ǂ߂�ǂ��̂ŋC����������B
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


        string jsonStr = request.downloadHandler.text;
        JObject responseObj = JObject.Parse(jsonStr);

        Debug.Log(responseObj["properties"]["�p��"]);

        // Linq�ŁA����u���b�N�K�w���ł̓����v���p�e�B���̒l���擾���ăR���N�V�����Ɋi�[������@
        var i = responseObj["properties"]["�^�O"]["multi_select"]["options"].Select(p => p["name"]);
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


        string jsonStr = request.downloadHandler.text;

        JObject responseObj = JObject.Parse(jsonStr);

        // ���X�|���X�̒�����results�v���p�e�B�̒��g���g��
        Debug.Log(responseObj["results"][0]);

        foreach (var a in responseObj["results"])
        {
            Debug.Log(a["properties"]);
        }
    }
}
