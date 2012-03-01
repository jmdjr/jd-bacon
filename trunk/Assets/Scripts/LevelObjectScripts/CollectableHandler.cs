using UnityEngine;
using System.Collections;

public class CollectableHandler : MonoBehaviour 
{
	private int _collectableCounter;
	private int _collectableTotal;
	private int _collectableRemaining;
	
	private TextMesh _guiText;
	
	public void Awake()
	{	
		//_guiText = GameObject.FindWithTag("CollectableCounter").GetComponent<TextMesh>();
		//_guiText.text = "Collect some shiat!";
	}
	
	public CollectableHandler()
	{
		_collectableCounter = 0;
		_collectableRemaining = 0;
		_collectableTotal = 0;
	}
	
	public int Counter
	{
		get{return _collectableCounter; }
		set{_collectableCounter = value; 
			UpdateTheText();}	
	}
	
	public int Total
	{
		get{ return _collectableTotal; }
		set{ _collectableTotal = value; }	
	}
	
	public int Remaining
	{
		get{ return _collectableTotal - _collectableCounter; }
	}
	
	public void UpdateTheText()
	{
		_guiText.text = "Collected = " + _collectableCounter + " of " + _collectableTotal;	
	}

}
