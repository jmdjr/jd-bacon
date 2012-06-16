using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using UnityScript.Lang;
// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData
{
    // We have to define a default instance of the structure 
    public PlayerSaveInfo _iUser;

    // Default constructor doesn't really do anything at the moment 
    public UserData() 
    {
        _iUser.GeneralStatsNames = new ArrayList();
        _iUser.GeneralStatsData = new ArrayList();
        _iUser.CollectionItemIDs = new ArrayList();
    }

    // Anything we want to store in the XML file, we define it here 
    public struct PlayerSaveInfo
    {
        public string PlayerName;
        public ArrayList GeneralStatsNames;
        public ArrayList GeneralStatsData;
        public ArrayList CollectionItemIDs;
    }
}
