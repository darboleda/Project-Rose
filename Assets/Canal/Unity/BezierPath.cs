using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Canal.Unity
{
    [ExecuteInEditMode]
    public class BezierPath : Behavior
    {
		public struct BezierCurve
		{
			public BezierPoint EntryPoint;
			public BezierPoint ExitPoint;

			public BezierCurve(BezierPoint entry, BezierPoint exit)
			{
				this.EntryPoint = entry;
				this.ExitPoint = exit;
			}

			public Vector3 Sample(float t)
			{
				float tIn = 1 - t;
				Vector3 p0, p1, p2, p3;
				p0 = this.EntryPoint.Position;
				p1 = this.EntryPoint.ExitTangent;
				p2 = this.ExitPoint.EntryTangent;
				p3 = this.ExitPoint.Position;

                if (t <= 0) return p0;
                if (t >= 1) return p3;

				float tInSq = tIn * tIn;
				float tSq = t * t;

				return ((tInSq * tIn) * p0)
					    + ((3 * tInSq * t) * p1)
					    + ((3 * tIn * tSq) * p2)
						+ ((tSq * t) * p3);
			}
		}

        public interface IBezierPointCollection
        {
            int Count { get; }
            BezierPoint this[int index] { get; }
            
            IEnumerator GetEnumerator();
        }
        
        private class BezierPointCollection : IBezierPointCollection
        {
            private BezierPath owner;
            public int Count { get { return owner.points.Count; } }
            public BezierPoint this[int index]
            {
                get
                {
                    return owner.points[index];
                }
            }

            public BezierPointCollection(BezierPath owner)
            {
                this.owner = owner;
            }

            private IEnumerable<BezierPoint> GetPoints()
            {
                foreach (BezierPoint point in (IBezierPointCollection)this)
                {
                    yield return point;
                }
            }

            IEnumerator IBezierPointCollection.GetEnumerator()
            {
                return owner.points.GetEnumerator();
            }
        }

        
        //[HideInInspector]
        [SerializeField]
        private List<BezierPoint> points = new List<BezierPoint>();

        private IBezierPointCollection pointCollection;
        public IBezierPointCollection Points { get { Initialize(); return pointCollection; } }

        public void Initialize()
        {
            pointCollection = pointCollection ?? new BezierPointCollection(this);
        }

		public int CurveCount
		{
			get { return (this.points.Count > 0 ? this.points.Count - 1 : 0); }
		}

		public BezierCurve GetCurve(int curveIndex)
		{
			if(curveIndex >= 0 && curveIndex < CurveCount)
			{
				return new BezierCurve(this.points[curveIndex], this.points[curveIndex + 1]);
			}
			return default(BezierCurve);
		}

        public void Extend()
        {
            if (Points.Count == 0)
            {
                BezierPoint first = CreatePoint();
                first.transform.position = this.transform.position;
                this.points.Add(first);
            }
                
            BezierPoint newPoint = CreatePoint();
            newPoint.transform.position = Points[Points.Count - 1].transform.position + Vector3.right;
            this.points.Add(newPoint);
        }

        public void AddPointAfter(BezierPoint point)
        {
            int index = points.IndexOf(point);
            if (index >= points.Count - 1)
            {
                this.Extend();
                return;
            }

            if (index < 0) return;
            this.SplitCurve(index);
        }

        public void DeletePoint(BezierPoint point)
        {
            if (this.points.Remove(point))
            {
                GameObject.DestroyImmediate(point);
            }
        }

        private BezierPoint CreatePoint()
        {
            GameObject pointObject = new GameObject();
            BezierPoint point = pointObject.AddComponent<BezierPoint>();
            pointObject.transform.parent = transform;
            pointObject.transform.localScale = Vector3.one;
            pointObject.transform.localRotation = Quaternion.identity;
            return point;
        }

        private void SplitCurve(int index)
        {
            BezierPoint point = points[index];
            BezierPoint point2 = points[index + 1];
            BezierPoint newPoint = CreatePoint();

            Vector3 p1 = Vector3.Lerp(point.transform.position, point.ExitTangent, 0.5f);
            Vector3 p2 = Vector3.Lerp(point2.transform.position, point2.EntryTangent, 0.5f);
            Vector3 p3 = Vector3.Lerp(point.ExitTangent, point2.EntryTangent, 0.5f);
            Vector3 p4 = Vector3.Lerp(p1, p3, 0.5f);
            Vector3 p5 = Vector3.Lerp(p2, p3, 0.5f);
            Vector3 p6 = Vector3.Lerp(p4, p5, 0.5f);

            newPoint.transform.position = p6;
            newPoint.EntryTangent = p4;
            newPoint.ExitTangent = p5;

            point.ExitTangent = p1;
            point2.EntryTangent = p2;

            points.Insert(index + 1, newPoint);
        }
    }
}
