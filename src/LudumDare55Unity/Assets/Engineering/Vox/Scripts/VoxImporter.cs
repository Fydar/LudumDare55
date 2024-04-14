#if UNITY_EDITOR
using Fydar.Vox.Meshing;
using Fydar.Vox.VoxFiles;
using Fydar.Vox.VoxFiles.Hierarchy;
using System.IO;
using System.Linq;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "vox")]
public class VoxImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var fileInfo = new FileInfo(ctx.assetPath);
        byte[] data = File.ReadAllBytes(fileInfo.FullName);

        var scene = new VoxelScene(new VoxDocument(data));

        RecursiveAdd(scene, ctx, scene.Root);

        // ctx.SetMainObject(mesh);
    }

    private void RecursiveAdd(VoxelScene scene, AssetImportContext ctx, VoxelNode node, string name = "")
    {
        if (node is VoxelGrouping voxelGrouping)
        {
            foreach (var child in voxelGrouping.Children)
            {
                RecursiveAdd(scene, ctx, child);
            }
        }
        else if (node is VoxelShape shape)
        {
            foreach (var model in shape.Models)
            {
                var mesh = VoxelGenerator.GenerateMesh(model);

                string meshName = $"{model.Parents.FirstOrDefault()?.Parent?.Name ?? ""}";

                if (meshName.StartsWith('~'))
                {
                    continue;
                }
                if (meshName.EndsWith("template"))
                {
                    continue;
                }

                mesh.name = meshName;
                ctx.AddObjectToAsset(meshName, mesh);

                /*
                var newModel = new EditableVoxelModel(scene, model.Width + 2, model.Height + 2, model.Depth + 2);
                for (int x = 0; x < model.Width; x++)
                {
                    for (int y = 0; y < model.Depth; y++)
                    {
                        for (int z = 0; z < model.Height; z++)
                        {
                            var voxel = model.GetWithRangeCheck(x, y, z);
                            var oldPos = new Vector3SByte(x, y, z);
                            var newPos = new Vector3SByte(x + 1, y + 1, z + 1);

                            if (!voxel.IsEmpty)
                            {
                                newModel.Voxels[newPos.x, newPos.y, newPos.z] = new VoxelModelVoxel() { Index = 1 };

                                var forward = model.GetWithRangeCheck(oldPos.x, oldPos.y + 1, oldPos.z);
                                if (forward.IsEmpty)
                                {
                                    newModel.Voxels[newPos.x, newPos.y + 1, newPos.z] = new VoxelModelVoxel() { Index = 1 };
                                }

                                var back = model.GetWithRangeCheck(oldPos.x, oldPos.y - 1, oldPos.z);
                                if (back.IsEmpty)
                                {
                                    newModel.Voxels[newPos.x, newPos.y - 1, newPos.z] = new VoxelModelVoxel() { Index = 1 };
                                }

                                var left = model.GetWithRangeCheck(oldPos.x - 1, oldPos.y, oldPos.z);
                                if (left.IsEmpty)
                                {
                                    newModel.Voxels[newPos.x - 1, newPos.y, newPos.z] = new VoxelModelVoxel() { Index = 1 };
                                }

                                var right = model.GetWithRangeCheck(oldPos.x + 1, oldPos.y, oldPos.z);
                                if (right.IsEmpty)
                                {
                                    newModel.Voxels[newPos.x + 1, newPos.y, newPos.z] = new VoxelModelVoxel() { Index = 1 };
                                }

                                var up = model.GetWithRangeCheck(oldPos.x, oldPos.y, oldPos.z + 1);
                                if (up.IsEmpty)
                                {
                                    newModel.Voxels[newPos.x, newPos.y, newPos.z + 1] = new VoxelModelVoxel() { Index = 1 };
                                }

                                var down = model.GetWithRangeCheck(oldPos.x, oldPos.y, oldPos.z - 1);
                                if (down.IsEmpty)
                                {
                                    newModel.Voxels[newPos.x, newPos.y, newPos.z - 1] = new VoxelModelVoxel() { Index = 1 };
                                }
                            }
                        }
                    }
                }

                var shellMesh = VoxelGenerator.GenerateMesh(newModel);
                string shellMeshName = $"{name}-mesh-shell";
                shellMesh.name = shellMeshName;
                ctx.AddObjectToAsset(shellMeshName, shellMesh);
                */
            }
        }
        else if (node is VoxelTransform transformNode)
        {
            RecursiveAdd(scene, ctx, transformNode.ChildNode, node.Name);
        }
    }
}
#endif
