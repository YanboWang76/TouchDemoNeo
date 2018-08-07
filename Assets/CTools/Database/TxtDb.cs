using System.IO;
using LitJson;

public class TxtDb {

    public static void Write(string path, string data, bool encrypt) {
        using (StreamWriter sw = File.CreateText(path)) {
            if (encrypt) {
                sw.Write(Crypto.Encrypt(data));
            } else {
                sw.Write(data);
            }
        }
    }

    public static string Read(string path, bool decrypt) {
        if (File.Exists(path)) {
            string readedStr = "";
            using (StreamReader sr = File.OpenText(path)) {
                readedStr = sr.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(readedStr)) {
                if (decrypt) {
                    return Crypto.Decrypt(readedStr);
                }
                return readedStr;
            }
        }
        return null;
    }
}
