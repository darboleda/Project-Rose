using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraLine : CameraController
    {
        public Transform LookAtTarget;
        public Transform UpTarget;

        public Vector3 LookOffset;

        public Transform Left;
        public Transform Right;

        public bool LockXRotation = true;
        public bool LockYRotation = false;
        public bool LockZRotation = true;

        public void Update()
        {
            Vector3 a = Left.position;
            Vector3 b = Right.position;
            Vector3 n = (a - b);
            Vector3 p = LookAtTarget.position;

            float k = Vector3.Dot(p - b, n) / Vector3.Dot(n, n);
            CameraContainer.transform.position = Vector3.Lerp(b, a, Mathf.Clamp(k, 0, 1));

            Vector3 startEuler = CameraContainer.eulerAngles;
            CameraContainer.LookAt(this.LookAtTarget.position + this.LookOffset, this.UpTarget.position - this.CameraContainer.position);
            Vector3 euler = CameraContainer.eulerAngles;
            euler.x = (LockXRotation ? startEuler.x : euler.x);
            euler.y = (LockYRotation ? startEuler.y : euler.y);
            euler.z = (LockZRotation ? startEuler.z : euler.z);
            CameraContainer.eulerAngles = euler;
        }
    }
}
