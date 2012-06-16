using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

/// <summary>
/// Found at the following URL: http://www.unifycommunity.com/wiki/index.php?title=Save_and_Load_from_XML
/// Original source code written by Zumwalt, all questions, comments and praises should be directed towards him :D
/// </summary>

public class _GameSaveLoad : MonoBehaviour
{

    // An example where the encoding can be found is at 
    // http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp 
    // We will just use the KISS method and cheat a little and use 
    // the examples from the web page since they are fully described 

    // This is our local private members 
    Rect _Save, _Load, _SaveMSG, _LoadMSG;
    string _FileLocation;
    

    // for temporary access by external components.
    public UserData myData;

    string _data;

    // When the EGO is instansiated the Start will trigger 
    // so we setup our initial values for our local members 
    public void Awake()
    {
        // We setup our rectangles for our messages 
        _Save = new Rect(10, 80, 100, 20);
        _Load = new Rect(10, 100, 100, 20);
        _SaveMSG = new Rect(10, 120, 400, 40);
        _LoadMSG = new Rect(10, 140, 400, 40);

        // Where we want to save and load to and from 
        _FileLocation = Application.dataPath;

        // we need soemthing to store the information into 
        myData = new UserData();

    }

    void Update() { }

    void OnGUI()
    {

        //*************************************************** 
        // Loading The Player... 
        // **************************************************       
        if (GUI.Button(_Load, "Load"))
        {

            GUI.Label(_LoadMSG, "Loading from: " + _FileLocation);
            // Load our UserData into myData 
            LoadXML();
            if (_data.ToString() != "")
            {
                // notice how I use a reference to type (UserData) here, you need this 
                // so that the returned object is converted into the correct type 
                myData = (UserData)DeserializeObject(_data);

                Debug.Log(myData._iUser.PlayerName);
            }

        }

        //*************************************************** 
        // Saving The Player... 
        // **************************************************    
        if (GUI.Button(_Save, "Save"))
        {
            GUI.Label(_SaveMSG, "Saving to: " + _FileLocation);

            // Time to creat our XML! 
            _data = SerializeObject(myData);
            // This is the final resulting XML from the serialization process 
            CreateXML();
            Debug.Log(_FileLocation);
        }


    }

    /* The following metods came from the referenced URL */
    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Here we serialize our UserData object of myData 
    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form 
    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        return xs.Deserialize(memoryStream);
    }

    // Finally our save and load methods for the file itself 
    void CreateXML()
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(_FileLocation + "\\" + myData._iUser.PlayerName + ".xml");
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
        Debug.Log("File written.");
    }

    void LoadXML()
    {
        StreamReader r = File.OpenText(_FileLocation + "\\" + myData._iUser.PlayerName + ".xml");
        string _info = r.ReadToEnd();
        r.Close();
        _data = _info;
        Debug.Log("File Read");
    }
}
