using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
    public int Priority { get; set; }
    public string Platform { get; set; }
    public string Assignee { get; set; }
    public string Status { get; set; }
    public List<string> Attachments { get; set; }
    public List<string> Comments { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan TotalTimeSpent { get; set; }
    public List<string> TestCases { get; set; }

    public Task()
    {
        Attachments = new List<string>();
        Comments = new List<string>();
        TestCases = new List<string>();
    }
}

public class TaskManager
{
    private List<Task> tasks = new List<Task>();
    private int nextTaskId = 1;

    public void AddTask(string description, DateTime dueDate, int priority, string platform, string assignee, string status)
    {
        Task task = new Task
        {
            Id = nextTaskId++,
            Description = description,
            IsCompleted = false,
            DueDate = dueDate,
            Priority = priority,
            Platform = platform,
            Assignee = assignee,
            Status = status,
            StartTime = DateTime.Now
        };
        tasks.Add(task);
        Console.WriteLine($"Task '{description}' for platform '{platform}' added.");
    }

    public void MarkTaskAsCompleted(int taskId)
    {
        Task task = tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.IsCompleted = true;
            task.EndTime = DateTime.Now;
            task.TotalTimeSpent = task.EndTime - task.StartTime;
            Console.WriteLine($"Task '{task.Description}' marked as completed.");
        }
        else
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
    }

    public void ViewAllTasks()
    {
        Console.WriteLine("All Tasks:");
        foreach (Task task in tasks)
        {
            Console.WriteLine($"Task ID: {task.Id}, Description: {task.Description}, Due Date: {task.DueDate}, Priority: {task.Priority}, Platform: {task.Platform}, Assignee: {task.Assignee}, Status: {task.Status}, Completed: {(task.IsCompleted ? "Yes" : "No")}, Time Spent: {task.TotalTimeSpent}");
        }
    }

    public void ViewIncompleteTasks()
    {
        Console.WriteLine("Incomplete Tasks:");
        foreach (Task task in tasks.Where(t => !t.IsCompleted))
        {
            Console.WriteLine($"Task ID: {task.Id}, Description: {task.Description}, Due Date: {task.DueDate}, Priority: {task.Priority}, Platform: {task.Platform}, Assignee: {task.Assignee}, Status: {task.Status}");
        }
    }

    public void SaveTasksToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Task task in tasks)
            {
                writer.WriteLine($"{task.Id},{task.Description},{task.IsCompleted},{task.DueDate},{task.Priority},{task.Platform},{task.Assignee},{task.Status},{task.StartTime},{task.EndTime},{task.TotalTimeSpent}");
                writer.WriteLine(string.Join(";", task.Attachments));
                writer.WriteLine(string.Join(";", task.Comments));
                writer.WriteLine(string.Join(";", task.TestCases));
            }
        }
        Console.WriteLine($"Tasks saved to file: {filePath}");
    }

    public void LoadTasksFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            tasks.Clear();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    Task task = new Task
                    {
                        Id = int.Parse(parts[0]),
                        Description = parts[1],
                        IsCompleted = bool.Parse(parts[2]),
                        DueDate = DateTime.Parse(parts[3]),
                        Priority = int.Parse(parts[4]),
                        Platform = parts[5],
                        Assignee = parts[6],
                        Status = parts[7],
                        StartTime = DateTime.Parse(parts[8]),
                        EndTime = DateTime.Parse(parts[9]),
                        TotalTimeSpent = TimeSpan.Parse(parts[10])
                    };
                    task.Attachments.AddRange(reader.ReadLine().Split(';'));
                    task.Comments.AddRange(reader.ReadLine().Split(';'));
                    task.TestCases.AddRange(reader.ReadLine().Split(';'));
                    tasks.Add(task);
                }
            }
            Console.WriteLine($"Tasks loaded from file: {filePath}");
        }
        else
        {
            Console.WriteLine($"File not found: {filePath}");
        }
    }

    public void AddAttachmentToTask(int taskId, string attachment)
    {
        Task task = tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.Attachments.Add(attachment);
            Console.WriteLine($"Attachment added to Task ID {taskId}.");
        }
        else
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
    }

    public void AddCommentToTask(int taskId, string comment)
    {
        Task task = tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.Comments.Add(comment);
            Console.WriteLine($"Comment added to Task ID {taskId}.");
        }
        else
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
    }

    public void AddTestCaseToTask(int taskId, string testCase)
    {
        Task task = tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.TestCases.Add(testCase);
            Console.WriteLine($"Test case added to Task ID {taskId}.");
        }
        else
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Mark Task as Completed");
            Console.WriteLine("3. View All Tasks");
            Console.WriteLine("4. View Incomplete Tasks");
            Console.WriteLine("5. Save Tasks to File");
            Console.WriteLine("6. Load Tasks from File");
            Console.WriteLine("7. Add Attachment to Task");
            Console.WriteLine("8. Add Comment to Task");
            Console.WriteLine("9. Add Test Case to Task");
            Console.WriteLine("10. Exit");

            int option;
            if (int.TryParse(Console.ReadLine(), out option))
            {
                switch (option)
                {
                    case 1:
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        Console.Write("Enter task due date (YYYY-MM-DD): ");
                        DateTime dueDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter task priority (1 = low, 2 = medium, 3 = high): ");
                        int priority = int.Parse(Console.ReadLine());
                        Console.Write("Enter platform (e.g., Windows, Linux, macOS): ");
                        string platform = Console.ReadLine();
                        Console.Write("Enter assignee: ");
                        string assignee = Console.ReadLine();
                        Console.Write("Enter status: ");
                        string status = Console.ReadLine();
                        taskManager.AddTask(description, dueDate, priority, platform, assignee, status);
                        break;
                    case 2:
                        Console.Write("Enter task ID to mark as completed: ");
                        int taskId;
                        if (int.TryParse(Console.ReadLine(), out taskId))
                        {
                            taskManager.MarkTaskAsCompleted(taskId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid task ID.");
                        }
                        break;
                    case 3:
                        taskManager.ViewAllTasks();
                        break;
                    case 4:
                        taskManager.ViewIncompleteTasks();
                        break;
                    case 5:
                        Console.Write("Enter file path to save tasks: ");
                        string saveFilePath = Console.ReadLine();
                        taskManager.SaveTasksToFile(saveFilePath);
                        break;
                    case 6:
                        Console.Write("Enter file path to load tasks: ");
                        string loadFilePath = Console.ReadLine();
                        taskManager.LoadTasksFromFile(loadFilePath);
                        break;
                    case 7:
                        Console.Write("Enter task ID to add attachment: ");
                        int attachTaskId = int.Parse(Console.ReadLine());
                        Console.Write("Enter attachment file path: ");
                        string attachment = Console.ReadLine();
                        taskManager.AddAttachmentToTask(attachTaskId, attachment);
                        break;
                    case 8:
                        Console.Write("Enter task ID to add comment: ");
                        int commentTaskId = int.Parse(Console.ReadLine());
                        Console.Write("Enter comment: ");
                        string comment = Console.ReadLine();
                        taskManager.AddCommentToTask(commentTaskId, comment);
                        break;
                    case 9:
                        Console.Write("Enter task ID to add test case: ");
                        int testCaseTaskId = int.Parse(Console.ReadLine());
                        Console.Write("Enter test case: ");
                        string testCase = Console.ReadLine();
                        taskManager.AddTestCaseToTask(testCaseTaskId, testCase);
                        break;
                    case 10:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }
}
