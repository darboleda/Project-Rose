﻿using UnityEngine;

using System.Collections.Generic;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class PlayerController : Behavior {
        public Tracer Tracer;
        public float WalkSpeed = 2f;

        public float Radius = 1f;

        public Animator Animator;
        public Transform FacingTarget;

        public bool activateTriggers = true;

        private HashSet<Trigger> currentTriggers = new HashSet<Trigger>();

        public void FixedUpdate()
        {
            float move = Input.GetAxis("Horizontal");
            
            transform.position = Tracer.Move(move * WalkSpeed * Time.deltaTime);
            Animator.SetFloat("hSpeed", move * WalkSpeed);
            Animator.SetBool("Walking", Mathf.Abs(move) > 0.1f);

            if (move != 0)
            {
                Vector3 facing = Tracer.GetFacingDirection(move);
                facing.y = 0;
                facing.Normalize();
                FacingTarget.rotation = Quaternion.FromToRotation(Vector3.right, facing);
            }

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
