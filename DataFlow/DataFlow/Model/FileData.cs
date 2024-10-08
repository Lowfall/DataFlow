﻿using System.ComponentModel.DataAnnotations;

namespace DataFlow.Model
{
    public class FileData
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string EnglishString { get; set; }
        public string RussianString { get; set; }
        public int GeneratedInteger { get; set; }
        public float GeneratedFloat { get; set; }
    }
}
