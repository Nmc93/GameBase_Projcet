public class StringTableData : TableBase
{
	public override string TableName { get => "StringTable"; }

	public StringTableData (string id, string text) 
	{
		this.id = id;
		this.text = text;
	}
	
	private string id;
	public string ID { get => id; }
	
	private string text;
	public string Text { get => text; }
}