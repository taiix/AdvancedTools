using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PathMetricsLogger
{
    private const string DefaultFileName = "theta_metrics.csv";
    private const string Header = "timestamp,scene,agentName,success,startX,startY,startZ,endX,endY,endZ,pathNodes,pathLength,turns,pathTimeMs,moveTimeMs,avgSpeed";

    private static readonly object FileLock = new object();
    private static string _filePath;

    static PathMetricsLogger()
    {
        _filePath = Path.Combine(Application.persistentDataPath, DefaultFileName);
        EnsureHeader();
    }

    public static void SetFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return;
        _filePath = Path.Combine(Application.persistentDataPath, fileName);
        EnsureHeader();
    }

    public static void Append(string agentName,
                              bool success,
                              long pathTimeMs,
                              double moveTimeMs,
                              double avgSpeed,
                              float pathLength,
                              int turnCount,
                              int nodeCount,
                              Vector3 start,
                              Vector3 end)
    {
        var ci = CultureInfo.InvariantCulture;
        string scene = SceneManager.GetActiveScene().name;
        string timestamp = DateTime.Now.ToString("o", ci); // ISO 8601

        string line = string.Join(",", new string[]
        {
            timestamp,
            Escape(scene),
            Escape(agentName),
            success ? "true" : "false",
            start.x.ToString(ci),
            start.y.ToString(ci),
            start.z.ToString(ci),
            end.x.ToString(ci),
            end.y.ToString(ci),
            end.z.ToString(ci),
            nodeCount.ToString(ci),
            pathLength.ToString(ci),
            turnCount.ToString(ci),
            pathTimeMs.ToString(ci),
            moveTimeMs.ToString("F3", ci),
            avgSpeed.ToString("F6", ci)
        });

        try
        {
            lock (FileLock)
            {
                using (var sw = new StreamWriter(_filePath, true))
                {
                    sw.WriteLine(line);
                }
            }
        }
        catch (IOException ex)
        {
            Debug.LogError($"PathMetricsLogger: Failed to append CSV row to '{_filePath}': {ex.Message}");
        }
    }

    private static void EnsureHeader()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                lock (FileLock)
                {
                    using (var sw = new StreamWriter(_filePath, false))
                    {
                        sw.WriteLine(Header);
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Debug.LogError($"PathMetricsLogger: Failed to create CSV '{_filePath}': {ex.Message}");
        }
    }

    private static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        bool mustQuote = input.Contains(",") || input.Contains("\"") || input.Contains("\n") || input.Contains("\r");
        if (!mustQuote) return input;
        return "\"" + input.Replace("\"", "\"\"") + "\"";
    }
}