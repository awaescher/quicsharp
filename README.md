# quic#
A very simple scripting app using an abstracted C# scripting engine powered by either ...
 - the [Roslyn Scripting API](https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples)
   - .NET version: `4.6`
   - dependencies: `Microsoft.CodeAnalysis.CSharp.Scripting`
   - branch: `Roslyn-Powered-Engine-NET46`
 - ScriptCs
   - .NET version: `4.5`
   - dependencies: `ScriptCs.Hosting`, `ScriptCs.Engine.Roslyn`
   - branch: `ScriptCs-Powered-Engine-NET45`
 - a self-written CodeDom scripting engine
   - .NET version: `3.5`
   - dependencies: `none`
   - branch: `master`/`CodeDom-Powered-Engine-NET35`

![Screenshot](_img/Screenshot.png)
