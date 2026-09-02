using System.Text;
using System.Text.Json;

namespace TalentFlow.Application.Responses;


public static class CursorPagination
{
   
    public static string CreateCursor(DateTimeOffset createdAt, Guid id)
    {
        var cursorData = new CursorData
        {
            CreatedAt = createdAt,
            Id = id
        };
        var json = JsonSerializer.Serialize(cursorData);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }


     public static CursorData? DecodeCursor(string? cursor)
    {
        if (string.IsNullOrEmpty(cursor))
            return null;

        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            return JsonSerializer.Deserialize<CursorData>(json);
        }
        catch
        {
            return null;
        }
    }

    public class CursorPagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public string? NextCursor { get; set; }
        public bool HasMore { get; set; }
        public int PageSize { get; set; }
    }
}

public class CursorData
{
    public DateTimeOffset CreatedAt { get; set; }
    public Guid Id { get; set; }
}



