using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _minClampX;

    [SerializeField]
    float _maxClampX;

    [SerializeField]
    float _minClampZ;

    [SerializeField]
    float _maxClampZ;

    private Vector3 _lastPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _lastPos = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 diff = Input.mousePosition - _lastPos;
        Vector3 move = new Vector3();

        move.x = Mathf.Clamp(Camera.main.transform.position.x - diff.x, _minClampX, _maxClampX);
        move.y = Camera.main.transform.position.y;
        move.z = Mathf.Clamp(Camera.main.transform.position.z - diff.y, _minClampZ, _maxClampZ);

        Camera.main.transform.position = move;

        _lastPos = Input.mousePosition;
    }
}
