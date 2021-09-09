using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JadesToolkit
{
    public class SceneHandler : MonoBehaviour
    {
        public static event Action SceneLoadStart, SceneUnloadStart; // sent out when loading and unloading starts, may be useful for debugging and such

        public static event Action<float> SceneLoading, SceneUnloading; // percentage

        public static event Action<SceneData> SceneLoaded, SceneUnloaded; // sends the scene out on load

        public SceneData MainScene { get; private set; }

        private static SceneHandler instance; // I still hate Unity singletons
        public static SceneHandler Manager
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SceneHandler>();
                    if (instance == null)
                        instance = new GameObject("SceneManager Instance").AddComponent<SceneHandler>();
                }
                return instance;
            }
        }

        private Dictionary<string, int> scenePairs = new Dictionary<string, int>(2);
        private Dictionary<int, SceneData> sceneDatas = new Dictionary<int, SceneData>(2);
        
        private List<SceneData> activeScenes = new List<SceneData>(4);

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                Debug.LogWarning("Instance already exists! Please ensure only one SceneManager at a time is present");
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeScenes();
        }

        private void InitializeScenes()
        {
            int scenesInBuild = SceneManager.sceneCountInBuildSettings;
            var currentScene = SceneManager.GetActiveScene();
            for (int i = 0; i < scenesInBuild; i++)
            {
                string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
                string name = System.IO.Path.GetFileNameWithoutExtension(pathToScene); // Unity really needs to fix their bullshit
                var data = new SceneData(name, i);
                if (data.Name.Equals(string.Empty) || data.Index < 0)
                {
                    Debug.LogError($"Attempted to load an empty scene! Skipping scene at index: {i}");
                    continue;
                }
                scenePairs.Add(data.Name, data.Index); // Map keys from string to int for "faster" fetching for a more user firendly interface
                sceneDatas.Add(data.Index, data);
            }
            sceneDatas.TryGetValue(currentScene.buildIndex, out SceneData value);
            MainScene = value ?? sceneDatas[0];
            activeScenes.Add(MainScene);
#if UNITY_EDITOR
            Debug.Log($"Initialized {scenesInBuild} Scenes and set {currentScene.name} as Main Scene!");
#endif
        }

        /// <summary>
        /// Loads a scene specified at the given key additively.
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="ArgumentException"></exception>
        public void LoadSceneAsync(string key, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (scenePairs.TryGetValue(key, out int index))
                LoadSceneAsync(index, mode);
            else
                throw new ArgumentException("Key was not present!");
        }

        /// <summary>
        /// Loads a scene specified at the given index additively.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void LoadSceneAsync(int index, LoadSceneMode mode = LoadSceneMode.Single)
        {
            sceneDatas.TryGetValue(index, out SceneData data);
            if (data == null)
                throw new NullReferenceException($"No Scene was found at Index: {index}");
            StartCoroutine(LoadScene(data, mode));
        }

        private IEnumerator LoadScene(SceneData scene, LoadSceneMode mode)
        {
            SceneLoadStart?.Invoke();
            var op = SceneManager.LoadSceneAsync(scene.Index, mode);
            while (!op.isDone)
            {
                SceneLoading?.Invoke(op.progress);
                yield return null;
            }
            activeScenes.Add(scene);
            SceneLoaded?.Invoke(scene);
            yield return null;
        }

        public void UnloadSceneAsync(string key)
        {
            if (scenePairs.TryGetValue(key, out int index))
                UnloadSceneAsync(index);
            else
                throw new ArgumentException("Key was not present!");
        }

        public void UnloadSceneAsync(int index)
        {
            sceneDatas.TryGetValue(index, out SceneData data);
            if (data == null)
                throw new NullReferenceException("SceneData was null!");
            StartCoroutine(UnloadScene(data));
        }

        private IEnumerator UnloadScene(SceneData scene)
        {
            SceneUnloadStart?.Invoke();
            var op = SceneManager.UnloadSceneAsync(scene.Index);
            while (!op.isDone)
            {
                SceneUnloading?.Invoke(op.progress);
                yield return null;
            }
            activeScenes.Add(scene);
            SceneUnloaded?.Invoke(scene);
            yield return null;
        }

    }
}
