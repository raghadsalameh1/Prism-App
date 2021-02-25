using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism_App.Models
{
    public class FieldItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FieldName { get; set; }

        public string GeoJson { get; set; }
    }
}
