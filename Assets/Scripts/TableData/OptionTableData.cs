using System;
using UnityEngine;
 
[Serializable]
public class OptionTableData : TableBase
{
	public override string TableName { get => "OptionTable"; }
	public override object GetKey { get => id; }

	public OptionTableData (string id, string optionvalue) 
	{
		this.id = id;
		this.optionvalue = optionvalue;
	}
	
	[SerializeField]
	private string id;
	public string ID { get => id; }
	
	[SerializeField]
	private string optionvalue;
	public string OptionValue { get => optionvalue; }
}