﻿namespace Library.Application.DTO;

public class BookDto : BaseDto
{
    /// <summary>
    /// ISBN of book
    /// </summary>
    public string ISBN { get; set; }
    
    /// <summary>
    /// Name of book
    /// </summary>
    public string BookName { get; set; }
    
    /// <summary>
    /// Genre of book
    /// </summary>
    public string Genre { get; set; }
    
    /// <summary>
    /// Description of book
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Is book available
    /// </summary>
    public bool IsAvailable { get; set; }
    
    /// <summary>
    /// Author of book
    /// </summary>
    public Guid AuthorId { get; set; }
}