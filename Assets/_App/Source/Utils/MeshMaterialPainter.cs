using System.Linq;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class MeshMaterialPainter
    {
        private static readonly int BaseColorProperty = Shader.PropertyToID("_BaseColor");

        private Material[] _materialsToPaint;
        
        public MeshMaterialPainter(MeshRenderer mesh, Material[] baseMaterials)
        {
            _materialsToPaint = mesh.materials.Where(mat =>
                baseMaterials.Any(baseMaterial => $"{baseMaterial.name} (Instance)" == mat.name)).ToArray();
        }
        
        public void SetColor(Color color)
        {
            foreach (var material in _materialsToPaint) 
                material.SetColor(BaseColorProperty, color);
        }
    }
}