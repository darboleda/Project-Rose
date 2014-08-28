using UnityEngine;
using System.Collections;

namespace Canal.Unity
{
    public class BezierPoint : Behavior
    {
        public bool brokenTangent;
        public bool lockY;

        [SerializeField] private Vector3 entryTangent;
        [SerializeField] private Vector3 exitTangent;

        public Vector3 EntryTangent
        {
            get { return transform.TransformPoint(entryTangent); }
            set
            {
                if (lockY) value.y = transform.position.y;
                entryTangent = transform.InverseTransformPoint(value);
                if (!brokenTangent)
                {
                    exitTangent = -exitTangent.magnitude * entryTangent.normalized;
                }
            }
        }

        public Vector3 ExitTangent
        {
            get { return transform.TransformPoint(exitTangent); }
            set
            {
                if (lockY) value.y = transform.position.y;
                exitTangent = transform.InverseTransformPoint(value);
                if (!brokenTangent)
                {
                    entryTangent = -entryTangent.magnitude * exitTangent.normalized;
                }
            }
        }
    }
}
