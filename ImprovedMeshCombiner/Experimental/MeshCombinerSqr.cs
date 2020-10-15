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

using System.Collections.Generic;
using UnityEngine;



namespace Kitbashery.MeshCombiner.Experimental
{
    public static class MeshCombinerSqr
    {
        public static Mesh Combine(MeshFilter[] filters, bool weld)
        {

            if (filters.Length == 0)
            {
                Debug.LogError("Mesh list is empty.");
                return null;
            }

            Transform root = filters[0].transform;
            int meshCount = filters.Length;
            MeshExtended[] extended = new MeshExtended[meshCount];
            for (int m = 0; m < meshCount; m++)
            {
                MeshFilter filter = filters[m];
                extended[m] = new MeshExtended();
                extended[m].Prepare(filter.sharedMesh, weld, root, filter.GetComponent<Transform>(), MeshExtended.SizeMethod.Bounds);
            }

            UVPackerSqr.Box[] boxes = UVPackerSqr.Pack(extended);
            Mesh combined = new Mesh();
            List<Vector3> combinedVertices = new List<Vector3>();
            List<Vector2> combinedUVs = new List<Vector2>();
            List<Vector3> combinedNormals = new List<Vector3>();
            List<int> combinedTris = new List<int>();
            int triOffset = 0;

            for (int meshIndex = 0; meshIndex < meshCount; meshIndex++)
            {

                MeshExtended extmesh = boxes[meshIndex].Extended;
                Vector3[] vertices = extmesh.Vertices;
                Vector2[] uvs = boxes[meshIndex].PackedUVs;
                Vector3[] normals = extmesh.Normals;
                int[] triangles = extmesh.Triangles;
                int vertexCount = vertices.Length;
                int triCount = triangles.Length;

                for (int v = 0; v < vertexCount; v++)
                {
                    combinedVertices.Add(vertices[v]);
                    combinedUVs.Add(uvs[v]);
                    combinedNormals.Add(normals[v]);
                }

                for (int t = 0; t < triCount; t++)
                {
                    combinedTris.Add(triangles[t] + triOffset);
                }
                triOffset = combinedVertices.Count;

            }

            UVPacker.AdjustSpace(combinedUVs);

            combined.SetVertices(combinedVertices);
            combined.SetUVs(0, combinedUVs);
            combined.SetNormals(combinedNormals);
            combined.SetTriangles(combinedTris, 0);

            return combined;

        }
    }
}