using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Thoughtworks.xr.Real3d.Core.ContentProviders;
using UnityEngine;

public class DemoPrefabContentFactory : MonoBehaviour
{
    private List<GameObject> demoPrefabs;
    private int currentPrefabIndex = 0;
    private List<string> demos = new List<string>();

    private void Start()
    {

        demos.Add("graph");
        StartCoroutine(loadAllDemoPrefabsInResources());
        
    }

    private IEnumerator loadAllDemoPrefabsInResources()
    {
        Debug.Log("DATAPATH: "+Application.dataPath + "/Resources/Demos/");
        
#if UNITY_EDITOR        
        String[] files = Directory.GetFiles(Application.dataPath + "/Resources/Demos/","*.prefab");

        demoPrefabs = new List<GameObject>();
        GameObject prefab = null;
        foreach (var file in files)
        {
            Debug.Log("FOUNDPREFAB: " + Path.GetFileNameWithoutExtension(file));
            yield return prefab = Resources.Load<GameObject>("Demos/" + Path.GetFileNameWithoutExtension(file));
            demoPrefabs.Add(prefab);
        }
#elif UNITY_ANDROID
    //String[] files = Directory.GetFiles(Application.streamingAssetsPath + "/","*.manifest");
    demoPrefabs = new List<GameObject>();
    AssetBundle bundle;
    foreach (var file in demos)
    {
        Debug.Log("FOUNDPREFAB: " + Path.Combine(Application.streamingAssetsPath,
            file));
        yield return bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath,
            file));
        demoPrefabs.AddRange(bundle.LoadAllAssets<GameObject>());
    }
#endif
    }

    public GameObject getNextContent()
    {
        GameObject result = Instantiate(demoPrefabs[currentPrefabIndex++]);
        if (currentPrefabIndex >= demoPrefabs.Count) currentPrefabIndex = 0;
        return result;
    }
}
