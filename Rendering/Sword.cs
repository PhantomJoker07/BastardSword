using GMath;
using Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using static GMath.Gfx;

namespace Meshes
{
    public static class Sword<T> where T : struct, IVertex<T>, INormalVertex<T>, ICoordinatesVertex<T>
    {

        #region Basic Mesh Figures

        public static Mesh<T> Cylinder(float radius = 1, float zmin = 0, float zmax = 1, int slices = 50, int stacks = 50, bool invert = false)
        {
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u, v) =>
            {
                float alpha = u * 2 * pi;
                float x = radius * cos(alpha);
                float y = v * (zmax - zmin);
                float z = radius * sin(alpha);
                return float3(x, y, z);
            });
            M.ComputeNormals(invert);
            return M;
        }


        public static Mesh<T> Semicylinder(float radius = 1, float zmin = 0, float zmax = 1, float change_rate = 0.1f, int slices = 50, int stacks = 50, bool invert = false)
        {
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u, v) =>
            {
                float alpha = u * 2 * pi;
                float x = radius * cos(alpha);
                float y = v * (zmax - zmin);
                float z = max(radius * sin(alpha) - change_rate * v, 0);
                return float3(x, y, z);
            });
            M.ComputeNormals(!invert);
            return M;
        }

        //DEPRECATED
        public static Mesh<T> OLDVarCylinder(float radius = 2f, float zmin = 0, float zmax = 2, float change_rate = 0.0003f, int slices = 50, int stacks = 50, bool invert = false)
        {
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u, v) =>
            {
                radius = max(radius - v* change_rate * radius,0);
                float alpha = u * 2 * pi;
                float x = radius * cos(alpha);
                float y = v * (zmax - zmin);
                float z = radius * sin(alpha);
                return float3(x, y, z);
            });
            Mesh<T> M2 = M.Clone();
            M.ComputeNormals(invert);
            M2.ComputeNormals(!invert);
            return (M + M2).Weld();
        }


        public static Mesh<T> VarCylinder(float radius = 2f, float zmin = 0, float zmax = 2, float change_rate = 0.3f, int slices = 50, int stacks = 50, bool invert = false)
        {
             Mesh<T> M = Manifold<T>.Revolution(slices, stacks, t =>
             {
                float x = 0;
                float z =  t*(zmax - zmin);
                float y = change_rate * z + radius;
                if (invert) z = -1*z;
                return float3(x, y, z);
             }, float3(0, 0, 1));

            return M;
        }


        public static Mesh<T> Cone(float radius = 1, float zmin = 0, float zmax = 1, int slices = 50, int stacks = 50, float top = int.MaxValue, bool invert = false)
        {
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u, v) =>
            {
                if (invert) {zmax *= -1; zmin *= -1;} 
                float alpha = u * 2 * pi;
                float x = -1  * v * radius * cos(alpha);
                float y = -1 * v * (zmax - zmin);
                float z = -1  * v * radius * sin(alpha);
                return float3(x, y, z);
            });
            M.ComputeNormals(true); //Invert normals
            return M;
        }


        public static Mesh<T> Circle(float radius = 2.5f, int slices = 50, int stacks = 50, bool invert = false)
        {
             Mesh<T> M = Manifold<T>.Revolution(slices, stacks, t =>
             {
                float x = radius * t;
                float z = radius * t;
                return float3(x, 0, z);
             }, float3(0, 1, 0));
            M.ComputeNormals(invert);
            return M;
        }


        public static Mesh<T> Rectangle(float w = 1, float h = 2, int slices = 50, int stacks = 50, bool invert = false)
        {
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float x = u * w;
                float z = v * h;
                return float3(x, 0, z);
            });
            M.ComputeNormals(invert);
            return M;
        }


        public static Mesh<T> OneSideCurvedRectangle(float w = 0.66f, float h = 4, float zMin = 0, float zMax = 0.5f, float change_rate = 0.55f, int slices = 50, int stacks = 50, bool invert = false)
        {
            float limit = 0;
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float z = u * w;
                float x = v * h;
                float y = 0;

                if(z > zMin) z += v * v * change_rate;
                if(z > zMax & limit != 0) z += limit; 
                if(z >= zMax & limit == 0) limit = v * v * change_rate;
               
                
                return float3(x, y, z);
            });
            M.ComputeNormals(!invert);
            return M;
        }


        public static Mesh<T> CurvedParallelogram(float w = 0.66f, float h = 4, float xMin = 0, float xMax = 0.5f, float change_rate = 0.55f, int slices = 50, int stacks = 50, bool invert = false)
        {
            float limit = 0;
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float z = u * w;
                float x = v * h;
                float y = 0;

                if(x > xMin) z += v * v * change_rate;
                if(x > xMax & limit != 0) z += limit; 
                if(x >= xMax & limit == 0) limit = v * v * change_rate;
               
                
                return float3(x, y, z);
            });
            M.ComputeNormals(!invert);
            return M;
        }


        public static Mesh<T> Rect_Triangle(float b = 1, float h = 1, float angle = pi_over_4, int slices = 50, int stacks = 50, bool invert = false)
        {
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float x = u * b;
                float z = v * h;
                if (z > x * tan (angle)) {z = x * tan (angle);}
                return float3(x, 0, z);
            });
            M.ComputeNormals(invert);
            return M.Weld();
        }


        public static Mesh<T> TriangleRectangle(float w = 0.66f, float h = 4, float xMin = 0, float xMax = 0.5f, float change_rate = 0.55f, int slices = 50, int stacks = 50, bool invert = false)
        {
            float limit = 0;
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float x = u * w;
                float z = v * h;
                float y = 0;

                if(x > xMin) x += v * change_rate;
                if(x > xMax & limit != 0) x += limit; 
                if(x >= xMax & limit == 0) limit = v * change_rate;
               
                
                return float3(x, y, z);
            });
            M.ComputeNormals(invert);
            return M;

        }


        public static Mesh<T> Triangle(float b = 3, float h = 3, float n = 1, float angle = (pi/6), int slices = 50, int stacks = 50, bool invert = false)
        {
            float angle2 = (pi/2) - angle;

            Mesh<T> M1 = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float x = u * b;
                float y = v * h;
                if (y > -1*x * tan (angle) + n) {y = -1 * x * tan (angle) + n;}
                if (y < 0) y = 0;
                return float3(x, y, 0);
            });

            M1.ComputeNormals(invert);

            Mesh<T> M2 = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                float x = u * b;
                float y = v * h;
                if (y > -1*x * tan (angle2) + n) {y = -1 * x * tan (angle2) + n;}
                if (y < 0) y = 0;
                return float3(x, y, 0);
            });

            M2.ComputeNormals(invert);
            float4x4 t = Transforms.RotateYGrad(180);
            M2 = M2.Transform(t);
            return (M1 + M2).Weld();
        }

        //Using Bezier
        public static Mesh<T> pseudoEllipse (float radius = 2, float a = 1.2f, float b = 1, float _min = pi, float _max = pi, int pieces = 20, int slices = 100, int stacks = 100, bool invert = false)
        {
            List<float3> points_list = new List <float3>();
            
            float lim = sqrt(radius/b);
            if  (_min == pi) _min = -lim;
            if  (_max == pi) _max = lim;

            float frag = (_max - _min)/pieces;

            for (int i = 0; i <= pieces; i++)
            {
                float z = _min + i * frag;
                float y = sqrt(max(radius - b*z*z,0)/a);
                points_list.Add(float3(0,y,z));
            }

            float3[] contourn = points_list.ToArray();
            
            Mesh<T> ans = Manifold<T>.Revolution(20, 30, t =>  EvalBezier(contourn, t), float3(0, 0, 1)).Weld();
            //ans.ComputeNormals(invert);
            return ans;
        }

         #endregion


         #region Generatrices & Generated meshes

        public static float3 gen_Circle (float v, float  radius = 1)
        {
            return float3(cos(v) * radius, sin(v) * radius, 0);
        }
        
        public static float3 s_Circle (float3 cir, float u, float xMin = pi, float xMax = pi, float yMin = pi, float yMax = pi)
        {
            float x = cir.x;
            float y = cir.y;
            float z = u;

            if(xMin != pi) { if (x<xMin) {x = 0; y = 0;} }
            if(xMax != pi) { if (x>xMax) {x = 0; y = 0;} }

            if(yMin != pi) { if (y<yMin) {x = 0; y = 0;} }
            if(yMax != pi) { if (y>yMax) {x = 0; y = 0;} }

            
            return float3(x, y, z);
        }

        public static Mesh<T> G_Cilinder (float rad = 1.0f, float h = 5, float change_rate = 0, float xMin = pi, float xMax = pi, float yMin = pi, float yMax = pi ,int slices = 100, int stacks = 100, bool invert = false)
        {
            float radius = rad;
            Mesh<T> M = Manifold<T>.Surface(slices, stacks, (u,v) =>
            {
                radius = rad * (radius + change_rate * u)/radius;    
                return s_Circle(gen_Circle(v * 2 * pi, radius), u * h, xMin, xMax, yMin, yMax);
            });
            M.ComputeNormals(invert);
            Mesh<T> M2 = M.Clone();
            M2.ComputeNormals(!invert);
            return (M + M2).Weld();
        }

         #endregion


         #region Components

        public static Mesh<T> Grip(float radius = 0.35f, float zmin = 0f, float zmax = 8f, int slices = 25, int stacks = 25, bool invert = false)
        {
            float ratioA = 0.1f;           //DEFAULT 0.1f;  
            float ratioB = 0.095f;         //DEFAULT 0.095f; 
            float proportion = 5.5f/10f;  //DEFAULT 5.5
            float limit = (zmax - zmin) * proportion;


            Mesh<T> HandlerA = VarCylinder(radius,zmin,limit, ratioA,slices,stacks,!invert);
            HandlerA.ComputeNormals(!invert);

            float new_radius = ratioA * limit + radius - (zmax - limit) * ratioB; 

            Mesh<T> HandlerB = VarCylinder(new_radius,limit,zmax,ratioB,slices,stacks,invert);
            HandlerB.ComputeNormals(invert);

            float4x4 tA = Transforms.Translate(0,0,zmax);
            HandlerA = HandlerA.Transform(tA);

            Mesh <T> H = (HandlerA + HandlerB);
            float4x4 tH = Transforms.Translate(0,radius,0);
            H = H.Transform(tH);

            return H.Weld();
        }


        public static Mesh<T> Pomel(float radius = 1.5f, int slices = 25, int stacks = 25, bool invert = false)
        {
            float deco_ratio = 0.25f; 
            Mesh<T> pomel_base = pseudoEllipse(radius, 1.4f, 1, pi, pi,100, slices, stacks, invert);
            Mesh<T> pomel_deco = pseudoEllipse(deco_ratio, 1, 1, pi, 0, 10, slices, stacks, invert);
            Mesh<T> pomel_joint = VarCylinder(0.48f, 0, 1.25f, 0.38f, 50, 50, !invert);

            pomel_base.ComputeNormals(invert);
            pomel_deco.ComputeNormals(invert);
            pomel_joint.ComputeNormals(!invert);

            float4x4 tbase = Transforms.Translate(0,0,-radius);
            float4x4 tdeco = Transforms.Translate(0,0,-2*radius + deco_ratio * 4f);
            float4x4 tpomel = Transforms.Translate(0,0.4f,0);
            
            pomel_base = pomel_base.Transform(tbase);
            pomel_deco = pomel_deco.Transform(tdeco);

            Mesh<T> pomelA = (pomel_deco + pomel_base + pomel_joint).Weld();
            Mesh<T> pomel = pomelA.Transform(tpomel);
            return pomel;
        }
            

        public static Mesh<T> Protector(float height = 0.75f, float w = 2.75f, float l = 2.25f, float volume = 0.1f, int pieces = 2, int slices = 25, int stacks = 25, bool invert = false)
        {
            Mesh<T> Face_up = Protector_Face (height, w, l , slices, stacks, invert);
            Mesh<T> Face_down = Protector_Face (height, w, l , slices, stacks, !invert);

            float4x4 to_upO = Transforms.Translate(0, (float) volume / PowOfTwo(pieces), 0);
            float4x4 to_downO = Transforms.Translate(0, (float) volume / PowOfTwo(pieces), 0);

            Mesh<T> NFUO = Face_up.Clone();
            Face_up = (Face_up + NFUO.Transform(to_downO));

            Mesh<T> NFDO = Face_down.Clone();  
            Face_down = (Face_down + NFDO.Transform(to_upO));
            for (int i = 0; i < pieces; i++)
            {
                float4x4 to_upE = Transforms.Translate(0, (float) volume / PowOfTwo(pieces - i), 0);
                float4x4 to_downE = Transforms.Translate(0, (float) volume / PowOfTwo(pieces - i), 0);

                Mesh<T> NFU = Face_up.Clone();
                Face_up = (Face_up + NFU.Transform(to_downE));

                Mesh<T> NFD = Face_down.Clone();  
                Face_down = (Face_down + NFD.Transform(to_upE));
            }

            float4x4 to_up =  Transforms.Translate(0,height,0);            
            Face_up = Face_up.Transform(to_up);

            float offset = 0.3f;
            float4x4 to_right =  Transforms.Translate(w/4 - offset, height/2, 0);
            float4x4 to_left =  Transforms.Translate(-w/4 + offset, height/2, 0);
            float4x4 to_left2 =  Transforms.Translate(-w/4, height/2, offset + 0.5f);
            float4x4 rotateC =  Transforms.RotateYGrad(90);    
            float4x4 to_rightC =  Transforms.Translate(w/4 + w,height/2,0);

            float lh = 0.5f*height;

            Mesh<T> sideA = G_Cilinder (height, l*2/3, 0, -0.1f, pi, -lh, lh, slices, stacks, invert);
            Mesh<T> sideB = G_Cilinder (height, l*2/3, 0, pi, 0.1f, -lh, lh, slices, stacks, invert);
            Mesh<T> sideC = G_Cilinder (height, w/2, 0, -0.1f, pi, -lh, lh, slices, stacks, invert);

            sideA = sideA.Transform(to_right);
            sideB = sideB.Transform(to_left);
            sideC = sideC.Transform(rotateC);
            sideC = sideC.Transform(to_left2);

            Mesh<T> sides= (sideA + sideB + sideC);
            Mesh<T> faces = (Face_up + Face_down);
            Mesh<T> result = (faces + sides).Weld();

            float4x4 trasl = Transforms.Translate(0,0,7.75f);
            result = result.Transform(trasl);

            return result;
        }


        public static Mesh<T> Protector_Face(float height = 1, float w = 4, float l = 2, int slices = 25, int stacks = 25, bool invert = false)
        {
            float corner_w = w/2;
            float corner_l = l/2f;
            Mesh<T> CornerA = Rect_Triangle(corner_w, corner_l, pi/4f, slices, stacks, invert);
            Mesh<T> CornerB = Rect_Triangle(corner_w, corner_l, pi/4f, slices, stacks, invert);

            float4x4 tcornerA = Transforms.RotateZGrad(180);
            float4x4 tcornerB = Transforms.RotateXGrad(180);

            CornerA = CornerA.Transform(tcornerB);
            CornerA = CornerA.Transform(tcornerA);
            CornerB = CornerB.Transform(tcornerB);

            float4x4 to_right =  Transforms.Translate(-corner_w,0,0);
            float4x4 to_left =  Transforms.Translate(corner_w,0,0);
            CornerB = CornerB.Transform(to_right);
            CornerA = CornerA.Transform(to_left);

            Mesh<T> Rect = Rectangle(w, l*2/3, slices, stacks, invert);
            Rect = Rect.Transform(to_right);

            return (CornerA + CornerB + Rect). Weld();
        }


        public static Mesh<T> CrossGuard(float height = 0.2f, float w = 4.2f, float l = 0.2f, int pieces = 2, int slices = 25, int stacks = 25, bool invert = false)
        {
            float4x4 to_up = Transforms.Translate(0, l / pieces, 0);
            float4x4 rotateX = Transforms.RotateXGrad(180);
            float4x4 rotateY = Transforms.RotateYGrad(180);
            float4x4 tsideC = Transforms.Translate(0, 0, -l*1.5f);

            Mesh<T> SideA = OneSideCurvedRectangle(height, w, 0, height/2, 0.65f, slices, stacks, invert);
            Mesh<T> SideB = OneSideCurvedRectangle(height, w, 0, height/2, 0.65f, slices, stacks, invert);
            Mesh<T> SideC = CurvedParallelogram (height, w, 0, height/2, 0.45f, slices, stacks, invert);

            SideC = SideC.Transform(tsideC);
            SideA = (SideA + SideC);
            SideB = (SideA + SideC);

            SideB = SideB.Transform(rotateX);
            SideB = SideB.Transform(rotateY);

            Mesh<T> guard = SideA + SideB;
            float4x4 to_upO = Transforms.Translate(0, (float) l / PowOfTwo(pieces), 0);

            Mesh<T> NewSideO = guard.Clone();
            NewSideO = NewSideO.Transform(to_upO);
            guard = guard + NewSideO;
            for (int i = 0; i < pieces; i++)
            {
                float4x4 to_upE = Transforms.Translate(0, (float) l / PowOfTwo(pieces - i), 0);

                Mesh<T> NewSideA = guard.Clone();
                NewSideA = NewSideA.Transform(to_upE);
                guard = guard + NewSideA;
            }

            float4x4 trasl = Transforms.Translate(0,l * 2,7.35f);
            guard = guard.Transform(trasl);

            return guard;
        }


        public static Mesh<T> Blade(float length = 27.5f, float w = 0.55f, float l = 0.1f, float change_rate = 0.50f, int pieces = 2, int slices = 25, int stacks = 25, bool invert = false)
        {
            float4x4 rotateX = Transforms.RotateXGrad(180);
            float4x4 rotateZ = Transforms.RotateZGrad(180);
            float4x4 rotateY = Transforms.RotateYGrad(180);

            Mesh<T> partA = TriangleRectangle(w, length, 0, length * 0.9f, change_rate, slices, stacks, invert);
            Mesh<T> partB = partA.Clone();
            partA = partA.Transform(rotateX);
            partB = partB.Transform(rotateX);
            partB = partB.Transform(rotateZ);
           
            Mesh<T> body = partA + partB;

            float4x4 to_upO1 = Transforms.Translate(0, (float) l / PowOfTwo(pieces), 0);
            Mesh<T> NewBodyO = body.Clone();
            NewBodyO = body.Transform(to_upO1);
            body = body + NewBodyO;
            for (int i = 0; i < pieces; i++)
            {
                float4x4 to_upE = Transforms.Translate(0, (float) l / PowOfTwo(pieces - i), 0);

                Mesh<T> NewBody = body.Clone();
                NewBody = body.Transform(to_upE);
                body = body + NewBody;
            }      
           
            Mesh<T> tipA = Rect_Triangle(w, 6*w, pi/2.55f, slices, stacks, invert);
            Mesh<T> tipB = tipA.Clone();
            tipB = tipB.Transform(rotateZ);

            float4x4 to_left = Transforms.Translate(w, 0, 0);
            float4x4 to_right = Transforms.Translate(-w, 0, 0);

            tipA = tipA.Transform(to_right);
            tipB = tipB.Transform(to_left);
            Mesh<T> tip = tipA + tipB;
           
            float4x4 to_upO = Transforms.Translate(0, (float) l / PowOfTwo(pieces), 0);
            Mesh<T> NewTipO = tip.Clone();
            NewTipO = tip.Transform(to_upO);
            tip = tip + NewTipO;
            for (int i = 0; i < pieces; i++)
            {
                float4x4 to_upE = Transforms.Translate(0, (float) l / PowOfTwo(pieces - i), 0);

                Mesh<T> NewTip = tip.Clone();
                NewTip = tip.Transform(to_upE);
                tip = tip + NewTip;
            }  
            
            float4x4 transl = Transforms.Translate(0, 0, 0);
            tip = tip.Transform(transl);

            Mesh<T> blade = tip + body;

            Mesh<T> line = G_Cilinder(0.090f, length, 0, pi, pi, 0, pi, slices, stacks, invert);
            float4x4 tline = Transforms.Translate(0, 0.420f, 7.9f);
            line = line.Transform(tline);

            float4x4 ftransl = Transforms.Translate(0, 0.4f, length + 8f);
            blade = blade.Transform(ftransl);
            return blade + line;
        }


        public static Mesh<T> hilt()
        {
            return Pomel() + Grip() + Protector() + CrossGuard();
        }

        #endregion


         #region Metodos

        public static float3 EvalBezier(float3[] control, float t)
        {
            // DeCasteljau
            if (control.Length == 1)
                return control[0]; // stop condition
            float3[] nestedPoints = new float3[control.Length - 1];
            for (int i = 0; i < nestedPoints.Length; i++)
                nestedPoints[i] = lerp(control[i], control[i + 1], t);
            return EvalBezier(nestedPoints, t);
        }

        public static float PowOfTwo(int n)
        {
            float result = 1;
            for (int i = 0; i<n ; i++)
            {
                result = 2*result;
            }
            return result;
        }
         #endregion

    }
}
