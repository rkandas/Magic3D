using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Thoughtworks.xr.Real3d.Core.ContentProviders
{
    public class MockContentFactory : MonoBehaviour
    {
        [SerializeField] private List<Material> materials;
        [SerializeField] private Vector3 bounds;
        [SerializeField] private int maxObjects = 20;
        
        public virtual GameObject getMockContents()
        {
            GameObject content = new GameObject("MockContent");
            for (int i = 0; i < maxObjects; i++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.GetComponent<Renderer>().material = materials[Random.Range(0,materials.Count)];
                obj.transform.position = new Vector3(Random.Range(0f, bounds.x), Random.Range(0f, bounds.y),
                    Random.Range(0f, bounds.z));
                float scale = Random.Range(0.1f, 0.5f);
                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.transform.parent = content.transform;
                
            }
            content.transform.position = new Vector3(0 - (bounds.x / 2f), 0 - (bounds.y / 2f), 0);
            return content;
        }
    }
}