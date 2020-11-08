
namespace TodoMysqlApi.DTOs
{

  public class TodoItemDTO
  {
    public long Id { get; set; }
    public string Entry { get; set; }
    public bool IsComplete { get; set; }
  }

}