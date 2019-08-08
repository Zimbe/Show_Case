using LunarByte.MVVM;
using LunarByte.MVVM.Configurations;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = nameof(DebugServiceConfiguration),
	menuName = "Debug/Configurations/Service")]
public class DebugServiceConfiguration : ServiceConfiguration<DebugService, InputModel, MovementDebugModel>

{
	private const            float        StepLengthOne          = 1;
	private const            float        StepLengthHalf         = 0.5f;
	private const            float        StepLengthOneHundredth = 0.01f;
	private const            float        StepLengthOneTenth     = 0.1f;
	private const            float        StepLengthTen          = 10;
	[SerializeField] private GameObject   DebugViewPrefab;

	protected override void Bind(DiContainer container)
	{
		container.Bind<DynamicModelContainer<DebuggableValueModel>>().ToSelf().AsCached();

		container.BindFactory<IDebuggableValueViewModel, DebuggableValueView, DebuggableValueViewFactory>()
		         .FromMonoPoolableMemoryPool(x => x.WithInitialSize(10)
		                                           .FromComponentInNewPrefab(DebugViewPrefab)
		                                           .UnderTransformGroup(
			                                           $"{nameof(DebugViewPrefab)} parent"));
	}


	protected override void Configure(DebugService service, InputModel inputModel, MovementDebugModel movementDebugModel)
	{
		service.RegisterDebugValue(StepLengthOne, nameof(InputModel.InputType),
		                           (float)inputModel.InputType, inputModel,
		                           (model, f) => model.InputType = (InputType)(Mathf.Max(0,f) % 3));

        service.RegisterDebugValue(StepLengthOneHundredth, nameof(MovementDebugModel.DrawInputMargin),
            (float)movementDebugModel.DrawInputMargin, movementDebugModel,
            (model, f) => model.DrawInputMargin = Mathf.Abs(f));

        service.RegisterDebugValue(StepLengthTen, nameof(MovementDebugModel.JoystickSensitivity),
            (float)movementDebugModel.JoystickSensitivity, movementDebugModel,
            (model, f) => model.JoystickSensitivity = Mathf.Abs(f));

        service.RegisterDebugValue(StepLengthOneTenth, nameof(MovementDebugModel.LineSectionLength),
            (float)movementDebugModel.LineSectionLength, movementDebugModel,
            (model, f) => model.LineSectionLength = Mathf.Abs(f));

        service.RegisterDebugValue(StepLengthOneTenth, nameof(MovementDebugModel.MovementSpeed),
            (float)movementDebugModel.MovementSpeed, movementDebugModel,
            (model, f) => model.MovementSpeed = Mathf.Abs(f));

        service.RegisterDebugValue(StepLengthOneTenth, nameof(MovementDebugModel.PlayerDrawAreaSize),
            (float)movementDebugModel.PlayerDrawAreaSize, movementDebugModel,
            (model, f) => model.PlayerDrawAreaSize = Mathf.Abs(f));
    }

	
}
