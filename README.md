## DebugService for Unity Games using Zenject & MVVM
System for easy value debugging.

**System requires LunarByte.MVVM to work** and a lot more other files :)

I created this system during my Lunarbyte internship in the summer of 2019 and it uses Lunarbyte's MVVM framework.

## How to use

### Value registration
DebugService has a configuration file where you just have to write the registration as follows:

You add your value's model as dependency to the configuration file and it gets there through zenject injection
```
public class DebugServiceConfiguration : ServiceConfiguration<DebugService, InputModel, MovementDebugModel>
{}
```
Then you just register it in the Configure method with following example:
```
protected override void Configure(DebugService service, InputModel inputModel, MovementDebugModel movementDebugModel)
	{
	
	//Example Registrations for InputModel data
	service.RegisterDebugValue(StepLengthOne, nameof(InputModel.InputType),
	    (float)inputModel.InputType, inputModel,
	    (model, f) => model.InputType = (InputType)(Mathf.Max(0,f) % 3));
```
Different parameters are as follows:
1. Steplengths are defined in the file and you just pick one and add it as first parameter.
2. You give it a name of which it can identify itself with and show in the view portion.
3. Then you give it default value
4. Then the Model itself which has ObservableProperties interface
5. Then you give it the setter with which it can update the model's value upon receiving inputs through the debug menu.
```
public void RegisterDebugValue<TObservableProperties>(float  stepLength,
	                                                      string name,
	                                                      float  defaultValue,
	                                                      TObservableProperties
		                                                      observableProperties,
	                                                      Action<TObservableProperties, float>
		                                                      propertySetter)
```
### Tester value adjustment
Here is a look at Unity Editor view of the hierarchy and UI of the debug menu.
DebuggableValueView prefab is only given height, rest is anchored and Content gameobject's **Content Size Fitter** and **Vertical Layout Group** components keep them nicely aligned.
Graphics are very primitive, but it gets the job done.
From registration to DebuggableValueView, they are created through DynamicFactory which is showcased in the **How it works** section.

#### Debug menu functionality:
- Increase and Decrease the value with + and - buttons the previously configured step length for each press.
- Hold + or - button to continuously increase the value, if you want drastic changes.
- You can reset the value to the saved value.
- You can Save the value if you want to hold is as a default when switching between values.

![DebugServiceHierarchy](https://github.com/Zimbe/Show_Case/blob/master/GithubImages/ShowCasePictures/DebugService.PNG)

You can also spot a ValuesToClipboardButton and yes, it does just what it says. It exports the values to Json file to your clipboard for easy sharing among the development team. Also importing is on TODOlist.
## How it works
System uses Zenject and MVVM framework from Lunarbyte.
### Dynamic View and ViewModel creation
First of all thevalue's addition to debug menu needs to be dynamic and it is created through [DynamicDebugViewConfiguration.cs](https://github.com/Zimbe/Show_Case/blob/master/DynamicDebugViewConfiguration.cs)

Debuggable data is stored in the actual model where it is stored and it is bound to automatically generated [DebuggableValueModel's](https://github.com/Zimbe/Show_Case/blob/master/DebuggableValueModel.cs) equilevant. Then the ViewModel and View are bound as well.
```
protected override void Configure(DynamicModelContainer<DebuggableValueModel> modelContainer,
	                                  DynamicViewModelContainer<DebuggableValueViewModel>
		                                  viewModelContainer,
	                                  DebugViewContainer         viewContainer,
	                                  DebugService               debugService,
	                                  DebuggableValueViewFactory debuggableValueViewFactory)
	{
		viewModelContainer.BindContainer()
		                  .ToContainer(modelContainer,
		                               model => ViewModelFactoryMethod(model, debugService));

		viewContainer.BindContainer().ToContainer(viewModelContainer,
		                                          vm => ViewFactoryMethod(
			                                          vm, debuggableValueViewFactory,
			                                          viewContainer.transform));
	}
```
Here is the dynamic creation and binding of [DebuggableValueViewModel](https://github.com/Zimbe/Show_Case/blob/master/DebuggableValueViewModel.cs)
```
private DebuggableValueViewModel ViewModelFactoryMethod(DebuggableValueModel model,
	                                                        DebugService         debugService)
	{
		var viewModel = new DebuggableValueViewModel
		{
			Value = model.Value,
			Name = model.Name
		};

		viewModel.Bind<DebuggableValueViewModel, float>(value => viewModel.Value = value)
		         .ToProperty(model, m => m.Value, nameof(DebuggableValueModel.Value));

		viewModel.Bind<DebuggableValueViewModel, string>(dName => viewModel.Name = dName)
		         .ToProperty(model, m => m.Name, nameof(DebuggableValueModel.Name));

		viewModel.IncreaseValueEvent.AddListener(model.IncreaseValue);
		viewModel.DecreaseValueEvent.AddListener(model.DecreaseValue);
		viewModel.ChangeValueInputFieldEvent.AddListener(model.ChangeValueInputField);
		viewModel.ResetValueEvent.AddListener(() => debugService.ResetValue(model));
		viewModel.SaveValueEvent.AddListener(() => debugService.SaveValue(model));

		return viewModel;
	}
```
Here is the dynamic creation and binding of [DebuggableValueView](https://github.com/Zimbe/Show_Case/blob/master/DebuggableValueView.cs)
```
private DebuggableValueView ViewFactoryMethod(DebuggableValueViewModel vm,
	                                              DebuggableValueViewFactory
		                                              debuggableValueViewFactory,
	                                              Transform parent)
	{
		DebuggableValueView view = debuggableValueViewFactory.Create(vm);
		view.transform.SetParent(parent, false);

		view.Bind<DebuggableValueView, float>(x => view.UpdateDebugValueText(x))
		    .ToProperty(vm, x => vm.Value, nameof(DebuggableValueViewModel.Value));

		view.Bind<DebuggableValueView, string>(x => view.SetDebugValueName(x))
		    .ToProperty(vm, x => vm.Name, nameof(DebuggableValueViewModel.Name));

		return view;
	}

```

## InitializationService for Unity Games using Zenject & MVVM
