using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelGenerator
{
    private static readonly Vector3[] vectorMapping = new Vector3[]
    {
        new Vector3(0, 1, 0), // Up
		new Vector3(0, -1, 0), // Down
		new Vector3(-1, 0, 0), // Left
		new Vector3(1, 0, 0), // Right
		new Vector3(0, 0, 1), // Forward
		new Vector3(0, 0, -1), // Back
	};

    public static Mesh GenerateMesh(VoxelModel voxelModel)
    {
        var voxelDriver = new ModelVoxelizer(voxelModel);
        var groupedMesher = new GroupedMesher(voxelDriver);
        var greedyMesher = new GreedyMesher();
        var grouped = groupedMesher.Voxelize();
        var greedyMesh = greedyMesher.Optimize(grouped);

        int faces = 0;
        for (int i = 0; i < greedyMesh.Surfaces.Length; i++)
        {
            var surface = greedyMesh.Surfaces[i];
            faces += surface.Faces.Length;
        }

        var vertices = new Vector3[faces * 4];
        var normals = new Vector3[faces * 4];
        var colours = new Color32[faces * 4];
        int[] triangles = new int[faces * 6];

        var offset = new Vector3(
            voxelModel.Width * 0.5f,
            voxelModel.Height * 0.5f,
            voxelModel.Depth * 0.5f
        );

        int index = 0;
        for (int i = 0; i < greedyMesh.Surfaces.Length; i++)
        {
            var surface = greedyMesh.Surfaces[i];

            var normal = vectorMapping[surface.Description.Normal.data];

            var colour = new Color32(
                surface.Description.Colour.r,
                surface.Description.Colour.g,
                surface.Description.Colour.b,
                255);

            foreach (var face in surface.TransformedFaces)
            {
                int vOffset = index * 4;
                int tOffset = index * 6;

                vertices[vOffset] = ToVector3(face.TopLeft) - offset;
                vertices[vOffset + 1] = ToVector3(face.TopRight) - offset;
                vertices[vOffset + 2] = ToVector3(face.BottomLeft) - offset;
                vertices[vOffset + 3] = ToVector3(face.BottomRight) - offset;

                triangles[tOffset] = vOffset;
                triangles[tOffset + 1] = vOffset + 1;
                triangles[tOffset + 2] = vOffset + 2;

                triangles[tOffset + 3] = vOffset + 1;
                triangles[tOffset + 4] = vOffset + 3;
                triangles[tOffset + 5] = vOffset + 2;

                colours[vOffset] = colour;
                colours[vOffset + 1] = colour;
                colours[vOffset + 2] = colour;
                colours[vOffset + 3] = colour;

                normals[vOffset] = normal;
                normals[vOffset + 1] = normal;
                normals[vOffset + 2] = normal;
                normals[vOffset + 3] = normal;

                index++;
            }
        }

        var mesh = new Mesh()
        {
            vertices = vertices,
            triangles = triangles,
            colors32 = colours,
            normals = normals
        };
        mesh.RecalculateBounds();

        return mesh;
    }

    private static Vector3 ToVector3(Vector3SByte vector3SByte)
    {
        return new Vector3(
            vector3SByte.x,
            vector3SByte.y,
            vector3SByte.z
        );
    }
}
