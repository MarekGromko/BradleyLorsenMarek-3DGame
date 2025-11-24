using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;

public class PlayerInteractFinder : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxInteractableDistance = 10f;
    private Interactable _currentTarget = null;
    private bool IsInteractable(Interactable inter)
    {
        if(!inter.gameObject.activeInHierarchy) return false;
        var pts = mainCamera.WorldToViewportPoint(inter.gameObject.transform.position);
        return (
            pts.x > .2 && pts.x < .8 &&
            pts.y > .2 && pts.y < .8 &&
            pts.z > 0
        );
    }
    public void OnInteract()
    {
        Debug.Log("Interact");
        if(_currentTarget) _currentTarget.InteractWith();
    }
    void Update()
    {
        var interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None)
            .Where(IsInteractable)
            .ToArray();
        
        Interactable closest = null;
        foreach(var inter in interactables) {
            var dist = Vector3.Distance(inter.gameObject.transform.position, mainCamera.transform.position);
            if(dist > maxInteractableDistance) 
                continue;

            if(
                closest == null || 
                dist < Vector3.Distance(closest.gameObject.transform.position, mainCamera.transform.position)
            )
            {
                closest = inter;
            }
        }

        if(closest == null)
        {
            if(_currentTarget != null)
            {
                _currentTarget.isTarget = false;
                _currentTarget = null;
            }
        } else
        {
            if(_currentTarget != null)
                _currentTarget.isTarget = false;
            _currentTarget = closest;
            closest.isTarget = true;
        }
    }
}