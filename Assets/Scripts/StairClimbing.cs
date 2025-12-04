using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class StairClimbing : MonoBehaviour
{
    [SerializeField] private InteractTarget stairTop;
    [SerializeField] private Vector3 topOffset = new ();
    [SerializeField] private InteractTarget stairBottom;
    [SerializeField] private Vector3 bottomOffset = new ();
    private GameObject _player;
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        stairBottom.OnActive.AddListener(GoStairTop);
        stairTop.OnActive.AddListener(GoStairBottom);
    }

    void GoStairTop(InteractTarget _)
    {
        _player.transform.position = stairTop.transform.position + topOffset;
    }
    void GoStairBottom(InteractTarget _)
    {
        _player.transform.position = stairBottom.transform.position + bottomOffset;
    }
}
