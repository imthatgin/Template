using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace Template.Core.Converters;

[JsonConverter(typeof(Vector2I))]
public class Vector2IConverter : JsonConverter<Vector2I>
{
    public override Vector2I Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => (Vector2I)GD.StrToVar(reader.GetString()!);
    public override void Write(Utf8JsonWriter writer, Vector2I value, JsonSerializerOptions options) => writer.WriteStringValue(GD.VarToStr(value));
}