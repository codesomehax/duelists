using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Units.Battle
{
    public partial class BattleUnit
    {
        private static readonly Quaternion HostUnitsRotation = Quaternion.identity;
        private static readonly Quaternion ClientUnitsRotation = Quaternion.Euler(0, 180, 0);

        private const float RotationSpeed = 10f;

        public event Action<NetworkConnection> OnDestinationReached;
        
        private float MovementSpeed => battleUnitData.MovementSpeed;

        [Header("Movement")]
        [SerializeField] private Transform movementTransform;

        [Server]
        private void RotateTowardsEnemySide()
        {
            transform.rotation = Owner.IsHost ? HostUnitsRotation : ClientUnitsRotation;
        }

        public void FollowPath(IList<Vector3> path)
        {
            StartCoroutine(MoveCoroutine(new Queue<Vector3>(path)));
        }

        private IEnumerator MoveCoroutine(Queue<Vector3> path)
        {
            Animate(AnimationType.Run);
            while (path.Count != 0)
            {
                Vector3 flatMovementDestination = path.Dequeue();
                flatMovementDestination.y = 0f;
                
                Vector3 FlatMovementTransformPosition() => new(movementTransform.position.x, 0f, movementTransform.position.z);
                bool CurrentDestinationReached() => (FlatMovementTransformPosition() - flatMovementDestination).magnitude < 0.1f;
                
                while (!CurrentDestinationReached())
                {
                    Vector3 direction = (flatMovementDestination - FlatMovementTransformPosition()).normalized;
                    Vector3 movementPerFrame = direction * (MovementSpeed * Time.deltaTime);
                    transform.position += movementPerFrame;

                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

                    yield return null;
                }
            }
            
            Animate(AnimationType.Idle);
            StartCoroutine(SetRotationCoroutine(Owner.IsHost ? HostUnitsRotation : ClientUnitsRotation));

            OnDestinationReached?.Invoke(Owner);
        }

        private IEnumerator SetRotationCoroutine(Quaternion rotation)
        { 
            bool LookingAtRotation() => Quaternion.Angle(transform.rotation, rotation) < 0.1f;
            while (!LookingAtRotation())
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                yield return null;
            }
        }

        public void AttackUnitAtPosition(Vector3 unitPosition, AnimationType attackAnimationType)
        {
            // _flatRotateDirection = new Vector3(unitPosition.x, 0f, unitPosition.z) 
            //                        - FlatMovementTransformPosition;
            // _attackAnimationType = attackAnimationType;
            // _isRotatingTowardsPosition = true;
            // _isAttacking = true;
        }

        #region RotateTowardsPositionAndAttackUpdate
        private bool _isRotatingTowardsPosition;
        private Vector3 _flatRotateDirection;
        private bool LookingAtPosition => Vector3.Angle(movementTransform.forward, _flatRotateDirection) < 0.1f;

        private bool _isAttacking;
        private AnimationType _attackAnimationType;
        
        private void RotateTowardsPositionAndAttackUpdate()
        {
            if (!_isRotatingTowardsPosition) return;
            
            Quaternion lookRotation = Quaternion.LookRotation(_flatRotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

            if (LookingAtPosition)
            {
                _isRotatingTowardsPosition = false;
                if (_isAttacking)
                {
                    _isAttacking = false;
                    Animate(_attackAnimationType);
                }
            }
        }
        
        private void AnimatorEvent_AttackFinished()
        {
            if (!IsOwner) return;
            
            // _isRotatingTowardsRotation = true;
            // _rotationDestination = IsHost ? HostUnitsRotation : ClientUnitsRotation;
        }
        #endregion
    }
}