using System;
using System.IO;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace tests {
  public class PlanTests {
    private static string planPath = "plan.txtplan";
    private IEnumerable<Resource> resources;
    public PlanTests () {
      var rawPlan = File.ReadAllText (Path.Combine (Environment.CurrentDirectory, planPath));
      var p = new PlanParser(rawPlan);
      resources = p.Convert();
    }

    [Fact]
    public void Test0 () {
      Assert.Equal(4, resources.Count());
    }

    [Fact]
    public void Test1 () {
      var r = resources.Single(p => p.Name=="rg");
      Assert.Equal("eastus2", r.Properties["location"]);
    }

    [Fact]
    public void Test2 () {
      var r = resources.Single(p => p.Name=="app-sn");
      Assert.Equal("10.0.0.32/27", r.Properties["address_prefix"]);
    }

    [Fact]
    public void Test3 () {
      var r = resources.Single(p => p.Name=="vnet");
      Assert.Equal("10.0.0.0/20", r.Properties["address_space"]);
    }
  }
}