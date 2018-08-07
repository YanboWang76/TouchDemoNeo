using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;

public class XMLDb {

    public static void Write<T>(string path, T data) {
        FileStream fs = null;
        try {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            serializer.Serialize(fs, data);
        } catch (Exception e) {
            Debug.Log(e.ToString());
        } finally {
            if (fs != null) {
                fs.Close();
            }
        }
    }

    public static T Read<T>(string path) {
        FileStream fs = null;
        try {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return (T)serializer.Deserialize(fs);
        } catch (Exception e) {
            Debug.Log(e.ToString());
        } finally {
            if (fs != null) {
                fs.Close();
            }
        }
        return default(T);
    }
}
