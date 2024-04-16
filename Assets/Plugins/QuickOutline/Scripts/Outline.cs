//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuickOutline
{
    [DisallowMultipleComponent]
    public class Outline : MonoBehaviour
    {
        private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

        public enum Mode
        {
            OutlineAll,
            OutlineVisible,
            OutlineHidden,
            OutlineAndSilhouette,
            SilhouetteOnly
        }

        public Mode OutlineMode
        {
            get => outlineMode;
            set
            {
                outlineMode = value;
                UpdateMaterialProperties();;
            }
        }

        public Color OutlineColor
        {
            get => outlineColor;
            set
            {
                outlineColor = value;
                UpdateMaterialProperties();;
            }
        }

        public float OutlineWidth
        {
            get => outlineWidth;
            set
            {
                outlineWidth = value;
                UpdateMaterialProperties();;
            }
        }

        [Serializable]
        private class ListVector3
        {
            public List<Vector3> data;
        }

        [SerializeField] private Mode outlineMode;

        [SerializeField] private Color outlineColor = Color.white;

        [SerializeField, Range(0f, 10f)] private float outlineWidth = 2f;

        [SerializeField, HideInInspector] private List<Mesh> bakeKeys = new List<Mesh>();
        [SerializeField, HideInInspector] private List<ListVector3> bakeValues = new List<ListVector3>();

        private Renderer[] renderers;
        private Material outlineMaskMaterial;
        private Material outlineFillMaterial;

        private bool needsUpdate;

        public void Initialize()
        {
            renderers = GetComponentsInChildren<Renderer>();

            outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
            outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

            outlineMaskMaterial.name = "OutlineMask (Instance)";
            outlineFillMaterial.name = "OutlineFill (Instance)";

            LoadSmoothNormals();

            UpdateMaterialProperties();

            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials.ToList();

                materials.Add(outlineMaskMaterial);
                materials.Add(outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        private void Bake()
        {
            var bakedMeshes = new HashSet<Mesh>();

            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {
                if (!bakedMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                bakeKeys.Add(meshFilter.sharedMesh);
                bakeValues.Add(new ListVector3() { data = smoothNormals });
            }
        }

        private void LoadSmoothNormals()
        {
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {
                if (!registeredMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
                var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

                meshFilter.sharedMesh.SetUVs(3, smoothNormals);

                var renderer = meshFilter.GetComponent<Renderer>();

                if (renderer != null)
                {
                    CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
                }
            }

            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                {
                    continue;
                }

                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

                CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
            }
        }

        private List<Vector3> SmoothNormals(Mesh mesh)
        {
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index))
                .GroupBy(pair => pair.Key);

            var smoothNormals = new List<Vector3>(mesh.normals);

            foreach (var group in groups)
            {
                if (group.Count() == 1)
                {
                    continue;
                }

                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        private void CombineSubmeshes(Mesh mesh, Material[] materials)
        {
            if (mesh.subMeshCount == 1)
            {
                return;
            }

            if (mesh.subMeshCount > materials.Length)
            {
                return;
            }

            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }

        private void UpdateMaterialProperties()
        {
            outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

            switch (outlineMode)
            {
                case Mode.OutlineAll:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineVisible:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineHidden:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineAndSilhouette:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.SilhouetteOnly:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                    break;
            }
        }
    }
}