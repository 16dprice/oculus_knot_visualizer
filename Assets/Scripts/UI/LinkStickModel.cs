using System.Collections.Generic;
using BeadsProviders;
using Domain;
using UnityEngine;


namespace UI
{
    public class LinkStickModel
    {
        const float PI = 3.1415926535898f;

        private readonly List<LinkComponent> _componentList;

        public LinkStickModel(ILinkBeadsProvider beadsProvider)
        {
            _componentList = beadsProvider.GetLinkComponents();
        }

        public LinkStickModel(List<LinkComponent> componentList)
        {
            _componentList = componentList;
        }

        public List<GameObject> GetKnotMeshObjects(int sides, float radius)
        {
            var knotMeshObjects = new List<GameObject>();

            foreach (var linkComponent in _componentList)
            {
                knotMeshObjects.Add(GetKnotMeshObject(linkComponent.BeadList, sides, radius));
            }

            return knotMeshObjects;
        }

        private GameObject GetKnotMeshObject(List<Bead> beads, int sides, float radius)
        {
            var _knotMaterial = Resources.Load<Material>("KnotMaterial");
            
            if (beads.Count < 3) return null;

            var newKnot = new GameObject();

            var meshFilter = newKnot.AddComponent(typeof(MeshFilter)) as MeshFilter;
            var meshRenderer = newKnot.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

            var mesh = new Mesh();

            mesh.vertices = GetVertices(beads, sides, radius);
            mesh.triangles = GetTriangles(beads, sides);
            mesh.uv.Initialize();
            mesh.uv = GetUvs(beads.Count * sides);
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh; //assign the mesh
            meshRenderer.sharedMaterial = _knotMaterial; //assign our new knot material

            return newKnot;
        }

        private Vector3[] GetVertices(List<Bead> beads, int sides, float radius)
        {
            var vertices = new Vector3[beads.Count * sides];

            //Define an n-gon by points on the xy plane then scale it to radius
            var ngon = GenerateNGonPoints(sides, radius);
            var ngonNormal = Vector3.back;

            //first few vertices around the bead
            var ngonRotation = Quaternion.FromToRotation(ngonNormal, beads[1].position - beads[beads.Count - 1].position);
            for (var j = 0; j < sides; j++)
                vertices[j] = ngonRotation * ngon[j] + beads[0].position;

            //Generate remaining vertices
            for (var i = 1; i < beads.Count - 1; i++)
            {
                /*Rotate the ngon such that the normal is parallel to the
             * line drawn between the two beads surrounding the bead beads[i] */
                ngonRotation =
                    Quaternion.FromToRotation(ngonNormal,
                        beads[i + 1].position -
                        beads[i - 1].position); //Somehow this rotation looks a lot better than I was expecting... thanks quaternions!

                for (var j = 0; j < sides; j++)
                    vertices[sides * i + j] = ngonRotation * ngon[j] + beads[i].position;
            }

            //last case
            ngonRotation = Quaternion.FromToRotation(ngonNormal, beads[0].position - beads[beads.Count - 2].position);
            for (var j = 0; j < sides; j++)
                vertices[sides * (beads.Count - 1) + j] = ngonRotation * ngon[j] + beads[beads.Count - 1].position;

            return vertices;
        }

        private List<Vector3> GenerateNGonPoints(int n, float radius)
        {
            var points = new List<Vector3>();

            //Don't forget the scale by radius
            for (float i = 0; i < Mathf.Abs(n); i++)
                points.Add(new Vector3(Mathf.Cos(i / n * 2 * PI), Mathf.Sin(i / n * 2 * PI), 0.0f) * radius);

            return points;
        }

        private int[] GetTriangles(List<Bead> beads, int sides)
        {
            var triangles = new int[beads.Count * 6 * sides];
            int p, passed;

            for (var i = 0; i < beads.Count - 1; i++)
            {
                p = 6 * sides * i;
                for (var j = 0; j < sides - 1; j++)
                {
                    passed = p + 6 * j;
                    triangles[passed + 0] = sides * i + j;
                    triangles[passed + 1] = sides * (i + 1) + j;
                    triangles[passed + 2] = sides * i + j + 1;
                    triangles[passed + 3] = sides * i + j + 1;
                    triangles[passed + 4] = sides * (i + 1) + j;
                    triangles[passed + 5] = sides * (i + 1) + j + 1;
                }

                //last side
                p += 6 * sides;
                triangles[p - 6] = sides * i + sides - 1;
                triangles[p - 5] = sides * (i + 1) + sides - 1;
                triangles[p - 4] = sides * i;
                triangles[p - 3] = sides * i;
                triangles[p - 2] = sides * (i + 1) + sides - 1;
                triangles[p - 1] = sides * (i + 1);
            }

            //last segment
            p = 6 * sides * (beads.Count - 1);
            for (var j = 0; j < sides - 1; j++)
            {
                passed = p + 6 * j;
                triangles[passed + 0] = sides * (beads.Count - 1) + j;
                triangles[passed + 1] = j;
                triangles[passed + 2] = sides * (beads.Count - 1) + j + 1;
                triangles[passed + 3] = sides * (beads.Count - 1) + j + 1;
                triangles[passed + 4] = j;
                triangles[passed + 5] = j + 1;
            }

            p += 6 * sides;
            triangles[p - 6] = sides * (beads.Count - 1) + sides - 1;
            triangles[p - 5] = sides - 1;
            triangles[p - 4] = sides * (beads.Count - 1);
            triangles[p - 3] = sides * (beads.Count - 1);
            triangles[p - 2] = sides - 1;
            triangles[p - 1] = 0;

            return triangles;
        }

        private Vector2[] GetUvs(int numberOfUvs)
        {
            var uvs = new Vector2[numberOfUvs];

            for (int i = 0; i < numberOfUvs; i++)
            {
                uvs[i] = new Vector2(1.0f, 1.0f);
            }

            return uvs;
        }
    }
}