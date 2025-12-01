using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class InteractPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _labelTarget;
    [SerializeField]
    private TextMeshPro _keyTarget;
    private RectTransform _rect;
    private InteractSystem _isys;
    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _isys = InteractSystem.FindSystem();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Targeting(Vector3 target)
    {

    }
}
