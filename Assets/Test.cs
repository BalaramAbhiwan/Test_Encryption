using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class Test : MonoBehaviour
{
    public string TestEncipt;

    [System.Serializable]
    public class WalletData
    {
        public string walletAddress;
    }
    public WalletData walletData;
    //private void Update()
    //{
    //    //Test 
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        //string TempEncriptedJson = "U2FsdGVkX1+LNknLqr69FgRUZcb4AqqYbSLBBsHCXImR8QZeLFlslEe/6nsem4cyt0BROpxA2B8J1om+Wie1lUAUgsx4eF6S0rVkLF7" +
    //        //    "NOvpoBuha5fXB7pgPnILqKLoc2IChZf37itNLQsVG91zUB0DN5Dx+wrIGQKyBny/yl2UwzRneqiQ5OCByjbHwRaTmWdvDoUJoNYsmjHB0fzsY1g== ";
    //        //string cleanedHexString = EncryptionHelper.CleanHexString(TempEncriptedJson);
    //        //Debug.Log(cleanedHexString);

    //        //string TempClean = EncryptionHelper.CleanHexString(TestEncipt);
    //        string TempDecriptedJson = EncryptionHelper.DecryptData(TestEncipt);
    //        Debug.Log(TempDecriptedJson);
    //    }

    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        //string JsonData={"walletAddress":"addr_test1qrpmklafl83phr57sknpl56nqmafc723n5lnawrkn5mrkewm349cl6c5ka7h0kqkapstqsjjqvy9mkhrkng66mpq33gqfau0c9"};

    //        var exampleData = new
    //        {
    //            walletAddress = "addr_test1qrpmklafl83phr57sknpl56nqmafc723n5lnawrkn5mrkewm349cl6c5ka7h0kqkapstqsqvy9mkhrkng66mpq39"
    //        };

    //        string JsonConvert=JsonUtility.ToJson(walletData);
    //        Debug.Log(JsonConvert);

    //        string TempEncripteJson = EncryptionHelper.EncryptData(JsonConvert);
    //        Debug.Log(TempEncripteJson);
    //        string[] spString = TempEncripteJson.Split(':');

    //        //TestEncipt = spString[1];
    //        TestEncipt = TempEncripteJson;
    //    }
    //}

    [ContextMenu("Onclick Encript")]
    void Onclick_encript()
    {
        var exampleData = new
        {
            walletAddress = "addr_test1qrpmklafl83phr57sknpl56nqmafc723n5lnawrkn5mrkewm349cl6c5ka7h0kqkapstqsqvy9mkhrkng66mpq39"
        };

        string JsonConvert = JsonUtility.ToJson(walletData);
        Debug.Log(JsonConvert);

        string TempEncripteJson = EncryptionHelper.EncryptData(JsonConvert);
        Debug.Log(TempEncripteJson);
        string[] spString = TempEncripteJson.Split(':');

        //TestEncipt = spString[1];
        TestEncipt = TempEncripteJson;
    }

    [ContextMenu("Onclick Decript")]
    public void Onclick_Decript()
    {
        string TempDecriptedJson = EncryptionHelper.DecryptData(TestEncipt);

        Debug.Log(TempDecriptedJson);


        //// Print the result
        //string[] TempDecripted= TempDecriptedJson.Split('{');


        //// Create a new string, starting from the 1st element (index 1)
        //string result = string.Join("{", TempDecripted, 1, TempDecripted.Length - 1);

        ////for (int i = 0; i < TempDecripted.Length; i++)
        ////{
        ////    Debug.Log(TempDecripted[i]);
        ////}

        ////Debug.Log(TempDecripted[1]);
    }

    public Image targetRenderer; // The renderer to apply the texture to
    IEnumerator Start()
    {
        //// Create a UnityWebRequest to get the texture
        //UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://ipfs.io/ipfs/QmTLeetyGofbCu2ateXzemQ8pHdbs3KRpSY1TCKXRcRNuw");

        //// Send the request and wait for it to complete
        //yield return request.SendWebRequest();

        //// Check for errors
        //if (request.result != UnityWebRequest.Result.Success)
        //{
        //    Debug.LogError("Error downloading texture: " + request.error);
        //}
        //else
        //{
        //    // Get the downloaded texture
        //    Texture2D texture = DownloadHandlerTexture.GetContent(request);
        //    Sprite sprite = TextureToSprite(texture);
        //    // Apply the texture to the renderer's material
        //    if (targetRenderer != null)
        //    {
        //        targetRenderer.sprite = sprite;
        //    }
        //}

        string TempWrappedURL= "http://192.168.0.48:5000/users"+ "/profile/get";
        string jsonData = JsonUtility.ToJson("{\"data\":\"ca82707913915af89d5cad37de0942a1:32ab8235d6f237a7884e6c2e58fd6bb1938750ee6452f3f4d8ad4137cb0e5eebcf1cf7e7fcce2d91ba488602e6a86114737fc6f272f0fc981e7193dc1b58a44f8289b8d2e8d8a4eb6a6f4b631ef50f358e580f6b07afe017bbcf99d729be624d99bcdee40fcb8bf9a0247479fec53d20879851fa0787dcd612ce8859bcf5f779175cb34cd976e7ba530cd0df61a094b6\"}");
        UnityWebRequest request = UnityWebRequest.Get(TempWrappedURL);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ3YWxsZXRBZGRyZXNzIjoiYWRkcl90ZXN0MXFycG1rbGFmbDgzcGhyNTdza25wbDU2bnFtYWZjNzIzbjVsbmF3cmtuNW1ya2V3bTM0OWNsNmM1a2E3aDBrcWthcHN0cXNqanF2eTlta2hya25nNjZtcHEzM2dxZmF1MGM5IiwiaWF0IjoxNzI1NDQ2OTQyLCJleHAiOjE3MjYzMTA5NDJ9.UvHwGl09UFXaQz-w_ZFdhNK-LvmCITpAgkmWBFFyW1Q");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
    }
    Sprite TextureToSprite(Texture2D texture)
    {
        // Create a new sprite from the Texture2D
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }




}