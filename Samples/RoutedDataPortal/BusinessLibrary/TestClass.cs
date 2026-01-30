using Csla;

namespace BusinessLibrary
{
  [CslaImplementProperties]
  public partial class TestClass : ReadOnlyBase<TestClass>
  {
    public partial string? CreatedFrom { get; private set; }

    [Fetch]
    private void Fetch()
    {
      var instanceId = Environment.GetEnvironmentVariable("INSTANCE_ID") ??
                       Environment.GetEnvironmentVariable("HOSTNAME") ??
                       Environment.GetEnvironmentVariable("COMPUTERNAME") ??
                       "unknown-instance";
      CreatedFrom = $"{instanceId} - {ApplicationContext.LocalContext["dpv"]?.ToString()}";
    }
  }
}
