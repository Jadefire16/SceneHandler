using JadesToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneHandler = JadesToolkit.SceneHandler;

public class TestScript : MonoBehaviour
{
    [SerializeField] private SceneHandler manager;
    private static int index = 1;

    private void Start()
    {
        SceneHandler.SceneLoadStart += () =>
        {
            Debug.Log("Starting");
        };
        SceneHandler.SceneLoading += (float val) =>
        {
            Debug.Log($"Loading Scene Percent {val}");
        }; 
        SceneHandler.SceneLoaded += (SceneData scene) =>
        {
            Debug.Log($"Finished loading Scene: {scene.Name}");
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            manager.LoadSceneAsync(index, LoadSceneMode.Additive);
            index++;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            manager.UnloadSceneAsync(index - 1);
            index--;
        }
        
    }

}
