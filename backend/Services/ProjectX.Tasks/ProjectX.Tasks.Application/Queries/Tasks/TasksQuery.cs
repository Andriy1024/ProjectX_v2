using ProjectX.Core.CQRS;

namespace ProjectX.Tasks.Application.Queries.Tasks;

public class TasksQuery : IQuery<TasksQuery.Result>
{
    public class Result 
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}