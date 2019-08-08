using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebuggableValueView : DynamicView<IDebuggableValueViewModel>
{
	private bool DebugValuesInitiallyLoaded;

    [SerializeField] public TextMeshProUGUI DebugValueNameText;
	[SerializeField] public TMP_InputField DebugValueText;
	[SerializeField] private Button          MinusButton;
	[SerializeField] private Button          PlusButton;
	[SerializeField] private Button          ResetButton;
	[SerializeField] private Button          SaveButton;

	protected override void OnViewAwake()
	{
		PlusButton.onClick.AddListener(IncreaseValue);
		MinusButton.onClick.AddListener(DecreaseValue);
		ResetButton.onClick.AddListener(ResetValue);
		SaveButton.onClick.AddListener(SaveValue);

		DebugValueText.onSubmit.AddListener(delegate
		{
			AddValueInputField(float.Parse(DebugValueText.text));
		});
	}

	protected override void OnViewStart()
	{
		InitialValueLoad();
	}

	public void UpdateDebugValueText(float debugValue)
	{
		DebugValueText.text = Truncate(debugValue.ToString(), 8);
	}

	public void SetDebugValueName(string debugName)
	{
		DebugValueNameText.text = debugName;
	}

	public void AddValueInputField(float value)
	{
		ViewModel.ChangeValueInputField.Dispatch(value);
	}

	public void IncreaseValue()
	{
		ViewModel.IncreaseValue.Dispatch();
	}

	public void DecreaseValue()
	{
		ViewModel.DecreaseValue.Dispatch();
	}

	public void ResetValue()
	{
		ViewModel.ResetValue.Dispatch();
	}

	public void SaveValue()
	{
		ViewModel.SaveValue.Dispatch();
	}

	private string Truncate(string input, int truncLength)
	{
		return !string.IsNullOrEmpty(input) && input.Length >= truncLength ?
			input.Substring(0, truncLength) :
			input;
	}

	private void InitialValueLoad()
	{
		if (!DebugValuesInitiallyLoaded)
		{
			ResetValue();
			SaveValue();
			DebugValuesInitiallyLoaded = true;
		}
	}
}
