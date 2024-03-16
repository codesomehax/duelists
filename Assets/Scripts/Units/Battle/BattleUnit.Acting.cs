using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
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

        private BattleUnit _enemyUnit;
        
        public void AttackUnitAtPosition(BattleUnit enemyUnit, Vector3 unitPosition, AnimationType attackAnimationType)
        {
            _enemyUnit = enemyUnit;
            StartCoroutine(AttackCoroutine(unitPosition, attackAnimationType));
        }

        private IEnumerator AttackCoroutine(Vector3 unitPosition, AnimationType attackAnimationType)
        {
            Vector3 flatUnitDirection = unitPosition - movementTransform.position;
            flatUnitDirection.y = 0f;
            Quaternion unitRotation = Quaternion.LookRotation(flatUnitDirection);
            yield return StartCoroutine(SetRotationCoroutine(unitRotation));
            
            Animate(attackAnimationType);
        }
        
        private void AnimationEvent_AttackFinished()
        {
            if (!IsHost) return;
            StartCoroutine(SetRotationCoroutine(Owner.IsHost ? HostUnitsRotation : ClientUnitsRotation));
        }

        private void AnimationEvent_HitMelee()
        {
            if (IsOwner)
                DealDamageServerRpc();
        }

        [ServerRpc]
        private void DealDamageServerRpc()
        {
            int distance = GridManager.DistanceBetweenPositions(CellPosition, _enemyUnit.CellPosition);
            AttackType attackType = distance == 1 ? MeleeAttackType : RangedAttackType;
            int damage = attackType == AttackType.Physical ? PhysicalDamage : MagicDamage;
            _enemyUnit.TakeDamage(damage, attackType);
        }
        
        private void TakeDamage(int damage, AttackType attackType)
        {
            int reducedDamage;
            if (attackType == AttackType.Physical)
                reducedDamage = Mathf.CeilToInt(damage * (float)(100 - PhysicalDefense) / 100);
            else
                reducedDamage = Mathf.CeilToInt(damage * (float)(100 - MagicDefense) / 100);
            
            Health -= reducedDamage;
            int remainingCount = Mathf.CeilToInt((float)Health / SingleUnitHealth);
            Count = remainingCount;

            if (remainingCount > 0)
                Animate(AnimationType.Hurt);
            else
                Die();
        }

        private void Die()
        {
            Animate(AnimationType.Death);
        }
    }
}