using UnityEngine;
using System.Collections;
using System;

public class ColorGradient  {

    public struct ColorPoint : System.IComparable
    {
        public float position;
        public Vector3 color;

        public ColorPoint(float position, Vector3 color) : this()
        {
            this.position = position;
            this.color = color;
        }

        public int CompareTo(object obj)
        {
            float thatPos = ((ColorPoint)obj).position;
            return thatPos < position ? 1 : (thatPos > position ? -1 : 0);
        }
    }

    private ColorPoint[] colorPoints;

    public ColorGradient(params ColorPoint[] colorPoints)
    {
        this.colorPoints = (ColorPoint[]) colorPoints.Clone();
        System.Array.Sort(this.colorPoints);
    }

    public Vector3 GetValue(float t)
    {
        ColorPoint min = colorPoints[0];
        for (int i = 0; i < colorPoints.Length; i++)
        {
            ColorPoint p = colorPoints[i];
            if (p.position > t)
            {
                float frac = (t - min.position) / (p.position - min.position);
                return min.color * (1 - frac) + p.color * frac;
            }
            min = p;
        }
        return colorPoints[colorPoints.Length - 1].color;
    }

}
