using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace tests {
  public class PlanParser {
    private string _raw;
    private const string _bookmark = "Terraform will perform the following actions:";
    public PlanParser (string text) {
      //remove all of the preamble
      var index = text.IndexOf (_bookmark);
      _raw = text.Substring (index + _bookmark.Length);
    }

    public IEnumerable<Resource> Convert () {
      return plan.Parse(_raw);
    }

    private static CommentParser comment = new CommentParser ("#", null, null, "\n");

    private static Parser<string> quotedString =
        from open in Parse.Char ('"')
        from text in Parse.AnyChar.Until(Parse.Char('"'))
        select string.Concat(text);

    private static Parser<Resource> resourceHeader =
      from op in Parse.Chars ('+', '-', '~')
      from s1 in Parse.WhiteSpace.Once ()
      from category in Parse.Letter.Many ().Text ()
      from s2 in Parse.WhiteSpace.Once ()
      from type in quotedString
      from s3 in Parse.WhiteSpace.Once ()
      from name in quotedString
      select new Resource (op, category, type, name);

    private static Parser<KeyValuePair<string, string>> property =
      from w0 in Parse.WhiteSpace.Many()
      from op in Parse.Chars ('+', '-', '~')
      from w1 in Parse.WhiteSpace
      from key in Parse.AnyChar.Until(Parse.Char('=')).Text()
      from w2 in Parse.WhiteSpace
      from open in Parse.Char('"').XOr(Parse.Char('(')).XOr(Parse.Char('['))
      from value in Parse.AnyChar.Until(Parse.Char('"').XOr(Parse.Char(')')).XOr(Parse.Char(']'))).Text()
      select new KeyValuePair<string, string>(key.Trim(), value.Trim());

    private static Parser<Dictionary<string, string>> resourceProperties =
      from w0 in Parse.WhiteSpace.Many()
      from open in Parse.Char('{')
      from props in Parse.Ref(() => property).Many()
      from w1 in Parse.WhiteSpace.Many()
      from close in Parse.Char('}')
      from nl in Parse.LineTerminator
      select new Dictionary<string, string>(props);

    private static Parser<Resource> resourceBlock =
      from x1 in Parse.WhiteSpace.Many()
      from c in comment.SingleLineComment
      from x2 in Parse.WhiteSpace.Many()
      from h in resourceHeader
      from p in resourceProperties
      select new Resource(h,p);

    private static Parser<IEnumerable<Resource>> plan = 
      from blocks in Parse.Ref(()=>resourceBlock).Many()
      select blocks;

  }
}