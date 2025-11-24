using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineToggle : MonoBehaviour
{
    private new Renderer renderer;
    private Material _outlineMat;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        _outlineMat = Resources.Load("Outline/OutlineMat", typeof(Material)) as Material;
    }
    public void Show()
    {
        List<Material> mats = new();
        renderer.GetMaterials(mats);
        int imat = mats.FindIndex(mat => mat.name.StartsWith(_outlineMat.name));
        if(imat == -1)
        {
            mats.Add(_outlineMat);
            renderer.SetMaterials(mats);
        }
    }
    public void Hide()
    {
        List<Material> mats = new();
        renderer.GetMaterials(mats);
        int imat = mats.FindIndex(mat => mat.name.StartsWith(_outlineMat.name));
        if(imat != -1)
        {
            mats.RemoveAt(imat);
            renderer.SetMaterials(mats);
        }
    }

}
