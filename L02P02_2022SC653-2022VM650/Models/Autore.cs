using System;
using System.Collections.Generic;

namespace L02P02_2022SC653_2022VM650.Models;

public partial class Autore
{
    public int Id { get; set; }

    public string? Autor { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
