using System;
using UnityEngine;


public class CameraLook : MonoBehaviour
{
    #region PUBLIC FIELDS

    [Header("Camera Field Of View")] public float cameraFieldOfViewMin;
    public float cameraFieldOfViewMax;
    public float fieldOfViewIncrement;
    public float cameraRotateXMin;
    public float cameraRotateXMax;
    public float cameraRotateYMin;
    public float cameraRotateYMax;

    [Header("Mouse Smoothing")] public float mouseSmooth;

    #endregion

    #region PRIVATE FIELDS

    private float m_mouseX;
    private float m_mouseY;
    private float m_rotateX;
    private float m_rotateY;
    private float m_mouseScrollWheel;
    private Transform m_parent;
    private Camera m_camera;
    private float m_fieldOfView;
    private bool onLockVision;

    #endregion

    #region UNITY_ROUTINES

    private void Awake()
    {
        m_parent = transform.parent;
        m_camera = Camera.main;
        if (m_camera != null) m_fieldOfView = m_camera.fieldOfView;
    }

    private void OnEnable()
    {
        LockMouse();
        ResetRotation();
    }

    private void Update()
    {
        if (onLockVision)
        {
            return;
        }

        MouseInput();
        RotatePlayY();
        RotateCameraX();
        //        CameraZoom();
    }

    #endregion

    #region HELPER ROUTINES

    private void MouseInput()
    {
        m_mouseX = Input.GetAxisRaw("Mouse X") * mouseSmooth;
        m_mouseY = Input.GetAxisRaw("Mouse Y") * mouseSmooth;
        m_mouseScrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
    }

    private void RotatePlayY()
    {
        //        m_parent.Rotate(Vector3.up * m_mouseX);
        m_rotateY += m_mouseX;
        m_rotateY = Mathf.Clamp(m_rotateY, cameraRotateYMin, cameraRotateYMax);
        m_parent.localRotation = Quaternion.Euler(0f, m_rotateY, 0f);
    }

    private void RotateCameraX()
    {
        m_rotateX += m_mouseY;
        m_rotateX = Mathf.Clamp(m_rotateX, cameraRotateXMin, cameraRotateXMax);
        m_camera.transform.localRotation = Quaternion.Euler(-m_rotateX, 0f, 0f);
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        onLockVision = false;
    }

    public void UnLockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        onLockVision = true;
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
        m_parent.rotation = Quaternion.identity;
    }

    private void CameraZoom()
    {
        if (m_mouseScrollWheel > 0.0f)
        {
            if (m_fieldOfView + fieldOfViewIncrement >= cameraFieldOfViewMin &&
                m_fieldOfView + fieldOfViewIncrement <= cameraFieldOfViewMax)
            {
                m_fieldOfView += fieldOfViewIncrement;
                m_camera.fieldOfView = m_fieldOfView;
            }
        }

        if (m_mouseScrollWheel < 0.0f)
        {
            if (m_fieldOfView - fieldOfViewIncrement >= cameraFieldOfViewMin &&
                m_fieldOfView - fieldOfViewIncrement <= cameraFieldOfViewMax)
            {
                m_fieldOfView -= fieldOfViewIncrement;
                m_camera.fieldOfView = m_fieldOfView;
            }
        }
    }

    #endregion
}