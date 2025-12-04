using System.Collections.Generic;
using UnityEngine;

public class TasksSystem : MonoBehaviour
{
    public static TasksSystem Instance { get; private set; }
    [SerializeField] private List<InteractTarget> _tasks;
    public int TotalTasks { get; private set; }
    public int CompletedTasks { get { return TotalTasks - _tasks.Count; } }
    public InteractTarget ActiveTask {get {return _tasks.Count > 0 ? _tasks[0] : null;}}
    void Awake()
    {
        Instance = this;
        TotalTasks = _tasks.Count;
        foreach(var task in _tasks)
        {
            task.OnActive.AddListener(OnTaskCompleted);
        }
    }

    private void OnTaskCompleted(InteractTarget task)
    {
        task.gameObject.SetActive(false);
        _tasks.Remove(task);
    }
}