using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

/// <summary>
/// Reads from a supplied SpriteAtlas Xml Document and provides methods for retrieving a
/// material reference for each frame of an animation sequence.  Currently only use
/// Starling object data type when exported from flash.
/// </summary>
public class JDSpriteAtlas : MonoBehaviour
{
    public Object AtlasDocument;
    
    // to read in from the animations xml file.
    private TextureList TextureAtlas = null;
    private Dictionary<string, TextureFrameSet> FrameSets = new Dictionary<string,TextureFrameSet>();

    public void Awake()
    {
        string _data = AtlasDocument.ToString();

        if (_data.IndexOf("<TextureAtlas") == -1)
        {
            Debug.LogError("Selected Atlas Document for: " + this.name + " is not properly formatted");
            var texture = new Texture2D(23, 23);
            return;
        }

        ParseTextureAtlas(_data);

        if(this.TextureAtlas == null)
        {
            Debug.LogError("Failed to generate TextureAtlas for: " + this.name + ".");
            return;
        }
        
        GenerateFrameSets();

        
    }

    private void ParseTextureAtlas(string _data)
    {
        // Add in some key string parts to xml to make it parseable.
        _data.Insert(_data.IndexOf("<SubTexture"), "<items>");
        _data.Insert(_data.LastIndexOf("/>") + "/>".Length, "</items>");

        this.TextureAtlas = (TextureList)JDGameUtilz.DeserializeObject(_data, "TextureAtlas", typeof(TextureList), JDGameUtilz.EncodingType.UTF8);
    }
    private void GenerateFrameSets()
    {
        foreach (SubTexture st in this.TextureAtlas.items)
        {
            string frameName = st.name;
            string setName = frameName.Substring(0, frameName.IndexOf("0"));
            bool hasFrameSet = this.FrameSets.ContainsKey(setName);

            if (!hasFrameSet)
            {
                this.FrameSets.Add(setName, new TextureFrameSet()
                {
                    Name = setName,
                    height = st.height,
                    width = st.width
                });

                this.FrameSets[setName].frames.Add(st);
            }
            else
            {
                // has setname but not the texture 
                if (!this.FrameSets[setName].frames.Contains(st))
                {
                    this.FrameSets[setName].frames.Add(st);
                }
            }
        }
    }


    // mimics the structure of the
    [XmlRoot(ElementName="TextureAtlas")]
    public class TextureList
    {
        public List<SubTexture> items;

        [XmlAttribute]
        public string imagePath;
    }
    public struct SubTexture 
    {
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public int x;
        [XmlAttribute]
        public int y;
        [XmlAttribute("h")]
        public int height;
        [XmlAttribute("w")]
        public int width;
    }
    public class TextureFrameSet
    {
        public string Name;
        public int height;
        public int width;

        public List<SubTexture> frames = new List<SubTexture>();
    }
}
