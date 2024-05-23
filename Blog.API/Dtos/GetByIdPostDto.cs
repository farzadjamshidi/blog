using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos;

public class GetByIdPostDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}