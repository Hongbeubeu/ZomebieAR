using DG.Tweening;
using UnityEngine;


public class CameraLook : MonoBehaviour
{
    #region PUBLIC FIELDS

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
    [SerializeField] private Transform m_YAxis;
    [SerializeField] private Transform m_XAxis;
    private Camera m_camera;
    private bool onLockVision;

    #endregion

    #region UNITY_ROUTINES

    private void Start()
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
    }

    private void RotatePlayY()
    {
//        m_parent.Rotate(Vector3.up * m_mouseX);
        m_rotateY += m_mouseX;
        // m_rotateY = Mathf.Clamp(m_rotateY, cameraRotateYMin, cameraRotateYMax);
        m_XAxis.localRotation = Quaternion.Euler(0f, m_rotateY, 0f);
    }

    private void RotateCameraX()
    {
        m_rotateX += m_mouseY;
        m_rotateX = Mathf.Clamp(m_rotateX, cameraRotateXMin, cameraRotateXMax);
        m_YAxis.localRotation = Quaternion.Euler(-m_rotateX, 0f, 0f);
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

    public void LookAtEnemy(Transform target)
    {
        onLockVision = true;
        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 des = Quaternion.LookRotation(dir).eulerAngles;
        transform.DORotate(des, 0.7f)
            .SetEase(Ease.OutBack);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
        m_YAxis.rotation = Quaternion.identity;
    }

    #endregion
}