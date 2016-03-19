# Pipeline middlewares for ASP.NET Core

[![Build status](https://ci.appveyor.com/api/projects/status/uxjx64j4o1xhle8t?svg=true)](https://ci.appveyor.com/project/mindfor/aspnetcore-pipeline)

Repo contains middlewares for:

- Basic authentication;
- Require only HTTP or HTTPS requests.

## Installation

*Dev* branch is build for *ASP.NET Core 1.0.0-RC2* (https://www.myget.org/gallery/aspnetvnext). We have published NuGet package to the custom [Mindfor MyGet source](https://www.myget.org/F/mindfor/api/v3/index.json) because ASP.NET Core RC2 is not yet ready. So you should add this source to the project `NuGet.config`:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<packageSources>
		<clear/>
		<add key="AspNetVNext" value="https://www.myget.org/F/aspnetvnext/api/v3/index.json" />
		<add key="Mindfor" value="https://www.myget.org/F/mindfor/api/v3/index.json" />
		<add key="NuGet" value="https://api.nuget.org/v3/index.json" />
	</packageSources>
</configuration>
```

The just install NuGet package:
```
Install-Package Mindfor.AspNetCore.Pipeline -Pre
```

We will publish package to the [NuGet.org](http://nuget.org) when there will be final RC2. 

## How to use

### Basic authentication

Basic authentication requires [ASP.NET Core Identity](https://github.com/aspnet/identity). Add Identity services and basic authentication to the pipeline in `Startup` class:

```C#
public void ConfigureServices(IServiceCollection services)
{
	services.AddIdentity<MyUserType, MyRoleType>();
}

public void Configure(IApplicationBuilder app)
{
	app.UseBasicAuthentication<MyUserType>();
}
```

### Require only HTTP/HTTPS requests

Add one of middlewares to the pipeline in `Startup` class to redirect all requests to HTTP or HTTPS:
```C#
public void Configure(IApplicationBuilder app)
{
	app.UseRequireHttps(); // redirect all requests to HTTPS
	app.UseRequireHttp(); // or redirect all requests to HTTP
	// register other middlewares
}
```

## Licence

Code by Alexander Fomin. Copyright 2016 Mindfor Ltd.

This package has MIT license. Refer to the [LICENSE](blob/dev/LICENSE) for detailed information.