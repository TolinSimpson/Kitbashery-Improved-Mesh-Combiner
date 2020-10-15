//
// Test mesh combiner
//



using UnityEditor;
using UnityEngine;
using wkepro;



public class CombineObjectMenu {

   

   [MenuItem("Tools/New Combine Object")]
   public static void CombineObjet() {

      GameObject obj = Selection.activeGameObject;
      if(obj==null) {
         Debug.LogError("You must select a GameObject to combine.");
         return;
      }

      Undo.RegisterCreatedObjectUndo(obj,"Combine Object");
      Combine(obj);

   }



   static public void Combine(GameObject obj) {

      MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();

      Mesh mesh = MeshCombiner.Combine(filters,true,0.001f);

      AssetDatabase.CreateAsset(mesh,"Assets/Meshes/wa_"+obj.name+".asset");
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

      foreach(Vector2 v2 in uv) {
         if(v2.x < xMin) {
            xMin = v2.x;
         }
         if(v2.x > xMax) {
            xMax = v2.x;
         }
         if(v2.y < yMin) {
            yMin = v2.y;
         }
         if(v2.y > yMax) {
            yMax = v2.y;
         }
      }
      Debug.Log("x1="+xMin+", x2="+xMax+", y1="+yMin+",y2="+yMax);

   }



}



// End of file.