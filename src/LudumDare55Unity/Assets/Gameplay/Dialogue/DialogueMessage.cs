using System;
using UnityEngine;

[Serializable]
public struct DialogueMessage
{
	[TextArea(1, 3)]
	public string Body;

	[Space]
	public DialogueCharacter Character;
	public DialogueFont Font;
}
