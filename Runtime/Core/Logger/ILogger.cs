using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILogger
{
    void LogError(string fmt, params object[] args);
    void LogWarning(string fmt, params object[] args);
    void LogInfo(string fmt, params object[] args);
    void LogDebug(string fmt, params object[] args);
}
