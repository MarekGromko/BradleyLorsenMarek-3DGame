using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTarget : MonoBehaviour
{
    [Header("Override options")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _hoverMaterial;

    // Properties
    public bool IsHover { get => InteractSystem.Instance.IsHovering(this); }
    
    // Events
    [Header("Events")]
    public UnityEvent<InteractTarget> OnActive;

    // Members
    private Material _hoverMaterialInstance;
    void Awake()
    {
        _hoverMaterialInstance = new Material(
            _hoverMaterial != null ?
            _hoverMaterial : 
            Resources.Load<Material>("Outline/DefaultOutline")
        );
    }
    public void Activate() => OnActive?.Invoke(this);
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
    private bool TryGetRenderer(out Renderer renderer)
    {
        if(_renderer != null)
        {
            renderer = _renderer;
            return true;
        }
        return TryGetComponent(out renderer);
    }
}