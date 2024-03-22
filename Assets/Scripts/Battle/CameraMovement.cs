using UnityEngine;

namespace Battle
{
    public class CameraMovement : MonoBehaviour
    {
        private const float MinDistanceToTarget = 3f;
        private const float MaxDistanceToTarget = 16f;
        
        [SerializeField] private new Camera camera;
        [SerializeField] private Transform target;
        [SerializeField] private float distanceToTarget = 14;

        private Vector3 _previousPosition;

        private void Awake()
        {
            camera.transform.position = target.position;
            camera.transform.Translate(new Vector3(0, 0, -distanceToTarget));
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y > 0 && (camera.transform.position - target.transform.position).magnitude > MinDistanceToTarget)
                camera.transform.Translate(new Vector3(0, 0, 1f));
            if (Input.mouseScrollDelta.y < 0 && (camera.transform.position - target.transform.position).magnitude < MaxDistanceToTarget)
                camera.transform.Translate(new Vector3(0, 0, -1f));

            if (Input.GetMouseButtonDown(1))
            {
                _previousPosition = camera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 newPosition = camera.ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = _previousPosition - newPosition;

                float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                camera.transform.position = target.position;
            
                camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            
                camera.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                _previousPosition = newPosition;
            }
        }
    }
}