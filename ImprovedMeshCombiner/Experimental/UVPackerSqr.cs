/*
 
MIT License

Copyright (c) 2020 Kitbashery

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


Created by Jorge Reyna Tamez @ https://www.fiverr.com/wkepro
Created for Kitbashery @ https://www.kitbashery.com

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kitbashery.MeshCombiner.Experimental
{
    public static class UVPackerSqr
    {

        public class Box
        {
            public MeshExtended Extended;
            public Vector2[] PackedUVs;
        }



        public static Box FitUVs(MeshExtended extended, float x, float y, float side)
        {

            Vector2[] uvs = extended.UVs;
            int count = uvs.Length;
            Vector2[] packed = new Vector2[count];

            float xMin = Mathf.Infinity;
            float xMax = -Mathf.Infinity;
            float yMin = Mathf.Infinity;
            float yMax = -Mathf.Infinity;

            foreach (Vector2 v2 in uvs)
            {
                if (v2.x < xMin)
                {
                    xMin = v2.x;
                }
                if (v2.x > xMax)
                {
                    xMax = v2.x;
                }
                if (v2.y < yMin)
                {
                    yMin = v2.y;
                }
                if (v2.y > yMax)
                {
                    yMax = v2.y;
                }
            }

            float width = xMax - xMin;
            float height = yMax - yMin;

            // debug:
            //float dminx = Mathf.Infinity;
            //float dmaxx = -Mathf.Infinity;
            //float dminy = Mathf.Infinity;
            //float dmaxy = -Mathf.Infinity;

            for (int u = 0; u < count; u++)
            {
                Vector2 uv = uvs[u];
                packed[u].x = x + (uv.x - xMin) / width * side;
                packed[u].y = y + (uv.y - yMin) / height * side;
                // debug:
                //if(packed[u].x<dminx) {
                //   dminx = packed[u].x;
                //}
                //if(packed[u].x>dmaxx) {
                //   dmaxx = packed[u].x;
                //}
                //if(packed[u].y<dminy) {
                //   dminy = packed[u].y;
                //}
                //if(packed[u].y>dmaxy) {
                //   dmaxy = packed[u].y;
                //}
            }

            //debug:
            //if(dminx<x || dmaxx>x+side) {
            //   Debug.Log("sx1="+x+", sx2= "+x+side+", x1="+dminx+", x2="+dmaxx);
            //}
            //if(dminy<y || dmaxy>y+side) {
            //   Debug.Log("sy1="+y+", sy2= "+y+side+", y1="+dminy+", y2="+dmaxy);
            //}
            return new Box { Extended = extended, PackedUVs = packed };

        }



        public static Box[] Pack(MeshExtended[] extended)
        {

            int squares = extended.Length;
            int width = (int)Mathf.Sqrt(squares);
            int length = (squares / width) - 1;
            int remaining = squares - length * width;
            if (remaining > width)
            {
                length++;
                remaining -= width;
            }
            float regularSide = 1f / (float)width;
            Box[] boxes = new Box[squares];
            int box = 0;

            float x;
            float y = 0;
            for (int l = 0; l < length; l++)
            {
                x = 0;
                for (int w = 0; w < width; w++)
                {
                    boxes[box] = FitUVs(extended[box], x, y, regularSide);
                    box++;
                    x += regularSide;
                }
                y += regularSide;
            }
            if (remaining > 0)
            {
                x = 0;
                for (int r = 0; r < remaining; r++)
                {
                    boxes[box] = FitUVs(extended[box], x, y, regularSide);
                    box++;
                    x += regularSide;
                }
            }

            return boxes;

        }

    }



}