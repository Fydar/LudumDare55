using UnityEngine;

[PreferBinarySerialization]
public class VoxData : ScriptableObject
{
	[HideInInspector]
	public byte[] Data;
}
