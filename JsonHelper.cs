using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

internal class AppData
{
    public List<Braetspil> Lager { get; set; } = new();
    public Dictionary<int, Kunde> Kunder { get; set; } = new();
    public List<Forespoergsel> Forespoergsler { get; set; } = new();
    public List<Medarbejder> Medarbejdere { get; set; } = new();
    public List<Reservation> Reservationer { get; set; } = new();
}

internal static class JSONHelper
{
    private const string filePath = "appdata.json";

    private static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        IncludeFields = true
    };

    public static AppData LoadAppData()
    {
        if (!File.Exists(filePath))
            return new AppData();

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<AppData>(json, options) ?? new AppData();
    }

    public static void SaveAppData(AppData data)
    {
        var json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }
}
