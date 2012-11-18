using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public static class JDGameUtilz
{
    public static string _FileLocationz = Application.dataPath;
    public static string _AnimationzLocationz = _FileLocationz + @"\Animations";
    public static string _prefabzLocationz = _FileLocationz + @"\Prefabs\Game Prefabs";
    public static string _LevelzLocationz = _FileLocationz + @"\Levels";
    public static string _ScriptzLocationz = _FileLocationz + @"\Scripts\JDBaconUnityScripts";

    public enum EncodingType
    {
        UTF8,
        UTF7,
        Unicode,
        UTF32 
    }

    /* The following metods came from the referenced URL */
    private static string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    private static byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    private static byte[] StringToUTFUnicodeByteArray(string pXmlString)
    {
        UnicodeEncoding encoding = new UnicodeEncoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Finally our save and load methods for the file itself 
    public static void CreateXML(string fileLocation, string _data)
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(fileLocation);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(_data);
        writer.Close();
    }

    public static string LoadXML(string xmlLocation)
    {
        StreamReader r = File.OpenText(xmlLocation);
        string _info = r.ReadToEnd();
        r.Close();
        return _info;
    }

    // Here we serialize our UserData object of myData 
    public static string SerializeObject(object pObject, string rootElementName, Type ObjectType)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = rootElementName;

        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(ObjectType, xRoot);
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form 
    public static object DeserializeObject(string pXmlizedString, string rootElementName, Type ObjectType, EncodingType encodingType)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = rootElementName;

        XmlSerializer xs = new XmlSerializer(ObjectType, xRoot);
        MemoryStream memoryStream = null;

        switch (encodingType)
        {
            case EncodingType.Unicode:
                memoryStream = new MemoryStream(StringToUTFUnicodeByteArray(pXmlizedString));
                break;
            case EncodingType.UTF8:
                memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
                break;
            case EncodingType.UTF7:
                break;
            default:
                memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
                break;
        }

        return xs.Deserialize(memoryStream);
    }

    public static JDMonoBehavior GetJDMonoBehavior(GameObject obj)
    {
        return obj.GetComponent<JDMonoBehavior>();
    }

}