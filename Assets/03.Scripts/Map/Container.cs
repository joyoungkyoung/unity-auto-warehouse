using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private int id = 0;
    [SerializeField]
    private Vector3 position = Vector3.zero;

    private void ChangeColor(Color color)
    {
        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.shader = Shader.Find("UI/Lit/Transparent");
            if (renderer.material.name.Equals("MatContainer (Instance)"))
            {
                renderer.material.color = color;
            } else
            {
                renderer.material.color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void Initialize()
    {
        this.id = 0;
        this.position = Vector3.zero;
        InitializeColor();
    }

    public void InitializeColor()
    {
        ChangeColor(new Color(0.5f, 0.5f, 0.5f, 0.5f));
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return id;
    }

    public void SetPosition(Vector3 vec)
    {
        this.position = vec;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void OnSelected()
    {
        ChangeColor(Color.cyan);
    }


}
