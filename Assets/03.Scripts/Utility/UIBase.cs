using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBase : MonoBehaviour
{
    public bool isOpend => gameObject.activeInHierarchy;

    [SerializeField]
    private RectTransform mRtf;
    public RectTransform rtf { get { return mRtf; } set { mRtf = value; } }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}