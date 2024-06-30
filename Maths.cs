using Raylib_cs;
using System.Numerics;

public static class Maths {
    /*
    Extends a & b into 3D, with z=0.
    Their cross-product is therefore some multiple of the unit z-vector. Return this scalar factor.
        Properties:
            - sign flips when the inputs are flipped
            - Cross2D(a,b) = |a||b|sinθ
            - v x v = 0
            - distributive over vector addition/subtraction
            - nv x w = n(v x w)                                                                     */
    private static float Cross2D(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;


    public static Vector2? IntersectLineSegments(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2) {
        Vector2 d1 = b1 - a1;
        Vector2 d2 = b2 - a2;
        //Find u & v in [0,1] s.t. a1+u(d1) = a2+v(d2)
        //Use cross product property v x v = 0 to eliminate d1 & d2:
        float crossd1d2 = Cross2D(d1, d2);
        if (crossd1d2 == 0) return null;
        float v = Cross2D(a1 - a2, d1) / (-1 * crossd1d2);
        if (v < 0 || v > 1) return null;
        float u = Cross2D(a2 - a1, d2) / crossd1d2;
        if (u < 0 || u > 1) return null;
        return a1 + u * d1;
    }

    public static Vector2? IntersectLineWithRect(Vector2 a, Vector2 b, Rectangle r) {
        Vector2 v00 = r.Position;
        Vector2 v10 = r.Position + Vector2.UnitX * r.Width;
        Vector2 v01 = r.Position + Vector2.UnitY * r.Height;
        Vector2 v11 = r.Position + r.Size;

        Vector2? x;
        x = IntersectLineSegments(a, b, v00, v10); if (x != null) return x;
        x = IntersectLineSegments(a, b, v10, v11); if (x != null) return x;
        x = IntersectLineSegments(a, b, v11, v01); if (x != null) return x;
        x = IntersectLineSegments(a, b, v01, v00); if (x != null) return x;

        return null;
    }
}