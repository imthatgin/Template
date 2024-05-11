using System.Linq;
using System.Text;
using Godot;
using Template.Core;
using Template.Core.Dependencies;

namespace Template.Views.Debug;

public partial class DebugView : Control
{
	[Export, Require] public RichTextLabel DebugRtl { get; set; } = null!;

	private readonly StringBuilder _builder = new();

	public override void _Process(double delta)
	{
		_builder.Clear();
		
		foreach (var grouping in Debugging.Watchers.GroupBy(w => w.Node))
		{
			var node = grouping.Key;
			_builder.Append($"{node.Name}[{node.ToString()}]\n");
			
			foreach (var container in grouping)
			{
				if(container.Value is null) continue;
				_builder.Append($"└── {container.Key}: {container.Value}\n");
			}

		}

		if (!Debugging.Watchers.Any())
			_builder.Append("[No watchers defined]");
		
		DebugRtl.Text = _builder.ToString();	
	}
}