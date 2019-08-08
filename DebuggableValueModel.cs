using LunarByte.MVVM;

public partial class DebuggableValueModel : Model
{
	private string NameField;
	private float StepLengthField;
	private float ValueField;
	public string Name
	{
		get { return NameField; }
		set
		{
			NameField = value;
			OnPropertyChanged();
		}
	}
	public float StepLength
	{
		get { return StepLengthField; }
		set
		{
			StepLengthField = value;
			OnPropertyChanged();
		}
	}

	public float Value
	{
		get { return ValueField; }
		set
		{
			ValueField = value;
			OnPropertyChanged();
		}
	}
}
