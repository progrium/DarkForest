using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace SectorGrid {
	public static class Util {
		public static Color HexRGBColor(string hex) {
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}

		public static Color HexRGBAColor(string hex) {
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}

		public static GameObject EnsureChild(Transform transform, string name) {
			var child = transform.Find (name);
			if (child != null) {
				child.gameObject.SetActive (true);
				return child.gameObject;
			} else {
				GameObject obj = new GameObject (name);
				obj.transform.position = transform.position;
				obj.transform.parent = transform;
				return obj;
			} 
		}

		public static void CopyComponent<T>(T original, T destination) where T : Component
		{
			System.Type type = original.GetType();
			System.Reflection.FieldInfo[] fields = type.GetFields();
			foreach (System.Reflection.FieldInfo field in fields)
			{
				field.SetValue(destination, field.GetValue(original));
			}
		}

		public static void DisableChild(Transform transform, string name) {
			var child = transform.Find (name);
			if (child == null) {
				return;
			}
			child.gameObject.SetActive (false);
		}

		public static GameObject AddChild(Transform transform, string prefix) {
			return EnsureChild (transform, GenerateName (transform, prefix));
		}

		public static string GenerateName(Transform transform, string prefix) {
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
			string uniqueID;
			do {
				uniqueID = prefix;
				for(int i = 0; i < 3; i++) {
					uniqueID += chars[Random.Range(0, chars.Length)];	
				}
			} while(transform.Find(uniqueID) != null);
			return uniqueID;
		}

		public static void DestroyChild(Transform transform, string name) {
			var t = transform.Find(name);
			if (t != null) {
				GameObject.DestroyImmediate(t.gameObject);
			}
		}

		public static void DrawRectLines(Vector3[] vertices, float width) {
			DrawLine (vertices [0], vertices [1], width);
			DrawLine (vertices [1], vertices [2], width);
			DrawLine (vertices [2], vertices [3], width);
			DrawLine (vertices [3], vertices [0], width);
		}

		public static void DrawLine(Vector3 p1, Vector3 p2, float width)
		{
			int count = Mathf.CeilToInt(width); // how many lines are needed.
			Camera c = Camera.current;
			if (count == 1 || c == null) {
				Gizmos.DrawLine (p1, p2);
			} else {
				Vector3 v1 = (p2 - p1).normalized; // line direction
				Vector3 v2 = (c.transform.position - p1).normalized; // direction to camera
				Vector3 n = Vector3.Cross(v1,v2); // normal vector
				for(int i = 0; i < count; i++) {
					Vector3 o = n * (0.08f*(float)i/(count-1));
					Gizmos.DrawLine(p1+o,p2+o);
				}
			}
		}

		public static Vector3[] RectXZ(Vector3 a, Vector3 b) {
			return new Vector3[] {a, a.WithX(b.x), b, b.WithX(a.x)};
		}

		public static float DistanceFromSegment(Vector3[] segment, Vector3 p) {
			// http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
			return Mathf.Sqrt (segmentDistSquared (p, segment[0], segment[1]));
		}

		public static List<Vector3> ToVector3List(List<Vector4> v) {
			var vv = new List<Vector3> ();
			foreach (var e in v) {
				vv.Add (new Vector3 (e.x, e.y, e.z));
			}
			return vv;
		}

		private static float distSquared(Vector3 v, Vector3 w) {
			return Mathf.Pow (v.x - w.x, 2) + Mathf.Pow (v.z - w.z, 2);
		}

		private static float segmentDistSquared(Vector3 p, Vector3 v, Vector3 w) {
			float d1 = distSquared (v, w);
			if (d1 == 0f)
				return distSquared (p, v);
			float d2 = ((p.x - v.x) * (w.x - v.x) + (p.z - v.z) * (w.z - v.z)) / d1;
			if (d2 < 0)
				return distSquared (p, v);
			if (d2 > 1)
				return distSquared (p, w);
			return distSquared (p, new Vector3 (v.x + d2 * (w.x - v.x), 0, v.z + d2 * (w.z - v.z)));
		}
	}

	public static class Extensions {

		// GameObject

		public static T EnsureComponent<T>(this GameObject obj) where T : Component {
			var com = obj.GetComponent<T> ();
			if (com == null) {
				com = obj.AddComponent<T> ();
			}
			return com;
		}

		// Vector3

	public static Vector3 MilliRound(this Vector3 v) {
		return new Vector3 (Mathf.Round(v.x*1000)/1000, Mathf.Round(v.x*1000)/1000, Mathf.Round(v.z*1000)/1000);
	}

		public static Vector3 X(this Vector3 v) {
			return new Vector3 (v.x, 0, 0);
		}

		public static Vector3 Y(this Vector3 v) {
			return new Vector3 (0, v.y, 0);
		}

		public static Vector3 Z(this Vector3 v) {
			return new Vector3 (0, 0, v.z);
		}

		public static Vector3 XZ(this Vector3 v) {
			return new Vector3 (v.x, 0, v.z);
		}

		public static Vector3 WithX(this Vector3 v, float x) {
			var vv = v;
			vv.x = x;
			return vv;
		}

		public static Vector3 WithZ(this Vector3 v, float z) {
			var vv = v;
			vv.z = z;
			return vv;
		}

		public static Vector3 WithY(this Vector3 v, float y) {
			var vv = v;
			vv.y = y;
			return vv;
		}

		public static Vector3 WithXZ(this Vector3 v, Vector3 xz) {
			var vv = v;
			vv.x = xz.x;
			vv.z = xz.z;
			return vv;
		}

		// Vector4

		public static Vector4 X(this Vector4 v) {
			return new Vector4 (v.x, 0, 0, 0);
		}

		public static Vector4 Y(this Vector4 v) {
			return new Vector4 (0, v.y, 0, 0);
		}

		public static Vector4 Z(this Vector4 v) {
			return new Vector4 (0, 0, v.z, 0);
		}

		public static Vector4 W(this Vector4 v) {
			return new Vector4 (0, 0, v.w, 0);
		}

		public static Vector4 XZ(this Vector4 v) {
			return new Vector4 (v.x, 0, v.z, 0);
		}

		public static Vector4 WithX(this Vector4 v, float x) {
			var vv = v;
			vv.x = x;
			return vv;
		}

		public static Vector4 WithZ(this Vector4 v, float z) {
			var vv = v;
			vv.z = z;
			return vv;
		}

		public static Vector4 WithY(this Vector4 v, float y) {
			var vv = v;
			vv.y = y;
			return vv;
		}

		public static Vector4 WithW(this Vector4 v, float w) {
			var vv = v;
			vv.w = w;
			return vv;
		}

	}

//}
	