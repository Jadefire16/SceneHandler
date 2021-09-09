using JadesToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SceneLoadTrigger : MonoBehaviour
{
    [Header("Required")]
    [Space]
    [SerializeField] private int index;
    [Space]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private LoadBehaviour loadBehaviour;

    [Header("Pre Trigger")]
    [SerializeField] private bool triggerActive = true;
    [SerializeField] private bool loadSceneAdditively = false;
    [SerializeField] private bool activateByTrigger = false;
    [Header("After Trigger")]
    [SerializeField] private bool destroyOnTrigger = false;
    [SerializeField] private bool disableOnTrigger = true;
    [SerializeField] private bool resetOnTrigger = false;
    [SerializeField] private float resetOnTriggerdelay = 1f;



    private BoxCollider col;

    public void Start()
    {
        if (col == null)
            col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!activateByTrigger && other.isTrigger)
            return;
        if (collisionMask == (collisionMask | (1 << other.gameObject.layer)) && triggerActive)
        {
            switch (loadBehaviour)
            {
                case LoadBehaviour.Load:
                    SceneHandler.Manager.LoadSceneAsync(index, loadSceneAdditively ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single);
                    break;
                case LoadBehaviour.Unload:
                    SceneHandler.Manager.UnloadSceneAsync(index);
                    break;
            }
            if (disableOnTrigger)
            {
                col.enabled = false;
                triggerActive = false;
            }
        }
        if (destroyOnTrigger)
            Destroy(gameObject);
        else if (resetOnTrigger)
            StartCoroutine(ResetTimer(resetOnTriggerdelay));
    }

    private IEnumerator ResetTimer(float time)
    {
        var currentTime = Time.time + time;
        while(currentTime > Time.time)
        {
            yield return null;
        }
        ResetTrigger();
    }

    public void ResetTrigger()
    {
        triggerActive = true;
        col.enabled = true;
    }

    public void SetTrigger(bool val) => triggerActive = val;

}
