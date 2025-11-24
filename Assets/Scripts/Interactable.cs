using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isTarget = false;
    public bool IsCurerntTarget() => isTarget;
    public delegate void InteractHandler();
    public event InteractHandler InteractEvent;
    public void InteractWith() => InteractEvent?.Invoke();
}