using Godot;

namespace Template.Core;

public static class Logger
{
    private const string ServerEmoji = "SV/🔵";
    private const string ClientEmoji = "CL/🟠";

    public static void Info(string content)
    {
        GD.Print(FormatContent(content));
    }
    
    public static void Info(string content, Node node)
    {
        GD.Print(FormatContent(content, node));
    }
    
    public static void Info(Node node, string content)
    {
        GD.Print(FormatContent(content, node));
    }

    private static string FormatContent(string content, Node? node = null)
    {
        var timestamp = (Time.GetTicksMsec()/1000f).ToString("F");
        if (node is not null)
        {
            var multiplayer = node.Multiplayer.HasMultiplayerPeer();
            var server = node.Multiplayer.IsServer() && node.IsMultiplayerAuthority();
        
            if (multiplayer)
            {
                string emoji = server ? ServerEmoji : ClientEmoji;
                return $"<{timestamp}> [{emoji}]: {content}";
            }  
        }

        return $"<{timestamp}>: {content}";
    }
}