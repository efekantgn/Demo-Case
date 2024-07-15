using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public SceneField TargetScene;

    public void LoadTargetScene()
    {
        SceneTransitionManager.Instance.LoadScene(TargetScene);
    }
    public void LoadTargetSceneAsync()
    {
        SceneTransitionManager.Instance.LoadSceneAsync(TargetScene);
    }
}
