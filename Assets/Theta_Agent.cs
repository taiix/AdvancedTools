using CodenameLib.Pathfinding;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Theta_Agent : MonoBehaviour
{
    [SerializeField] private Transform target;

    private ThetaStarAgent agent;
    private Stopwatch pathTimer;
    private Stopwatch moveTimer;

    private Vector3 destination;
    private Vector3 startPosition;
    private bool awaitingPathOnly;

    // Metrics
    private float lastPathLength;
    private int lastTurnCount;
    private int lastPathNodeCount;
    private long lastPathTimeMs;

    // CSV
    private string csvPath;

    private void Start()
    {
        agent = GetComponent<ThetaStarAgent>();
        if (agent == null)
        {
            UnityEngine.Debug.LogError("ThetaStarAgent component not found on the GameObject.");
            return;
        }

        if (target == null)
        {
            UnityEngine.Debug.LogError("Target is not assigned on Theta_Agent.");
            return;
        }

        // Subscribe to events
        agent.OnPathComplete += HandlePathComplete;
        agent.OnMovementStart += HandleMovementStart;
        agent.OnMovementComplete += HandleMovementComplete;

        pathTimer = new Stopwatch();
        moveTimer = new Stopwatch();

        // CSV setup
        PrepareCsv();

        // Accurate measurement: compute path first (no movement), then command movement
        startPosition = transform.position;
        destination = target.position;
        awaitingPathOnly = true;

        pathTimer.Restart();
        agent.CalculatePathOnly(destination);
    }

    private void HandlePathComplete(PathfindingResult result)
    {
        if (awaitingPathOnly)
        {
            
            if (result.success)
            {
                pathTimer.Stop();
                lastPathTimeMs = pathTimer.ElapsedMilliseconds;

                
                UnityEngine.Debug.Log($"Theta* Pathfinding Time: {lastPathTimeMs:F2} ");

                // Path quality metrics
                lastPathLength = 0f;
                lastTurnCount = 0;
                lastPathNodeCount = result.Path != null ? result.Path.Count : 0;

                for (int i = 1; i < result.Path.Count; i++)
                {
                    lastPathLength += Vector3.Distance(result.Path[i - 1], result.Path[i]);

                    if (i >= 2)
                    {
                        Vector3 dir1 = (result.Path[i - 1] - result.Path[i - 2]).normalized;
                        Vector3 dir2 = (result.Path[i] - result.Path[i - 1]).normalized;
                        if (Vector3.Angle(dir1, dir2) > 1f) lastTurnCount++;
                    }
                }

                //UnityEngine.Debug.Log($"Theta* Pathfinding Time (calc only): {lastPathTimeMs} ms");
                //UnityEngine.Debug.Log($"Path Length: {lastPathLength:F2}, Turns: {lastTurnCount}");

                // Now measure movement
                awaitingPathOnly = false;
                moveTimer.Restart();
                agent.MoveTo(destination);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Theta* Pathfinding failed: {result.ErrorMessage}");

                // Log CSV row for failure (movement time/avg speed = 0)
                AppendCsvRow(
                    success: false,
                    pathTimeMs: lastPathTimeMs,
                    moveTimeMs: 0.0,
                    avgSpeed: 0.0,
                    pathLength: 0.0f,
                    turnCount: 0,
                    nodeCount: 0,
                    start: startPosition,
                    end: destination
                );

                Cleanup();
            }
        }
        else
        {
            // Ignore subsequent OnPathComplete calls from MoveTo()
        }
    }

    private void HandleMovementStart()
    {
        if (awaitingPathOnly) return;
        if (!moveTimer.IsRunning) moveTimer.Start();
    }

    private void HandleMovementComplete()
    {
        moveTimer.Stop();

        double moveMs = moveTimer.Elapsed.TotalSeconds;
        double moveSec = moveTimer.Elapsed.TotalSeconds;
        double avgSpeed = moveSec > 0 ? lastPathLength / moveSec : 0.0;

        UnityEngine.Debug.Log($"Movement Time: {moveMs:F2} ms");
        UnityEngine.Debug.Log($"Average Speed: {avgSpeed:F3} units/s");

        // Log success row
        AppendCsvRow(
            success: true,
            pathTimeMs: lastPathTimeMs,
            moveTimeMs: moveMs,
            avgSpeed: avgSpeed,
            pathLength: lastPathLength,
            turnCount: lastTurnCount,
            nodeCount: lastPathNodeCount,
            start: startPosition,
            end: destination
        );

        Cleanup();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        if (agent != null)
        {
            agent.OnPathComplete -= HandlePathComplete;
            agent.OnMovementStart -= HandleMovementStart;
            agent.OnMovementComplete -= HandleMovementComplete;
        }

        if (pathTimer != null && pathTimer.IsRunning) pathTimer.Stop();
        if (moveTimer != null && moveTimer.IsRunning) moveTimer.Stop();
    }

    private void PrepareCsv()
    {
        csvPath = Path.Combine(Application.persistentDataPath, "theta_metrics.csv");

        try
        {
            if (!File.Exists(csvPath))
            {
                using (var sw = new StreamWriter(csvPath, false))
                {
                    sw.WriteLine("timestamp,scene,agentName,success,startX,startY,startZ,endX,endY,endZ,pathNodes,pathLength,turns,pathTimeMs,moveTimeMs,avgSpeed");
                }
            }
        }
        catch (IOException ex)
        {
            UnityEngine.Debug.LogError($"Failed to prepare CSV file at '{csvPath}': {ex.Message}");
        }
    }



    private void AppendCsvRow(
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
        string timestamp = System.DateTime.Now.ToString("o", ci); // ISO 8601

        try
        {
            using (var sw = new StreamWriter(csvPath, true))
            {
                sw.WriteLine(string.Join(",", new string[]
                {
                    timestamp,
                    EscapeCsv(scene),
                    EscapeCsv(gameObject.name),
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
                }));
            }
        }
        catch (IOException ex)
        {
            UnityEngine.Debug.LogError($"Failed to append CSV row to '{csvPath}': {ex.Message}");
        }
    }

    private static string EscapeCsv(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        bool mustQuote = input.Contains(",") || input.Contains("\"") || input.Contains("\n") || input.Contains("\r");
        if (!mustQuote) return input;
        return "\"" + input.Replace("\"", "\"\"") + "\"";
    }
}