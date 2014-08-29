using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class MovingCamera : Behavior
    {
        public CameraPosition Target;
        public float FollowSpeed;
        public float RotationSpeed;

        public void Awake()
        {
            if (Target != null)
            {
                this.transform.position = Target.PositionTarget.position;
                this.transform.LookAt(Target.LookAtTarget.position, Target.UpTarget.position - Target.PositionTarget.position);
            }
        }

        public void Update()
        {
            Vector3 targetPos = Target.PositionTarget.position;
            Vector3 pos = this.transform.position;
            float distanceToMove = FollowSpeed * Time.deltaTime;
            Vector3 dir = (targetPos - pos);
            if (distanceToMove * distanceToMove > dir.sqrMagnitude)
            {
                this.transform.position = targetPos;
            }
            else
            {
                Vector3 distanceTraveled = distanceToMove * dir.normalized;
                this.transform.position += distanceTraveled;
            }

            this.transform.LookAt(Target.LookAtTarget.position, Target.UpTarget.position - Target.PositionTarget.position);
        }
    }
}
