﻿using System.Collections.Generic;

namespace DataManager.Models
{
    public class Folder
    {
        public Folder()
        {
            Contents = new List<Folder>();
        }

        public string Title { get; set; }
        public List<Folder> Contents { get; set; }
    }
}
