using PostBindOrchestrator.Core;

namespace PostBindOrchestrator.DomainLayer;

internal static class NextOrchestrationTaskEnumProvider
{
    public static readonly string[] orchestrationTasks = Enum.GetValues<OrchestrationTask>().Select(ot => ot.ToString()).ToArray();

    public static OrchestrationTask GetNext(OrchestrationTask orchestrationTask)
    {
        var currentIndex = Array.IndexOf(orchestrationTasks, orchestrationTask.ToString());

        return currentIndex == -1 || currentIndex == orchestrationTasks.Length - 1
            ? OrchestrationTask.None
            : (OrchestrationTask)(currentIndex + 1);
    }
}
