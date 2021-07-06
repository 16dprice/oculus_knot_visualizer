using UnityEngine;
using System.Collections.Generic;


namespace UI
{
    public static class MeshManipulation
    {
        public static void DisplayLink(Transform parentTransform, LinkStickModel stickModel, int sides = 6, float radius = 0.02f)
        {
            DeleteChildren(parentTransform);

            var knotMeshObjects = stickModel.GetKnotMeshObjects(sides, radius);

            foreach (var meshObject in knotMeshObjects)
            {
                if (meshObject != null)
                {
                    meshObject.transform.parent = parentTransform;
                    ResetTransform(meshObject);
                }
            }
            
            stickModel.SetLinkComponentGameObjects(knotMeshObjects);
        }

        public static void DeleteChildren(Transform parentTransform)
        {
            foreach (Transform child in parentTransform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    
        static void ResetTransform(GameObject obj)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }
    }
}