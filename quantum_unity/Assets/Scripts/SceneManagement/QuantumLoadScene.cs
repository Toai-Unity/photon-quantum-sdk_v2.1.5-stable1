using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class QuantumLoadScene : MonoBehaviour
{
    [SerializeField] private GameSceneSO _testingGroundScene = default;
    private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;
    private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();

    private void Start()
    {
        _gameplayManagerLoadingOpHandle = _testingGroundScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        _gameplayManagerLoadingOpHandle.WaitForCompletion();
        _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;
    }
}
