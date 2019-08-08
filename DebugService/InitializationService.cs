using System;
using System.Collections.Generic;
using LunarByte.HyperCasualSystems;
using UnityEngine;

// ReSharper disable IdentifierTypo

public class InitializationService : Service, IInitializationService
{
	private readonly IReadOnlyList<IResourceInitializer> ResourceInitializers;
	private readonly IReadOnlyList<IModelInitializer> ModelInitializers;
	private readonly IReadOnlyList<IServiceInitializer> ServiceInitializers;
	private readonly IReadOnlyList<ISceneInitializer> SceneInitializers;

	public InitializationService(List<IResourceInitializer> resourceInitializers,
	                             List<IModelInitializer>  modelInitializers,
	                             List<IServiceInitializer> serviceInitializers,
	                             List<ISceneInitializer>    sceneInitializers)
	{
		ResourceInitializers = resourceInitializers;
		ModelInitializers = modelInitializers;
		ServiceInitializers = serviceInitializers;
		SceneInitializers = sceneInitializers;
	}

	public void StartInitialization()
	{
		InitializeResources(ResourceInitializers);
	}

	private void InitializeResources(IReadOnlyList<IResourceInitializer> resourceInitializers)
	{
		var runningInitializers = new HashSet<IIdentifiable>(resourceInitializers);
		for (int i = 0; i < resourceInitializers.Count; i++)
		{
			var initializer = resourceInitializers[i];
			initializer.LoadResources(() => OnResourceLoadSuccess(initializer, runningInitializers), () => OnResourceLoadFailed(initializer, runningInitializers));

		}
	}

	private void OnResourceLoadSuccess(IIdentifiable identifiable, HashSet<IIdentifiable> runningInitializers)
	{
		runningInitializers.Remove(identifiable);
        if (runningInitializers.Count <= 0)
        {
	        InitializeModels(ModelInitializers);
        }
	}

	private void OnResourceLoadFailed(IIdentifiable identifiable, HashSet<IIdentifiable> runningInitializers)
	{
		Debug.LogError($"Failed to load resources with {identifiable.Identifier}");
		runningInitializers.Remove(identifiable);
		if (runningInitializers.Count <= 0)
		{
			InitializeModels(ModelInitializers);
		}
    }

	

	private void InitializeModels(IReadOnlyList<IModelInitializer> modelInitializers)
	{
		for (int i = 0; i < modelInitializers.Count; i++)
		{
			modelInitializers[i].Initialize();
		}
		InitializeServices(ServiceInitializers);
	}

	private void InitializeServices(IReadOnlyList<IServiceInitializer> serviceInitializers)
	{
		for (int i = 0; i < serviceInitializers.Count; i++)
		{
			serviceInitializers[i].Initialize();
		}
		InitializeScenes(SceneInitializers);

    }

    private void InitializeScenes(IReadOnlyList<ISceneInitializer> sceneInitializers)
	{
		var runningInitializers = new HashSet<IInitializer>(sceneInitializers);
		for (int i = 0; i < sceneInitializers.Count; i++)
		{
			sceneInitializers[i].Initialize();
		}
    }
	
}
