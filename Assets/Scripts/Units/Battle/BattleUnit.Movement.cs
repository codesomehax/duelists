using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace Units.Battle
{
    public partial class BattleUnit
    {
        private static readonly Quaternion HostUnitsRotation = Quaternion.identity;
        private static readonly Quaternion ClientUnitsRotation = Quaternion.Euler(0, 180, 0);

        private const float RotationSpeed = 10f;
        private float MovementSpeed => battleUnitData.MovementSpeed;

        [Header("Movement")]
        [SerializeField] private Transform movementTransform;

        private bool _isMoving;
        private Vector3 _flatMovementDestination;
        private Queue<Vector3> _movementPath = new();
        private Vector3 FlatMovementTransformPosition => new(movementTransform.position.x, 0f, movementTransform.position.z);
        private bool CurrentDestinationReached => (FlatMovementTransformPosition - _flatMovementDestination).magnitude < 0.1f;

        [Server]
        private void RotateTowardsEnemy()
        {
            transform.rotation = Owner.IsHost ? HostUnitsRotation : ClientUnitsRotation;
        }

        public void FollowPath(IList<Vector3> path)
        {
            _movementPath = new Queue<Vector3>(path);
            MoveToNextPosition();
            _isMoving = true;
        }

        private void MoveToNextPosition()
        {
            _flatMovementDestination = _movementPath.Dequeue();
            _flatMovementDestination.y = 0f;
        }

        private void MoveUpdate()
        {
            if (!_isMoving) return;

            if (!CurrentDestinationReached)
            {
                Vector3 direction = (_flatMovementDestination - FlatMovementTransformPosition).normalized;
                Vector3 movementPerFrame = direction * (MovementSpeed * Time.deltaTime);
                transform.position += movementPerFrame;

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
            }
            else if (_movementPath.Count != 0)
                MoveToNextPosition();
            else
                _isMoving = false;
        }
    }
}