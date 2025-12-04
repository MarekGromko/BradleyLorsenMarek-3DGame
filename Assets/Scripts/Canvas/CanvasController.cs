using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Camera mainCamera;
    public RectTransform innerHealthBar;
    public TextMeshProUGUI timerCounter;
    public TextMeshProUGUI taskCounter;
    public RectTransform taskBubble;

    void Update()
    {
        UpdateHealthBar();
        UpdateTime();
        UpdateTask();
    }
    void UpdateHealthBar()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            HealthState healthState = player.GetComponent<HealthState>();
            if(healthState != null && innerHealthBar != null)
            {
                float healthPercent = healthState.CurrentHealth / healthState.MaxHealth;
                innerHealthBar.localScale = new Vector3(healthPercent, 1f, 1f);
            }
        }
    }
    void UpdateTime()
    {
        if(timerCounter != null)
        {
            float time = TimeSystem.Instance.RemainingTime;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f) + 15 / 15 * 15;
            timerCounter.text = $"{minutes:00}:{seconds:00}";
        }
    }
    void UpdateTask()
    {
        taskCounter.text = $"Tasks: {TasksSystem.Instance.CompletedTasks}/{TasksSystem.Instance.TotalTasks}";
        var task = TasksSystem.Instance.ActiveTask;
        if(task == null)
        {
            taskBubble.gameObject.SetActive(false);
            return;
        }
        var screenPoint = mainCamera.WorldToScreenPoint(task.transform.position);
        if(screenPoint.z < 0)
        {
            taskBubble.gameObject.SetActive(false);
            return;
        }
        taskBubble.gameObject.SetActive(true);
        taskBubble.transform.position = new Vector3(screenPoint.x, screenPoint.y, 0f);
    }
}