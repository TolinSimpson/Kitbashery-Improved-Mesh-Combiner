//
// Mesh Combiner.
//



using System.Collections.Generic;
using UnityEngine;



namespace wkepro {



   public static class MeshCombinerSqr {



      public static Mesh Combine(MeshFilter[] filters,bool weld) {

         if(filters.Length==0) {
            Debug.LogError("Mesh list is empty.");
            return null;
         }

         Transform      root      = filters[0].transform;
         int            meshCount = filters.Length;
         MeshExtended[] extended  = new MeshExtended[meshCount];
         for(int m = 0;m<meshCount;m++) {
            MeshFilter filter = filters[m];
            extended[m]       = new MeshExtended();
            extended[m].Prepare(filter.sharedMesh,weld,root,filter.GetComponent<Transform>(),MeshExtended.SizeMethod.Bounds);
         }

         UVPackerSqr.Box[]  boxes            = UVPackerSqr.Pack(extended);
         Mesh               combined         = new Mesh();
         List<Vector3>      combinedVertices = new List<Vector3>();
         List<Vector2>      combinedUVs      = new List<Vector2>();
         List<Vector3>      combinedNormals  = new List<Vector3>();
         List<int>          combinedTris     = new List<int>();
         int                triOffset        = 0;

         for(int meshIndex = 0;meshIndex<meshCount;meshIndex++) {

            MeshExtended extmesh     = boxes[meshIndex].Extended;
            Vector3[]    vertices    = extmesh.Vertices;
            Vector2[]    uvs         = boxes[meshIndex].PackedUVs;
            Vector3[]    normals     = extmesh.Normals;
            int[]        triangles   = extmesh.Triangles;
            int          vertexCount = vertices.Length;
            int          triCount    = triangles.Length;

            for(int v = 0;v<vertexCount;v++) {
               combinedVertices.Add(vertices[v]);
               combinedUVs.Add(uvs[v]);
               combinedNormals.Add(normals[v]);
            }

            for(int t = 0;t<triCount;t++) {
               combinedTris.Add(triangles[t]+triOffset);
            }
            triOffset = combinedVertices.Count;

         }

         UVPacker.AdjustSpace(combinedUVs);

         combined.SetVertices(combinedVertices);
         combined.SetUVs(0,combinedUVs);
         combined.SetNormals(combinedNormals);
         combined.SetTriangles(combinedTris,0);

         return combined;

      }



   }



}