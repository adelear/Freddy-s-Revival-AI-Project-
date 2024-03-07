using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public enum TaskType
    {
        FindStunGun,
        CollectCupcake,
        ImmobilizeFreddy
    }
    public struct Task
    {
        public TaskType type;
        public string description;
        public bool isCompleted;

        public Task(TaskType type, string description)
        {
            this.type = type;
            this.description = description;
            this.isCompleted = false;
        }
    }

    private Task[] tasks = new Task[3];

    [SerializeField] private TMP_Text[] taskTexts;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        tasks[0] = new Task(TaskType.FindStunGun, "Find Stun Gun");
        tasks[1] = new Task(TaskType.CollectCupcake, "Collect Cupcake");
        tasks[2] = new Task(TaskType.ImmobilizeFreddy, "Immobilize Freddy");

        UpdateTaskUI();
    }

    public void CompleteTask(TaskType taskType)
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].type == taskType)
            {
                tasks[i].isCompleted = true;
                break;
            }
        }
        UpdateTaskUI();
    }

    private void UpdateTaskUI()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].isCompleted)
                taskTexts[i].text = "<s>" + tasks[i].description + "</s>";
            else
                taskTexts[i].text = tasks[i].description;
        }
    }
}
