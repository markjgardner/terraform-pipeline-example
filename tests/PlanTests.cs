using System;
using System.IO;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace tests {
  public class PlanTests {
    private static string planPath = "plan.json";
    private dynamic plan;
    private IList<dynamic> resources;
    public PlanTests () {
      var rawPlan = File.ReadAllText (Path.Combine (Environment.CurrentDirectory, planPath));
      plan = JObject.Parse(rawPlan);
      resources = new List<dynamic>(plan.resource_changes);
    }

    [Fact]
    public void Test0()
    {
      Assert.Equal(4, resources.Count());
    }

    [Fact]
    public void Test1 () {
      var r = resources.Single(p => p.name=="vnet");
      Assert.Equal("eastus2", r.change.after.location.Value);
    }

    [Fact]
    public void Test2()
    {
      var r = resources.Single(p => p.name == "gw-sn");
      Assert.Equal("10.0.0.0/27", r.change.after.address_prefixes[0].Value);
    }

    [Fact]
    public void Test3()
    {
      var r = resources.Single(p => p.name == "app-sn");
      Assert.Equal("10.0.0.32/27", r.change.after.address_prefixes[0].Value);
    }
  }
}