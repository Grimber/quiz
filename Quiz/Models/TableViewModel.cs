using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class TableViewModel
    {
        public int IdForName { get; set; }
        public string Name { get; set; }
        public List<DataRow> DataRows { get; set; }
        public List<string> Headline { get; set; }

        public TableViewModel()
        {
            DataRows = new List<DataRow>();
        }

        public TableViewModel(string name)
        {
            Name = name;
            DataRows = new List<DataRow>();
        }

        public TableViewModel(string name, int idForName)
        {
            IdForName = idForName;
            Name = name;
            DataRows = new List<DataRow>();
        }

        public TableViewModel(string name, int idForName, List<string> headline)
        {
            IdForName = idForName;
            Name = name;
            Headline = headline;
            DataRows = new List<DataRow>();
        }
    }

    public class DataRow
    {
        public int Id { get; set; }
        public List<string> Row { get; set; }

        public DataRow()
        {
            Row = new List<string>();
        }
    }
}