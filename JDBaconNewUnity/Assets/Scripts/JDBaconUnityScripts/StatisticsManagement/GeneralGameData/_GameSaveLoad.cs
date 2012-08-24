using System.IO;
using UnityEngine;

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
        //Debug.Log(Application.persistentDataPath); // location good for save game location
        //Debug.Log(Application.dataPath);            // For testing purposes

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

            _data = JDGameUtilz.LoadXML(_FileLocation + "\\Animations\\JD Bacon SpriteAts\\Sparrowv1Grenade.xml");
            //LoadXML(_FileLocation + "\\" + myData._iUser.PlayerName + ".xml");

            if (_data.ToString() != "")
            {
                //Debug.Log(_data);
                //var obj = (JDSpriteAtlas.TextureList)JDGameUtilz
                //    .DeserializeObject(_data, "TextureAtlas", typeof(JDSpriteAtlas.TextureList), JDGameUtilz.EncodingType.UTF8);
                //Debug.Log(obj.imagePath);
                //Debug.Log(obj.items[0].name + " " + obj.items[0].height + " " + obj.items[0].width);
            }

        }

        //*************************************************** 
        // Saving The Player... 
        // **************************************************    
        if (GUI.Button(_Save, "Save"))
        {
            GUI.Label(_SaveMSG, "Saving to: " + _FileLocation);
            //JDSpriteAtlas.TextureAtlas = new JDSpriteAtlas.TextureList();
            //JDSpriteAtlas.TextureAtlas.imagePath = "stuffs.png";
            //JDSpriteAtlas.TextureAtlas.items.Add(new JDSpriteAtlas.SubTexture() 
            //{
            //   height = 0, y = 0, x = 0, width = 0, name = "test"
            //});
            
            //    // Time to creat our XML! 
            //_data = JDGameUtilz.SerializeObject(JDSpriteAtlas.TextureAtlas, "TextureAtlas", typeof(JDSpriteAtlas.TextureList));
            // This is the final resulting XML from the serialization process 
            JDGameUtilz.CreateXML(JDGameUtilz._AnimationzLocationz + @"\JD Bacon SpriteAts\Testing.xml", this._data);
        }


    }

}
