//-----------------------------------------------------------------------
// <copyright file="ApplicationContextManager.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Provides consistent context information between the client</summary>
//-----------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using Csla.Configuration;

namespace Csla.Windows
{
  /// <summary>
  /// ApplicationContextManager for Windows Forms applications
  /// </summary>
  /// <param name="securityOptions"></param>
  public class ApplicationContextManager(SecurityOptions securityOptions) : Csla.Core.ApplicationContextManager
  {
    private static IPrincipal _principal = default!;
    private SecurityOptions _securityOptions = securityOptions;

    /// <summary>
    /// Gets the current principal.
    /// </summary>
    public override IPrincipal GetUser()
    {
      if (_principal == null)
      {
        if (_securityOptions.FlowSecurityPrincipalFromClient)
          SetUser(new System.Security.Claims.ClaimsPrincipal());
        else
        {
          // Use platform-compatible identity for cloud environments (Linux)
          if (OperatingSystem.IsWindows())
          {
#pragma warning disable CA1416 // Validate platform compatibility
            SetUser(new WindowsPrincipal(WindowsIdentity.GetCurrent()));
#pragma warning restore CA1416 // Validate platform compatibility
          }
          else
          {
            // For Linux/cloud environments, use ClaimsPrincipal with environment-based identity
            var identity = new System.Security.Claims.ClaimsIdentity("CloudPrincipal");
            var userName = Environment.GetEnvironmentVariable("APP_USER") ?? Environment.UserName;
            identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, userName));
            SetUser(new System.Security.Claims.ClaimsPrincipal(identity));
          }
        }
      }
      return _principal;
    }

    /// <inheritdoc />
    [MemberNotNull(nameof(_principal))]
    public override void SetUser(IPrincipal principal)
    {
      _principal = principal ?? throw new ArgumentNullException(nameof(principal));
      Thread.CurrentPrincipal = principal;
    }
  }
}