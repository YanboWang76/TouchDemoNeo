using UnityEngine;
using System.IO;
using LitJson;

public class JsonDb {

    public static void Write(string path, object data, bool encrypt) {
        string str = JsonMapper.ToJson(data);
        using (StreamWriter sw = File.CreateText(path)) {
            if (encrypt) {
                sw.Write(Crypto.Encrypt(str));
            } else {
                sw.Write(str);
            }
        }
        Debug.Log("Saved to: " + path);
    }

    public static T Read<T>(string path, bool decrypt) {
        if (File.Exists(path)) {
            string readedStr = "";
            using (StreamReader sr = File.OpenText(path)) {
                readedStr = sr.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(readedStr)) {
                if (decrypt) {
                    readedStr = Crypto.Decrypt(readedStr);
                }
                try {
                    return JsonMapper.ToObject<T>(readedStr);
                } catch (System.Exception e) {
                    Debug.LogError(e);
                }
            }
        }
        return default(T);
    }

    public static T Parse<T>(string str) {
        if (!string.IsNullOrEmpty(str)) {
            try {
                return JsonMapper.ToObject<T>(str);
            } catch (System.Exception e) {
                Debug.LogError(e);
            }
        }
        return default(T);
    }


    public static string Parse(object data) {
        if (data != null) {
            try {
                return JsonMapper.ToJson(data);
            } catch (System.Exception e) {
                Debug.LogError(e);
            }
        }
        return null;
    }
}
