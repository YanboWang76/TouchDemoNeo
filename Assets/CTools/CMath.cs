using UnityEngine;
using System.Collections;

public static class CMath{

	/// <summary>
	/// 限制value在minValue和MaxValue范围内，如果超出范围，则返回对应的边界值
	/// </summary>
	public static int GetClampRange(this int value,int minValue,int maxValue){
		value = Mathf.Max (value, Mathf.Min(minValue,maxValue));
		value = Mathf.Min (value, Mathf.Max(minValue,maxValue));
		return value;
	}
	/// <summary>
	/// 限制value在minValue和MaxValue范围内，如果超出范围，则返回对应的边界值
	/// </summary>
	public static float GetClampRange(this float value,float minValue,float maxValue){
		value = Mathf.Max (value, Mathf.Min(minValue,maxValue));
		value = Mathf.Min (value, Mathf.Max(minValue,maxValue));
		return value;
	}

	public static float GetDistanceXY(Transform fromPos,Transform toPos){
		return GetDistanceXY(fromPos.position,toPos.position);
	}

	public static float GetDistanceXY(Vector3 fromPos,Vector3 toPos){
		return (new Vector2(toPos.x,toPos.y)-new Vector2(fromPos.x,fromPos.y)).magnitude;
	}

	public static float GetDistanceXZ(Transform fromPos,Transform toPos){
		return GetDistanceXZ(fromPos.position,toPos.position);
	}

	public static float GetDistanceXZ(Vector3 fromPos,Vector3 toPos){
		return (new Vector2(toPos.x,toPos.z)-new Vector2(fromPos.x,fromPos.z)).magnitude;
	}

	public static float GetAngle3D(Transform fromPos,Transform toPos){
		return GetAngle3D(fromPos.position,toPos.position);
	}

	public static float GetAngle3D(Vector3 fromPos,Vector3 toPos){
		return GetRaidan3D(fromPos,toPos)*180f/Mathf.PI;
	}

	public static float GetRaidan3D(Transform fromPos,Transform toPos){
		return GetRaidan3D(fromPos.position,toPos.position);
	}

	public static float GetRaidan3D(Vector3 fromPos,Vector3 toPos){
		return Mathf.Atan2(toPos.x-fromPos.x,toPos.z-fromPos.z);
	}

	public static float GetAngle2D(Transform fromPos,Transform toPos){
		return GetAngle2D(fromPos.position,toPos.position);
	}

	public static float GetAngle2D(Vector3 fromPos,Vector3 toPos){
		return GetRaidan2D(fromPos,toPos)*180f/Mathf.PI;
	}

	public static float GetRaidan2D(Transform fromPos,Transform toPos){
		return GetRaidan2D(fromPos.position,toPos.position);
	}

	public static float GetRaidan2D(Vector3 fromPos,Vector3 toPos){
		return Mathf.Atan2(toPos.y-fromPos.y,toPos.x-fromPos.x);
	}

	public static float GetRaidan2D(Transform trans){
		return GetRaidan2D(trans.localEulerAngles);
	}

	public static float GetRaidan2D(Vector3 localEulerAngles){
		return (localEulerAngles.z+90f)*Mathf.PI/180f;
	}

	//求一元一次方程的k,b
	public struct KB{
		public float k,b;
	}
	public static KB GetKB(float x1,float y1,float x2,float y2){
		KB kb;
		kb.k=(y2-y1)/(x2-x1);
		kb.b=y1-kb.k*x1;
		return kb;
	}

    //获取贝塞尔曲线
    public static Vector3 GetBezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        //t范围0到1,对应的贝塞尔起点到终点;
        float x = CalculateQuadSpline(p1.x, p2.x, p3.x, t);
        float y = CalculateQuadSpline(p1.y, p2.y, p3.y, t);
        float z = CalculateQuadSpline(p1.z, p2.z, p3.z, t);
        return new Vector3(x, y, z);
    }
    public static Vector2 GetBezierPoint(Vector2 p1, Vector2 p2, Vector2 p3, float t) {
        //t范围0到1,对应的贝塞尔起点到终点;
        float x = CalculateQuadSpline(p1.x, p2.x, p3.x, t);
        float y = CalculateQuadSpline(p1.y, p2.y, p3.y, t);
        return new Vector2(x, y);
    }
    public static Vector3[] GetBezierPoints(Vector3 p1, Vector3 p2, Vector3 p3, int detailCount) {
        Vector3[] newPointF = new Vector3[detailCount + 1];
        float tStep = 1 / ((float)detailCount);
        float t = 0f;
        for (int ik = 0; ik <= detailCount; ik++) {
            float x = CalculateQuadSpline(p1.x, p2.x, p3.x, t);
            float y = CalculateQuadSpline(p1.y, p2.y, p3.y, t);
            float z = CalculateQuadSpline(p1.z, p2.z, p3.z, t);
            newPointF[ik] = new Vector3(x, y, z);
            t = t + tStep;
        }
        return newPointF;
    }
    public static Vector2[] GetBezierPoints(Vector2 p1, Vector2 p2, Vector2 p3, int detailCount) {
        Vector2[] newPointF = new Vector2[detailCount + 1];
        float tStep = 1 / ((float)detailCount);
        float t = 0f;
        for (int ik = 0; ik <= detailCount; ik++) {
            float x = CalculateQuadSpline(p1.x, p2.x, p3.x, t);
            float y = CalculateQuadSpline(p1.y, p2.y, p3.y, t);
            newPointF[ik] = new Vector2(x, y);
            t = t + tStep;
        }
        return newPointF;
    }
    static float CalculateQuadSpline(float z0, float z1, float z2, float t) {
        float a1 = (1.0f - t) * (1.0f - t) * z0;
        float a2 = 2.0f * t * (1f - t) * z1;
        float a3 = t * t * z2;
        float a4 = a1 + a2 + a3;
        return a4;
    }

}