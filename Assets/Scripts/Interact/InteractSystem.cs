using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    public static InteractSystem Instance { get; private set; }
    
    [Header("Interact config")]
    [SerializeField] private float _scanPerSeconds = 20f;
    
    [Header("Sender config")]
    [SerializeField] private Camera _fieldOfView;
    [SerializeField] private float  _maxDistance = 10f;
    [SerializeField] private Vector2 _screenOffset = new (0.1f, 0.1f);
    private InteractTarget _hoveringTarget;
    void Awake()
    {
        Instance = this;
        _hoveringTarget = null;
    }
    void Start()
    {
        StartCoroutine(InteractSystemLoop());
    }
    void OnInteract()
    {
        if(_hoveringTarget != null)
        {
            _hoveringTarget.Activate();
            SwapTarget(null);
        }
    }
    private IEnumerator InteractSystemLoop()
    {
        while(true)
        {
            ScanForTarget();
            yield return new WaitForSeconds(1f/_scanPerSeconds);
        }
    }
    private void ScanForTarget()
    {
        var targets = new List<InteractTarget>(FindObjectsByType<InteractTarget>(FindObjectsSortMode.None));

        // filter out invalid target 
        int last = targets.Count;
        for(int i = last-1; i>=0; --i)
        {
            var target = targets[i];
            if(!ValidateTarget(target))
            {
                targets[i] = targets[--last];
            }
        }
        // check for valid target
        InteractTarget closest  = null;
        float closestDistance   = float.PositiveInfinity;
        for(int i = 0; i<last; ++i)
        {
            var target     = targets[i];
            var distance   = Vector3.Dot(
                Vector3.Normalize(_fieldOfView.transform.position - target.transform.position),
                _fieldOfView.transform.forward
            );
            if(distance < closestDistance)
            {
                closest = target;
                closestDistance = distance;
            }
        }
        SwapTarget(closest);
    }
    private bool ValidateTarget(InteractTarget target)
    {
        if(!target.gameObject.activeInHierarchy)
            return false;

        var pts = _fieldOfView.WorldToViewportPoint(target.transform.position);
        return (
            pts.x > _screenOffset.x && pts.x < 1f - _screenOffset.x &&
            pts.y > _screenOffset.y && pts.y < 1f - _screenOffset.y &&
            pts.z > 0 && 
            Vector3.Distance(
                target.transform.position,
                _fieldOfView.transform.position
            ) < _maxDistance
        );
    }
    public void SwapTarget(InteractTarget target)
    {
        
        var hovering = _hoveringTarget;
        if(target == hovering)
            return;
        _hoveringTarget = target;
        if(hovering != null)
            hovering.ToggleHover();
        if(target != null)
            target.ToggleHover();
    }
    public bool IsHovering(InteractTarget target) => target == _hoveringTarget;
}