﻿namespace Library.Core.Models;

public class Book
{
    /// <summary>
    /// ID of book
    /// </summary>
    public Guid Id { get; set; }
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
    /// Date when book was taken
    /// </summary>
    public DateTime DateTaken { get; set; }
    /// <summary>
    /// Author of book
    /// </summary>
    public Author Author { get; set; }
}