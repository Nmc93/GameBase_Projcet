using System;
using UnityEngine;
 
[Serializable]
public class InputKeyTableData : TableBase
{
	public override string TableName { get => "InputKeyTable"; }
	public override object GetKey { get => id; }

	public InputKeyTableData (string id, string keystring) 
	{
		this.id = id;
		this.keystring = keystring;
	}
	
	[SerializeField]
	private string id;
	public string ID { get => id; }
	
	[SerializeField]
	private string keystring;
	public string KeyString { get => keystring; }
}