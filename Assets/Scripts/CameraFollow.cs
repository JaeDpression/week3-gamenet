using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    private Transform _target;
    public Vector3 offset = new Vector3(0, 5, -7);

    public void SetTarget(Transform target) => _target = target;

    private void LateUpdate()
    {
        if (_target == null) return;
        transform.position = _target.position + offset;
        transform.LookAt(_target.position);
    }
}