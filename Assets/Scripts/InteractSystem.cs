using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    public static InteractSystem FindSystem()
    {
        return FindFirstObjectByType<InteractSystem>();
    }
    [Header("General settings")]
    [SerializeField, Min(0.0f), Tooltip("Time interval in seconds between each scan")]
    private float _intervalSeconds = 0.05f;
    
    [System.Serializable]
    private struct SenderOptions
    {
        public Camera   fieldOfView;
        public float    maxDistance;
        public Vector2  screenOffset;
    }
    [SerializeField]
    private SenderOptions _senderOptions;
    private InteractTarget _hoveringTarget;
    void Awake()
    {
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
            yield return new WaitForSeconds(_intervalSeconds);
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
                Vector3.Normalize(_senderOptions.fieldOfView.transform.position - target.transform.position),
                _senderOptions.fieldOfView.transform.forward
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

        var pts = _senderOptions.fieldOfView.WorldToViewportPoint(target.transform.position);
        return (
            pts.x > _senderOptions.screenOffset.x && pts.x < 1f - _senderOptions.screenOffset.x &&
            pts.y > _senderOptions.screenOffset.y && pts.y < 1f - _senderOptions.screenOffset.y &&
            pts.z > 0 && 
            Vector3.Distance(
                target.transform.position,
                _senderOptions.fieldOfView.transform.position
            ) < _senderOptions.maxDistance
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