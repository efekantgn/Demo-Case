using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseGameObject : MonoBehaviour
{
    public void OpenCloseGO()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
