using LunarByte.MVVM;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DynamicDebugViewConfiguration),
	menuName = "Debug/Configurations/DynamicDebugView")]
public class DynamicDebugViewConfiguration : DynamicViewConfiguration<
	DynamicModelContainer<DebuggableValueModel>, DynamicViewModelContainer<DebuggableValueViewModel>
	, DebugViewContainer, DebugService,
	DebuggableValueViewFactory>
{
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
}
