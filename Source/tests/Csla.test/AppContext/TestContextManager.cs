using System.Collections.Specialized;
using System.Security.Principal;
using Csla.Core;

namespace Csla.Test.AppContext
{
  public class TestContextManager : IContextManager
  {
    private readonly AsyncLocal<IContextDictionary> _myContext = new();
    private readonly AsyncLocal<IPrincipal> _principal = new();

    private const string _localContextName = "Csla.ClientContext";
    private const string _clientContextName = "Csla.ClientContext";
    private const string _globalContextName = "Csla.GlobalContext";

    public bool IsValid
    {
      get { return true; }
    }

    public bool IsStatefulContext => true;

    public ApplicationContext ApplicationContext { get; set; }

    public IPrincipal GetUser()
    {
      IPrincipal result = _principal.Value;
      if (result == null)
      {
        result = new System.Security.Claims.ClaimsPrincipal();
        SetUser(result);
      }
      return result;
    }

    public void SetUser(IPrincipal principal)
    {
      _principal.Value = principal;
    }

    public IContextDictionary GetLocalContext()
    {
      if (_myContext.Value == null)
        _myContext.Value = new ContextDictionary();
      if (_myContext.Value[_localContextName] == null)
        SetLocalContext(new ContextDictionary());
      return (IContextDictionary)_myContext.Value[_localContextName];
    }

    public void SetLocalContext(IContextDictionary localContext)
    {
      if (_myContext.Value == null)
        _myContext.Value = new ContextDictionary();
      _myContext.Value[_localContextName] = localContext;
    }

    public IContextDictionary GetClientContext(ApplicationContext.ExecutionLocations executionLocation)
    {
      if (_myContext.Value == null)
        _myContext.Value = new ContextDictionary();
      if (_myContext.Value[_clientContextName] == null)
        SetClientContext(new ContextDictionary(), executionLocation);
      return (IContextDictionary) _myContext.Value[_clientContextName];
    }

    public void SetClientContext(IContextDictionary clientContext, ApplicationContext.ExecutionLocations executionLocation)
    {
      if (_myContext.Value == null)
        _myContext.Value = new ContextDictionary();
      _myContext.Value[_clientContextName] = clientContext;
    }

    public IContextDictionary GetGlobalContext()
    {
      if (_myContext.Value == null)
        _myContext.Value = new ContextDictionary();
      if (_myContext.Value[_globalContextName] == null)
        SetGlobalContext(new ContextDictionary());
      return (ContextDictionary)_myContext.Value[_globalContextName];
    }

    public void SetGlobalContext(IContextDictionary globalContext)
    {
      if (_myContext.Value == null)
        _myContext.Value = new ContextDictionary();
      _myContext.Value[_globalContextName] = globalContext;
    }

    private static IServiceProvider _provider;

    /// <summary>
    /// Gets the default IServiceProvider
    /// </summary>
    public IServiceProvider GetDefaultServiceProvider()
    {
      return _provider;
    }

    /// <summary>
    /// Sets the default IServiceProvider
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider instance</param>
    public void SetDefaultServiceProvider(IServiceProvider serviceProvider)
    {
      _provider = serviceProvider;
    }

    /// <summary>
    /// Gets the service provider for current scope
    /// </summary>
    public IServiceProvider GetServiceProvider()
    {
      return (IServiceProvider)ApplicationContext.LocalContext["__sps"];
    }

    /// <summary>
    /// Sets the service provider for current scope
    /// </summary>
    /// <param name="scope">IServiceProvider instance</param>
    public void SetServiceProvider(IServiceProvider scope)
    {
      ApplicationContext.LocalContext["__sps"] = scope;
    }
  }
}
