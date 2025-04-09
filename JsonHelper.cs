using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
// Evt. lav så at når man ændre kunde eller brætspilnavn og relaoder programmet - så følger de atributes med i gemte reservationer og forespoergsler.

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

    public static void LoadAll()
    {
        BraetspilManager.Instance.LoadJSON();
        KundeManager.Instance.LoadJSON();
        ForespoergselManager.Instance.LoadJSON();
        MedarbejderManager.Instance.LoadJSON();
        ReservationManager.Instance.LoadJSON();

        if (!File.Exists("appdata.json") || new FileInfo("appdata.json").Length == 0)
        {
            BraetspilManager.Instance.TilfoejDefaultSpil();
            MedarbejderManager.Instance.TilfoejDefaultMedarbejdere();
            SaveAll();
        }

        AppData data = JSONHelper.LoadAppData();
    }

    public static void SaveAll()
    {
        BraetspilManager.Instance.SaveJSON();
        KundeManager.Instance.SaveJSON();
        ForespoergselManager.Instance.SaveJSON();
        MedarbejderManager.Instance.SaveJSON();
        ReservationManager.Instance.SaveJSON();


    }
}
