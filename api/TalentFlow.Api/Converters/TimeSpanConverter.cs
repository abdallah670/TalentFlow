using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TalentFlow.Api.Converters;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return TimeSpan.Zero;

            if (TimeSpan.TryParse(value, out var result))
                return result;

            if (value.Contains(':'))
            {
                var parts = value.Split(':');
                if (parts.Length >= 2 && 
                    int.TryParse(parts[0], out int hours) && 
                    int.TryParse(parts[1], out int minutes))
                {
                    int seconds = 0;
                    if (parts.Length > 2 && int.TryParse(parts[2], out int secs))
                        seconds = secs;
                    return new TimeSpan(hours, minutes, seconds);
                }
            }
        }
        
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            int hours = 0, minutes = 0, seconds = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();
                    if (propName == "hours")
                        hours = reader.GetInt32();
                    else if (propName == "minutes")
                        minutes = reader.GetInt32();
                    else if (propName == "seconds")
                        seconds = reader.GetInt32();
                }
            }
            return new TimeSpan(hours, minutes, seconds);
        }

        return TimeSpan.Zero;
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
    }
}

public class NullableTimeSpanConverter : JsonConverter<TimeSpan?>
{
    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        var converter = new TimeSpanConverter();
        return converter.Read(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Value.ToString(@"hh\:mm\:ss"));
    }
}