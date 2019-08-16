using System.Collections.Generic;

namespace tests {
  public class Resource {
    public enum Operations {
      Create = '+',
      Modify = '~',
      Remove = '-'
    }

    public Operations Operation { get; set; }
    public string Categorey { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public IDictionary<string, string> Properties { get; set; }

    public Resource (char op, string category, string type, string name) {
      Operation = (Operations) op;
      Categorey = category;
      Type = type;
      Name = name;
    }

    public Resource (Resource copy, IDictionary<string,string> props) {
      Operation = copy.Operation;
      Categorey = copy.Categorey;
      Type = copy.Type;
      Name = copy.Name;
      Properties = props;
    }
  }
}