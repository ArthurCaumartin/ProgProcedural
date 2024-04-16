using UnityEngine;

public class MouseAim : MonoBehaviour
{
    [SerializeField] private Vector3 _mousePointerPose = Vector3.zero;

    private void LateUpdate()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hit = Physics.RaycastAll(camRay, Mathf.Infinity);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.layer == 10)
            {
                _mousePointerPose = hit[i].point;
                _mousePointerPose.y = 0;
                break;
            }
        }

        transform.LookAt(_mousePointerPose);
    }

    public Vector3 GetMousePos()
    {
        return _mousePointerPose;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_mousePointerPose, .05f);
    }
}
