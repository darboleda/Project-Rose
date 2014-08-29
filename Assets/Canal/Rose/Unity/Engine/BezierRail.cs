using UnityEngine;
using System.Collections.Generic;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
	public class BezierRail : Rail {
        public BezierPath PathToBake;

        public int DefaultCurveSegmentCount = 10;
        public float LocalMinimumSegmentLength = 0.03f;
        public List<int> CurveSegmentCounts = new List<int>();

        public List<Vector3> SegmentPoints = null;

        public void BakePath(BezierPath path)
        {
            if(path == null) return;

            while(CurveSegmentCounts.Count < path.CurveCount)
            {
                CurveSegmentCounts.Add(DefaultCurveSegmentCount);
            }

            this.SegmentPoints = new List<Vector3>();
            float minLengthSq = LocalMinimumSegmentLength * LocalMinimumSegmentLength;
            for (int i = 0, length = path.CurveCount; i < length; ++i)
            {
                BezierPath.BezierCurve curve = path.GetCurve(i);
                if(i == 0)
                {
                    SegmentPoints.Add(this.transform.InverseTransformPoint(curve.Sample(0)));
                }

                float deltaT = 1.0f / CurveSegmentCounts[i];
                float currentT = deltaT;
                while(currentT < 1.0f)
                {
                    Vector3 newPoint = this.transform.InverseTransformPoint(curve.Sample(currentT));
                    Vector3 previousPoint = SegmentPoints[SegmentPoints.Count - 1];
                    if (currentT != deltaT && (newPoint - previousPoint).sqrMagnitude < minLengthSq)
                    {
                        SegmentPoints[SegmentPoints.Count - 1] = newPoint;
                    }
                    else
                    {
                        SegmentPoints.Add(newPoint);
                    }

                    currentT += deltaT;
                };

                {
                    Vector3 newPoint = this.transform.InverseTransformPoint(curve.Sample(1.0f));
                    Vector3 previousPoint = SegmentPoints[SegmentPoints.Count - 1];
                    if (currentT != deltaT && (newPoint - previousPoint).sqrMagnitude < minLengthSq)
                    {
                        SegmentPoints[SegmentPoints.Count - 1] = newPoint;
                    }
                    else
                    {
                        SegmentPoints.Add(newPoint);
                    }
                    
                    currentT += deltaT;
                }
            }
        }

        public override float GetLength()
        {
            if (this.SegmentPoints.Count <= 1) return 0;
            float total = 0;
            for (int i = 0, length = this.SegmentPoints.Count - 1; i < length; ++i)
            {
                Vector3 point1 = SegmentPoints[i];
                Vector3 point2 = SegmentPoints[i + 1];
                
                float segLength = (point2 - point1).magnitude;
                total += segLength;
            }
            return total;
        }

        public override float GetWorldLength()
        {
            if (this.SegmentPoints.Count <= 1) return 0;
            float total = 0;
            for (int i = 0, length = this.SegmentPoints.Count - 1; i < length; ++i)
            {
                Vector3 point1 = this.transform.TransformPoint(SegmentPoints[i]);
                Vector3 point2 = this.transform.TransformPoint(SegmentPoints[i + 1]);
                
                float segLength = (point2 - point1).magnitude;
                total += segLength;
            }
            return total;
        }

        public override Vector3 SampleWorld(float worldDistance)
        {
            if (this.SegmentPoints.Count == 0) return Vector3.zero;
            if (this.SegmentPoints.Count == 1) return this.transform.TransformPoint(SegmentPoints[0]);
            if (worldDistance <= 0) return this.transform.TransformPoint(SegmentPoints[0]);

            for (int i = 0, length = this.SegmentPoints.Count - 1; i < length; ++i)
            {
                Vector3 point1 = this.transform.TransformPoint(SegmentPoints[i]);
                Vector3 point2 = this.transform.TransformPoint(SegmentPoints[i + 1]);
                
                float segLength = (point2 - point1).magnitude;
                if(segLength > worldDistance)
                {
                    return Vector3.Lerp(point1, point2, worldDistance / segLength);
                }
                worldDistance -= segLength;
            }
            return this.SegmentPoints[this.SegmentPoints.Count - 1];
        }

        public override Vector3 Sample(float distance)
        {
            if (this.SegmentPoints.Count == 0) return Vector3.zero;
            if (this.SegmentPoints.Count == 1) return this.SegmentPoints[0];
            if (distance <= 0) return this.SegmentPoints[0];
            for (int i = 0, length = this.SegmentPoints.Count - 1; i < length; ++i)
            {
                Vector3 point1 = SegmentPoints[i];
                Vector3 point2 = SegmentPoints[i + 1];

                float segLength = (point2 - point1).magnitude;
                if(segLength > distance)
                {
                    return Vector3.Lerp(point1, point2, distance / segLength);
                }
                distance -= segLength;
            }
            return this.SegmentPoints[this.SegmentPoints.Count - 1];
        }
	}
}
