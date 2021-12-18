using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.SDF;

public class VFXUpdateSkinnedSDF : MonoBehaviour
{
    [SerializeField] private int _maxResolution = 64;
    [SerializeField] private Vector3 _center = Vector3.zero;
    [SerializeField] private Vector3 _sizeBox = Vector3.one;
    [SerializeField] private int _signPassCount = 1;
    [SerializeField] private float _threshold = 0.5f;

    private MeshToSDFBaker _baker;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Mesh _mesh;
    private VisualEffect _vfx;
    
    private void Start()
    {
        _mesh = new Mesh();
        _vfx = GetComponent<VisualEffect>();
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _skinnedMeshRenderer.BakeMesh(_mesh);
        _baker = new MeshToSDFBaker(_sizeBox, _center, _maxResolution, _mesh, _signPassCount, _threshold);
        _baker.BakeSDF();
        _vfx.SetTexture("WalkingSDF", _baker.SdfTexture);
        _vfx.SetVector3("BoxSize", _baker.GetActualBoxSize());
    }

    private void Update()
    {
        _skinnedMeshRenderer.BakeMesh(_mesh);
        _baker.BakeSDF();
        _vfx.SetTexture("WalkingSDF", _baker.SdfTexture);
    }

    private void OnDestroy()
    {
        if (_baker != null)
        {
            _baker.Dispose();
        }
    }
}
