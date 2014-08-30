using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraPosition : CameraController
    {
        public Transform LookAtTarget;
        public Transform PositionTarget;
        public Transform UpTarget;

        public float FollowSpeed;
        public float RotationSpeed;

        public float InterpolationRatio = 0.3f;

        public bool LockZRotation = true;
        public bool LockXRotation = true;

        public void Start()
        {
            if (Camera != null)
            {
                Camera.transform.position = this.PositionTarget.position;
                Camera.transform.LookAt(this.LookAtTarget.position, this.UpTarget.position - this.PositionTarget.position);
            }
        }

        public void Update()
        {
            if (Camera == null) return;

            Vector3 targetPos = this.PositionTarget.position;
            Vector3 pos = Camera.transform.position;
            Vector3 dir = (targetPos - pos);
            float distanceToMove = FollowSpeed * Time.deltaTime;
            if (dir.sqrMagnitude < (distanceToMove * distanceToMove) )
            {
                Camera.transform.position = targetPos;
            }
            else
            {
                Vector3 newPos = Vector3.Lerp(pos, targetPos, InterpolationRatio);
                if ((newPos - pos).sqrMagnitude < distanceToMove * distanceToMove)
                {
                    newPos = Time.deltaTime * FollowSpeed * dir.normalized + Camera.transform.position;
                }
                Camera.transform.position = newPos;
            }

            Camera.transform.LookAt(this.LookAtTarget.position, this.UpTarget.position - this.PositionTarget.position);
            Vector3 euler = Camera.transform.localEulerAngles;
            euler.z = (LockZRotation ? 0 : euler.z);
            euler.x = (LockXRotation ? 0 : euler.x);
            Camera.transform.localEulerAngles = euler;
        }
    }
}