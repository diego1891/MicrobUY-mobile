using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobUY.Models.Backend.Posts;

public class PostResponse
{
    public string Id { get; set; }
    public string Contenido { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int Likes { get; set; }
    public string Hashtag { get; set; }
}
