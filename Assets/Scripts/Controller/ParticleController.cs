using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private NcParticleSystem particlePrefab;

    [SerializeField]
    private Transform particlePrefabParentRoot;

	

    [SerializeField]
    private bool playOnLoad = true;
    void Awake()
    {
        if (particlePrefab != null)
        {
            Transform parentNode = transform;
            if (particlePrefabParentRoot != null)
                parentNode = particlePrefabParentRoot;
			GameObject go =  Instantiate(particlePrefab.gameObject,parentNode.position,Quaternion.identity) as GameObject;
			go.transform.SetParent(parentNode);
        }
        else
        {
            particlePrefab = GetComponent<NcParticleSystem>();
            if (null == particlePrefab)
                Debug.LogError(gameObject.name + ": ParticleController" + " Error : NcParticleSystem is Empty");
        }
    }
    void Start()
    {
        if (playOnLoad)
            PlayParticleSystem();
    }
    public void PlayParticleSystem(bool clearOldParticleObject = false)
    {
        if (null == particlePrefab)
            return;
        particlePrefab.OnResetReplayStage(clearOldParticleObject);
    }
}
