using System.ComponentModel.DataAnnotations;

namespace Blog.API.Dtos;

public class CreatePostDto
{
    public string Content { get; set; }
}