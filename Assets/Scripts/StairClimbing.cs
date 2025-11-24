using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(OutlineToggle))]
public class StairClimbing : MonoBehaviour
{
    [SerializeField] private Interactable stairTop;
    [SerializeField] private Vector3 topOffset = new ();
    [SerializeField] private Interactable stairBottom;
    [SerializeField] private Vector3 bottomOffset = new ();

    private OutlineToggle _olt;
    private GameObject _player;
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _olt = GetComponent<OutlineToggle>();
        stairBottom.InteractEvent += GoStairTop;
        stairTop.InteractEvent += GoStairBottom;
    }

    void GoStairTop()
    {
        _player.transform.position = stairTop.transform.position + topOffset;
    }
    void GoStairBottom()
    {
        _player.transform.position = stairBottom.transform.position + bottomOffset;
    }
    void Update()
    {
        if(stairTop.isTarget || stairBottom.isTarget)
            _olt.Show();
        else 
            _olt.Hide();
    }
}
