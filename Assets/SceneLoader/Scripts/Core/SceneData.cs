using UnityEngine;
using UnityEngine.SceneManagement;

namespace JadesToolkit
{
    [System.Serializable]
    public class SceneData
    {
        public string Name { get => name; }
        public int Index { get => index; }

        [SerializeField] private string name;
        [SerializeField] private int index;
        public SceneData(string name, int index)
        {
            this.name = name;
            this.index = index;
        }

    }
}
