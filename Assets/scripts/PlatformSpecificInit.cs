using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecificInit : MonoBehaviour
{
    public List<GameObject> commonToDisableGameObjects;
    public List<GameObject> commonToEnableGameObjects;
    public List<GameObject> H_toDisableGameObjects;
    public List<GameObject> H_toEnableGameObjects;
    public List<GameObject> SW_toDisableGameObjects;
    public List<GameObject> SW_toEnableGameObjects;

    private void Awake()
    {
        initGameObjects();
    }

    private void initGameObjects()
    {
        foreach (GameObject theGameObject in commonToDisableGameObjects)
        {
            theGameObject.SetActive(false);
        }

        foreach (GameObject theGameObject in commonToEnableGameObjects)
        {
            theGameObject.SetActive(true);
        }
#if UNITY_STANDALONE || UNITY_WEBGL
        foreach (GameObject theGameObject in SW_toDisableGameObjects)
        {
            theGameObject.SetActive(false);
        }

        foreach (GameObject theGameObject in SW_toEnableGameObjects)
        {
            theGameObject.SetActive(true);
        }
#elif UNITY_ANDROID || UNITY_IPHONE
        foreach (GameObject theGameObject in H_toDisableGameObjects)
        {
            theGameObject.SetActive(false);
        }

        foreach (GameObject theGameObject in H_toEnableGameObjects)
        {
            theGameObject.SetActive(true);
        }
#endif

    }
}
