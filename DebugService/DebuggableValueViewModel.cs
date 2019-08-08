using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarByte.MVVM;

public class DebuggableValueViewModel : ViewModel, IDebuggableValueViewModel
{
	private float ValueField;
	private string NameField;
	public  Event  IncreaseValueEvent { get; } = new Event();
	public  Event  DecreaseValueEvent { get; } = new Event();

	public Event ResetValueEvent { get; } = new Event();
	public Event SaveValueEvent  { get; } = new Event();

	public Event<float> ChangeValueInputFieldEvent = new Event<float>();

	public string Identifier;

	public float Value
	{
		get { return ValueField; }
		set
		{
			ValueField = value;
			OnPropertyChanged();
		}
	}

	public string Name
	{
		get { return NameField; }
		set
		{
			NameField = value;
			OnPropertyChanged();
		}
	}

	public IDispatchableEvent IncreaseValue
	{
		get { return IncreaseValueEvent; }
	}

	public IDispatchableEvent DecreaseValue
	{
		get { return DecreaseValueEvent; }
	}

	public IDispatchableEvent<float> ChangeValueInputField
	{
		get { return ChangeValueInputFieldEvent; }
	}

	public IDispatchableEvent ResetValue
	{
		get { return ResetValueEvent; }
	}

	public IDispatchableEvent SaveValue
	{
		get { return SaveValueEvent; }
	}
}

public interface IDebuggableValueViewModel
{
	float             Value         { get; }
	string             Name          { get; }
	IDispatchableEvent IncreaseValue { get; }
	IDispatchableEvent DecreaseValue { get; }

	IDispatchableEvent<float> ChangeValueInputField { get; }
	IDispatchableEvent        ResetValue            { get; }
	IDispatchableEvent        SaveValue             { get; }
}
