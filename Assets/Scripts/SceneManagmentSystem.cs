using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagmentSystem : MonoBehaviour
{
    [SerializeField] SceneField gameScene;
    public void OpenGameScene()
    {
        SceneTransitionManager.Instance.LoadScene(gameScene);
    }
}
