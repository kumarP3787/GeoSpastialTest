using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GroundPlane : MonoBehaviour
{
    [SerializeField] private GameObject _collisionPlane;
    [SerializeField]
    private ARPlaneManager planeManager;
    private Vector3 _newPosition;
    private float _minY;

    private void Start()
    {
        _collisionPlane.SetActive(false);
    }

    private void Update()
    {
        if(planeManager.trackables.count > 0)
        {
            if (!_collisionPlane.activeSelf) _collisionPlane.SetActive(true);

            /// Find and match Y val of lowest AR plane & hope that this is the "ground"
            _minY = Mathf.Infinity;
            foreach (ARPlane plane in planeManager.trackables)
            {
                if (plane.center.y < _minY)
                    _minY = plane.center.y;
            }

            _newPosition.Set(0, _minY, 0);
            transform.position = _newPosition;
        }
        else
        {
            if (_collisionPlane.activeSelf) _collisionPlane.SetActive(false);
        }
    }
}
