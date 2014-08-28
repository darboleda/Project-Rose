using UnityEngine;
using UnityEditor;

using System.Linq;

namespace Canal.Unity.Editor
{

    [CustomEditor(typeof(BezierPath))]
    public class BezierPathEditor : UnityEditor.Editor
    {
        public BezierPoint selectedPoint = null;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BezierPath path = target as BezierPath;
            if (path.Points.Count > 0)
            {
                BezierPoint pointToRemove = null;
                BezierPoint pointToAddAfter = null;
                for (int i = 0, length = path.Points.Count; i < length; ++i)
                {
                    bool shouldRemove, shouldAddAfter;
                    this.DrawPointInspector(i + 1, path.Points[i], out shouldRemove, out shouldAddAfter);
                    if (shouldRemove) pointToRemove = path.Points[i];
                    if (shouldAddAfter) pointToAddAfter = path.Points[i];
                }
                if (pointToRemove != null) path.DeletePoint(pointToRemove);
                if (pointToAddAfter != null) path.AddPointAfter(pointToAddAfter);
            }

            if (GUILayout.Button("Add Point"))
            {
                path.Extend();
            }

            if (GUILayout.Button("Level Path"))
            {
                foreach (BezierPoint point in path.Points)
                {
                    Undo.RecordObject(point, "Level Path");
                    Vector3 position = point.transform.position;
                    position.y = 0;
                    point.transform.position = position;

                    position = point.EntryTangent;
                    position.y = 0;
                    point.EntryTangent = position;

                    position = point.ExitTangent;
                    position.y = 0;
                    point.ExitTangent = position;
                }
            }
        }

        public void OnSceneGUI()
        {
            BezierPath path = target as BezierPath;

            if (path.Points.Count > 0)
            {
                BezierPoint previous = path.Points[0];
                if (previous != selectedPoint)
                {
                    this.DrawBezierPoint(previous);
                    if (this.BezierPointSelector(previous.transform.position))
                    {
                        selectedPoint = previous;
                        Selection.activeGameObject = path.gameObject;
                    }
                }
                foreach (BezierPoint current in path.Points)
                {
                    if (current == previous) continue;
                    if (previous == null) { previous = current; continue; }
                    if (current == null) { continue; }
                    
                    Handles.DrawBezier(
                        previous.transform.position,
                        current.transform.position,
                        previous.ExitTangent,
                        current.EntryTangent,
                        Color.red,
                        null,
                        4f);

                    if (current != selectedPoint)
                    {
                        this.DrawBezierPoint(current);
                        if (this.BezierPointSelector(current.transform.position))
                        {
                            selectedPoint = current;
                            Selection.activeGameObject = path.gameObject;
                        }
                    }
                    previous = current;
                }
            }

            if (selectedPoint != null)
            {
                Tools.current = Tool.None;
                this.DrawBezierPoint(selectedPoint, true);
            }
        }

        private void DrawBezierPoint(BezierPoint point, bool selected = false)
        {
            Color previousColor = Handles.color;
            Handles.color = (selected ? Color.white : Color.yellow);

            Vector3 pointPos;
            Vector3 position = pointPos = point.transform.position;
            float size = HandleUtility.GetHandleSize(position);
            Vector3 direction = SceneView.currentDrawingSceneView.camera.transform.rotation * Vector3.back;
            Vector3 newPosition;
            Handles.DrawSolidDisc(position, direction, size * 0.05f);
            if (selected)
            {
                newPosition = PositionHandle(position);
                if (position != newPosition)
                {
                    Undo.RecordObject(point, "Move point");
                    if (point.lockY) newPosition.y = position.y;
                    point.transform.position = newPosition;
                    
                }
            }

            if (!selected) return;

            position = point.ExitTangent;
            Handles.DrawBezier(pointPos, position, pointPos, position, Color.white, null, 2f);

            size = HandleUtility.GetHandleSize(position);
            newPosition = PositionHandle(position);
            if (position != newPosition)
            {
                Undo.RecordObject(point, "Move Tangent");
                point.ExitTangent = newPosition;

            }

            position = point.EntryTangent;
            Handles.DrawBezier(pointPos, position, pointPos, position, Color.white, null, 2f);

            size = HandleUtility.GetHandleSize(position);
            newPosition = PositionHandle(position);
            if (position != newPosition)
            {
                Undo.RecordObject(point, "Move Tangent");
                point.EntryTangent = newPosition;

            }

            Handles.color = previousColor;
        }

        private void DrawPointInspector(int index, BezierPoint point, out bool shouldRemove, out bool shouldAddAfter)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("Point {0}", index));

            EditorGUILayout.LabelField("Lock Y", GUILayout.Width(40f));
            point.lockY = EditorGUILayout.Toggle(point.lockY, GUILayout.Width(20f));
            EditorGUILayout.LabelField("Break Tangent", GUILayout.Width(90f));
            point.brokenTangent = EditorGUILayout.Toggle(point.brokenTangent, GUILayout.Width(20f));
            shouldRemove = GUILayout.Button("Delete Point");
            shouldAddAfter = GUILayout.Button("Split After");

            GUILayout.EndHorizontal();
        }

        private Vector3 PositionHandle(Vector3 position)
        {
            Quaternion cameraRotation = SceneView.currentDrawingSceneView.camera.transform.rotation;
            float size = HandleUtility.GetHandleSize(position);
            Vector3 direction = cameraRotation * Vector3.back;
            return Handles.Slider2D(position, direction, cameraRotation * Vector3.up, cameraRotation * Vector3.right, size * 0.05f, Handles.RectangleCap, size * 0.001f);
        }

        private bool BezierPointSelector(Vector3 position)
        {
            float size = HandleUtility.GetHandleSize(position);
            Quaternion cameraRotation = SceneView.currentDrawingSceneView.camera.transform.rotation;
            return Handles.Button(position, cameraRotation, 0.1f * size, 0.1f * size, Handles.CircleCap);
        }
    }
}
