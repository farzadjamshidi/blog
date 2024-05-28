using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos;

public class CreateCommentDto
{
    public string Text { get; set; }
    public int PostId { get; set; }
}