using System;
using UnityEngine;
 
[Serializable]
public class StringTableData : TableBase
{
	public override string TableName { get => "StringTable"; }
	public override object GetKey { get => id; }

	public StringTableData (string id, string text) 
	{
		this.id = id;
		this.text = text;
	}
	
	[SerializeField]
	private string id;
	public string ID { get => id; }
	
	[SerializeField]
	private string text;
	public string Text { get => text; }
}