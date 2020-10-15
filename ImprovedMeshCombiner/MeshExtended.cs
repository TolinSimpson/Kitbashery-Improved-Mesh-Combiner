//
//  Extended Mesh Data.
//



using System.Collections.Generic;
using UnityEngine;



namespace wkepro {



   public class MeshExtended {


     
      public enum SizeMethod { Bounds, WorldScale}



      class VertexUV {
         public Vector3 Position;
         public Vector2 UV;
      }



      public Vector3[]  Vertices;
      public Vector2[]  UVs;
      public Vector3[]  Normals;
      public int[]      Triangles;
      public Vector3    Size;



      static int FindWeld(List<VertexUV> list,Vector3 vertex,Vector2 uv) {
         // Find out if there's is a suitable vertext to weld.
         return list.FindIndex(e => {
            Vector3 p = e.Position;
            Vector3 u = e.UV;
            return p.x==vertex.x && p.y==vertex.y && p.z==vertex.z && u.x==uv.x && u.y==uv.y;
            // Notice we take into account the UVs, otherwise we will lose texture info.
         });
      }



     
      public void Prepare(Mesh mesh,bool weld,Transform root,Transform trans,SizeMethod szm) {

         // Get data from the mesh...
         Vector3[]      vertices    = mesh.vertices;
         Vector2[]      uvs         = mesh.uv;
         Vector3[]      normals     = mesh.normals;
         int[]          triangles   = mesh.triangles;
         int            vertexCount = vertices.Length;
         int            triCount    = triangles.Length;
         List<VertexUV> wVertices   = new List<VertexUV>();
         List<Vector3>  wNormals    = new List<Vector3>();
         List<int>      wTriangles  = new List<int>();

         if(weld) {

            for(int v = 0;v<vertexCount;v++) {
               Vector3 currentVertex = vertices[v];
               Vector2 currentUV     = uvs[v];
               int     index         = FindWeld(wVertices,currentVertex,currentUV);
               if(index==-1) {
                  // no weld point found, so add the vertex and UV to the list.
                  wVertices.Add(new VertexUV { Position = currentVertex,UV = currentUV });
                  wNormals.Add(normals[v]);
                  index = wVertices.Count-1;
               }
               // the value of v and index will be always equal unless a weld point was found.
               if(v!=index) {
                  // if there's a weld, we need to update the triangles.
                  for(int t = 0;t<triCount;t++) {
                     if(triangles[t]==v) {
                        triangles[t] = index;
                     }
                  }
               }
            }

            // wait untill all the vertex are processed to update the triangle's list.
            for(int t = 0;t<triCount;t++) {
               wTriangles.Add(triangles[t]);
            }

         } else {

            // just copy vertices and UV
            for(int v = 0;v<vertexCount;v++) {
               wVertices.Add(new VertexUV { Position = vertices[v],UV=uvs[v] });
               wNormals.Add(normals[v]);
            }

            // and the triangles
            for(int t = 0;t<triCount;t++) {
               wTriangles.Add(triangles[t]);
            }

         }

         // Save mesh data.
         Normals    = wNormals.ToArray();
         Triangles  = wTriangles.ToArray();

         // Calculate the UV scale.
         if(szm==SizeMethod.Bounds) {
            Size = mesh.bounds.size;
         } else {
            Size  = trans.localScale;
            Transform parent = trans.parent;
            while(parent!=null) {
               Vector3 pscale = parent.localScale;
               Size.x *= pscale.x;
               Size.y *= pscale.y;
               Size.z *= pscale.z;
               parent  = parent.parent;
            }
         }

         // Save vertices and UVs
         int pcount = wVertices.Count;
         Vertices   = new Vector3[pcount];
         UVs        = new Vector2[pcount];
         for(int v = 0;v<pcount;v++) {
            // convert local mesh coordinates to world space.
            Vector3 worldPos = trans.TransformPoint(wVertices[v].Position);
            // convert world space to the root's local space.
            Vertices[v]      = root.InverseTransformPoint(worldPos);
            UVs[v]           = wVertices[v].UV;
         }

      }


   }

}