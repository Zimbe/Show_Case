using System;
using LunarByte.HyperCasualSystems;
using LunarByte.MVVM;

public class DebugService : Service, IDebugService
{
	public const     float                                       StepLengthOne          = 1;
	public const     float                                       StepLengthHalf         = 0.5f;
	public const     float                                       StepLengthOneHundredth = 0.01f;
	public const     float                                       StepLengthOneTenth     = 0.1f;
	public const     float                                       StepLengthTen          = 10;
	private readonly DynamicModelContainer<DebuggableValueModel> DebuggableValuesContainer;
	private readonly ISaveService                                 SaveService;

	public DebugService(ISaveService                                 saveService,
	                    DynamicModelContainer<DebuggableValueModel> debuggableValuesContainer)
	{
		SaveService = saveService;
		DebuggableValuesContainer = debuggableValuesContainer;
	}

	public void RegisterDebugValue<TObservableProperties>(float  stepLength,
	                                                      string name,
	                                                      float  defaultValue,
	                                                      TObservableProperties
		                                                      observableProperties,
	                                                      Action<TObservableProperties, float>
		                                                      propertySetter)
		where TObservableProperties : class, IObservableProperties
	{
		var debuggableValue = new DebuggableValueModel
		{
			Name = name,
			StepLength = stepLength
		};

		bool saveDoesNotExist = !SaveService.TryLoad(debuggableValue);

		if (saveDoesNotExist)
		{
			debuggableValue.Value = defaultValue;
			SaveValue(debuggableValue);
		}

		observableProperties
			.Bind<TObservableProperties, float>(val => propertySetter(observableProperties, val))
			.ToProperty(debuggableValue, dv => dv.Value, nameof(DebuggableValueModel.Value));

		DebuggableValuesContainer.Add(debuggableValue);
	}

	private void LoadValue(DebuggableValueModel debuggableValue)
	{
		SaveService.Load(debuggableValue);
	}

	public void SaveValue(DebuggableValueModel debuggableValue)
	{
		SaveService.Save(debuggableValue);
	}

	public void ResetValue(DebuggableValueModel debuggableValue)
	{
		LoadValue(debuggableValue);
	}
}
