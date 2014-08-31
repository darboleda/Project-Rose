using UnityEngine;

using System.Collections.Generic;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class MovementPhysics : Behavior {
        public Tracer Tracer;

        [System.NonSerialized]
        public float WalkDirection = 0f;

        public float WalkAcceleration = 2f;
        public float WalkSpeed = 2f;
        public float MomentumRatio = 0.5f;

        public float Radius = 1f;

        public Animator Animator;
        public Transform FacingTarget;

        public bool activateTriggers = true;

        private float currentAcceleration = 0;
        private float currentSpeed = 0;

        private HashSet<Trigger> currentTriggers = new HashSet<Trigger>();

        public void FixedUpdate()
        {
            ApplyAcceleration();
            ApplyVelocity();
            CheckTriggers();
        }

        public void ApplyAcceleration()
        {
            if (WalkDirection != 0)
            {
                currentSpeed = Mathf.Clamp(currentSpeed + (WalkDirection * WalkAcceleration * Time.deltaTime), -WalkSpeed, WalkSpeed);
            }
            else
            {
                currentSpeed *= MomentumRatio;
            }
        }

        public void ApplyVelocity()
        {
            transform.position = Tracer.Move(currentSpeed * Time.deltaTime);
            Animator.SetFloat("hSpeed", currentSpeed);
            Animator.SetBool("Walking", Mathf.Abs(currentSpeed) > 0.5f);

            if (currentSpeed != 0)
            {
                Vector3 facing = Tracer.GetFacingDirection(Mathf.Sign(currentSpeed));
                facing.y = 0;
                facing.Normalize();
                FacingTarget.rotation = Quaternion.FromToRotation(Vector3.right, facing);
            }
        }

        public void CheckTriggers()
        {
            if (activateTriggers)
            {
                List<Trigger> newTriggers = new List<Trigger>(Tracer.GetTriggers(this.Radius));
                foreach (Trigger trigger in newTriggers)
                {
                    if (currentTriggers.Contains(trigger)) trigger.NotifyTriggerStay();
                    else trigger.NotifyTriggerEnter();
                }
                foreach (Trigger trigger in newTriggers)
                {
                    currentTriggers.Remove(trigger);
                }
                foreach (Trigger leftover in currentTriggers)
                {
                    leftover.NotifyTriggerExit();
                }
                currentTriggers.Clear();
                foreach (Trigger trigger in newTriggers) currentTriggers.Add(trigger);
            }
        }
    }
}
