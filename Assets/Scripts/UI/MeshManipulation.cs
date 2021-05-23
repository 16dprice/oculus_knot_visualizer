using UnityEngine;
using System.Collections.Generic;


namespace UI
{
    public static class MeshManipulation
    {
        public static List<GameObject> DisplayLink(Transform parentTransform, LinkStickModel stickModel, int sides = 6, float radius = 0.02f)
        {
            foreach (Transform child in parentTransform)
            {
                Object.Destroy(child.gameObject);
            }

            var knotMeshObjects = stickModel.GetKnotMeshObjects(sides, radius);

            foreach (var meshObject in knotMeshObjects)
            {
                if (meshObject != null)
                {
                    meshObject.transform.parent = parentTransform;
                    ResetTransform(meshObject);
                }
            }

            return knotMeshObjects;
        }
    
        static void ResetTransform(GameObject obj)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }
    }
}