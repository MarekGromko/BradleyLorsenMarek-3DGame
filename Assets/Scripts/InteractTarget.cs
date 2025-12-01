using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractTarget : MonoBehaviour
{
    [System.Serializable]
    private struct OverrideOptions
    {
        public Renderer renderer;
        public Material hoverMaterial;
    }
    [SerializeField]
    private OverrideOptions _override;
    private Material _hoverMaterialInstance;
    private InteractSystem _isys;
    void Awake()
    {
        _isys = FindFirstObjectByType<InteractSystem>();
        _hoverMaterialInstance = new Material(
            _override.hoverMaterial != null ?
            _override.hoverMaterial : 
            Resources.Load<Material>("Outline/DefaultOutline")
        );
    }
    bool TryGetRenderer(out Renderer renderer)
    {
        if(_override.renderer != null)
        {
            renderer = _override.renderer;
            return true;
        }
        return TryGetComponent(out renderer);
    }
    
    public bool IsHover { get => _isys.IsHovering(this); }
    public void ToggleHover()
    {
        if (!TryGetRenderer(out Renderer renderer))
            return;

        List<Material> mats = new(renderer.sharedMaterials);
        if(IsHover)
        {
            if(!mats.Contains(_hoverMaterialInstance))
            {
                mats.Add(_hoverMaterialInstance);
                renderer.sharedMaterials = mats.ToArray();
            }
        } else
        {
            if (mats.Contains(_hoverMaterialInstance))
            {
                mats.Remove(_hoverMaterialInstance);
                renderer.sharedMaterials = mats.ToArray();
            }
        }
    }
    public delegate void ActiveHandler();
    public event ActiveHandler OnActive;
    public void Activate() => OnActive?.Invoke();
}