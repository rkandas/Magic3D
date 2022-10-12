using System.Collections;
using System.Collections.Generic;
using Thoughtworks.xr.Real3d.Core.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Thoughtworks.xr.Real3d.Core.ContentProviders
{
    [RequireComponent(typeof(MockContentFactory))]
    public class SceneContentProvider : MonoBehaviour
    {
        private GameObject currentContent;
        private MockContentFactory factory;
        private DemoPrefabContentFactory demoFactory;

        private SwipeInteraction swipeProvider;

        private void Start()
        {
            factory = GetComponent<MockContentFactory>();
            demoFactory = GetComponent<DemoPrefabContentFactory>();
            swipeProvider = GetComponent<SwipeInteraction>();
            if (swipeProvider != null)
            {
                swipeProvider.OnSwipeLeft.AddListener(new UnityAction(GenerateNewContent));
                swipeProvider.OnSwipeRight.AddListener(new UnityAction(GenerateNewContent));
            }
            
            GenerateNewContent();
        }

        private void GenerateNewContent()
        {
            StartCoroutine(GenerateContent());

        }

        private IEnumerator GenerateContent()
        {
            yield return new WaitForEndOfFrame();
            if(currentContent != null) DestroyImmediate(currentContent);
            GameObject content;
            if (demoFactory != null)
            {
                content = demoFactory.getNextContent();
            }
            else
            {
                content = factory.getMockContents();
            }
            content.transform.parent = transform;
            currentContent = content;
           Animation defaultAnimation = content.GetComponent<Animation>();
           
            if (defaultAnimation != null)
            {
                foreach (AnimationState state in defaultAnimation)
                {
                    if(state.name.Equals("sundaranim"))
                        state.speed = 2f;
                    else
                    {
                        state.speed = 0.05f;
                    }
                }
                defaultAnimation.Play();
            }
        }
    }
}
