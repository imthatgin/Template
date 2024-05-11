using Godot;
using Template.Core;
using Template.Core.Dependencies;

namespace Template.Views.Game;

public partial class GameScene : Node3D
{
	[ExportCategory("Dependencies")]
	[Export, Require] public CsgPrimitive3D BaseObject { get; set; } = null!;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Debugging.Watch(BaseObject, "Rotation");
		Debugging.Watch(this, "ProcessMode");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		BaseObject.RotateY(.1f * (float)delta);
	}
}
