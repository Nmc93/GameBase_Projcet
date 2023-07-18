using System;
using UnityEngine;
 
[Serializable]
public class SoundTableData : TableBase
{
	public override string TableName { get => "SoundTable"; }
	public override object GetKey { get => id; }

	public SoundTableData (int id, string path, int soundtype, bool isloop) 
	{
		this.id = id;
		this.path = path;
		this.soundtype = soundtype;
		this.isloop = isloop;
	}
	
	[SerializeField]
	private int id;
	public int ID { get => id; }
	
	[SerializeField]
	private string path;
	public string Path { get => path; }
	
	[SerializeField]
	private int soundtype;
	public int SoundType { get => soundtype; }
	
	[SerializeField]
	private bool isloop;
	public bool IsLoop { get => isloop; }
}