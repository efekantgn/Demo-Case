using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCanvas : MonoBehaviour
{
    public GameObject m_MobileCanvas;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID 
        m_MobileCanvas.SetActive(true);
#endif
    }

}
