﻿namespace CRUD_Class.Models
{
    public class Tag:BaseEntity
    {
        public string Name {  get; set; }
        public List<BookTag>? BookTags { get; set; }
    }
}
