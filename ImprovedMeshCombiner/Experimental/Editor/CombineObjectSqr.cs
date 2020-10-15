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

using UnityEditor;
using UnityEngine;

namespace Kitbashery.MeshCombiner.Experimental
{

    public class CombineObjectMenuSqr
    {
        [MenuItem("Tools/Experimental/Kitbashery Combine Meshes (Square Grid Method)")]
        public static void CombineObjet()
        {

            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                Debug.LogError("You must select a GameObject to combine.");
                return;
            }

            Undo.RegisterCreatedObjectUndo(obj, "Combine Object");
            Combine(obj);

        }

        static public void Combine(GameObject obj)
        {

            MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();

            Mesh mesh = MeshCombinerSqr.Combine(filters, true);

            AssetDatabase.CreateAsset(mesh, "Assets/Meshes/wq_" + obj.name + ".asset");
            AssetDatabase.SaveAssets();
            Debug.Log("Mesh saved.");

            //GameObject export = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //export.name       = obj.name;
            //MeshFilter filter = export.GetComponent<MeshFilter>();
            //filter.sharedMesh = mesh;
            //OBJExport.Export(export,"Assets/Meshes");
            //GameObject.DestroyImmediate(export);


            Vector2[] uv = mesh.uv;
            float xMin = Mathf.Infinity;
            float xMax = -Mathf.Infinity;
            float yMin = Mathf.Infinity;
            float yMax = -Mathf.Infinity;

            foreach (Vector2 v2 in uv)
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
            Debug.Log("x1=" + xMin + ", x2=" + xMax + ", y1=" + yMin + ",y2=" + yMax);
        }
    }
}