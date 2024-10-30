using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _underRoot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _underRoot.Rotate(Input.GetAxis("Mouse Y") * Time.deltaTime * 20, 0, 0);
            transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * 20, 0);
        }
    }
}