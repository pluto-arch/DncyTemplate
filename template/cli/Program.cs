
using System.Text;
using Sharprompt;


var versionString = Assembly.GetEntryAssembly()?
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
    .InformationalVersion
    .ToString();

Console.WriteLine($"dotnetydd tool v{versionString}");
Console.WriteLine("================================");
ShowBot();
Console.WriteLine("================================");
if (args.Length<=0)
{
    Console.WriteLine("请使用 `dotnetydd --init` 进行初始化项目");
    return;
}
if (args[0] != "--init")
{
    Console.WriteLine("请使用 `dotnetydd --init` 进行初始化项目");
    return;
}

var name = Prompt.Input<string>("项目名称");

var hasTenant = Prompt.Confirm(new ConfirmOptions
{
    Message = "是否包含租户",
    DefaultValue = true
});

var ui = Prompt.MultiSelect("用户界面", new[] { "API", "MVC", "BlazorServer" });


var hasAspire = Prompt.Confirm(new ConfirmOptions
{
    Message = "是否启用Aspire",
    DefaultValue = true
});


var dir = Directory.GetCurrentDirectory();


#if DEBUG
Console.WriteLine($"名称 {name}!");
Console.WriteLine($"租户： {hasTenant}");
Console.WriteLine($"用户界面： {string.Join(",",ui)}");
Console.WriteLine($"目录： {dir}");
Console.WriteLine($"Aspire： {hasAspire}");
#endif

#if DEBUG
dir+="\\TestCliCom";
#endif


if (!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
}

Process process = new Process();
process.StartInfo.FileName = "dotnet";
process.StartInfo.Arguments = "new list --columns author";
process.StartInfo.UseShellExecute = false;
process.StartInfo.RedirectStandardOutput = true;
process.Start();
string output = process.StandardOutput.ReadToEnd();

string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

string searchString = "boltapp";
if (lines.All(x=>!x.Contains(searchString)))
{
    Console.WriteLine("未安装项目模板");
    return;
}

process.StartInfo.FileName = "dotnet";
process.StartInfo.Arguments = $"new boltapp -n {name} -T {hasTenant}  -A {hasAspire} -o {dir}";
process.StartInfo.UseShellExecute = false;
process.StartInfo.RedirectStandardOutput = true;
process.Start();
output = process.StandardOutput.ReadToEnd();


process.StartInfo.Arguments = "";

var slnPath = Path.Combine(dir, $"{name}.sln");

var apiPath = Path.Combine(dir, "src",$"{name}.Api",$"{name}.Api.csproj");
var mvcPath = Path.Combine(dir, "src",$"{name}.Mvc",$"{name}.Mvc.csproj");
var blazorServerPath = Path.Combine(dir, "src",$"{name}.BlazorServer",$"{name}.BlazorServer.csproj");


var aspirePath1=Path.Combine(dir, "aspire",$"{name}.AppHost",$"{name}.AppHost.csproj");
var aspirePath2=Path.Combine(dir, "aspire",$"{name}.ServiceDefaults",$"{name}.ServiceDefaults.csproj");




var aspireProjectBuilder = new StringBuilder();

StringBuilder sb = new StringBuilder();
var uiselect = ui.Select(x => x.ToLower());
if (uiselect.Contains("api"))
{
    sb.Append($" {apiPath} ");
    aspireProjectBuilder.AppendLine($"builder.AddProject<Projects.{name}_Api>(\"{name}.api\");");
}
else
{
    if (hasAspire)
    {
        process.StartInfo.Arguments = $" remove {aspirePath1} reference {apiPath}";
        process.Start();
    }
}

if (uiselect.Contains("mvc"))
{
    sb.Append($" {mvcPath} ");
    aspireProjectBuilder.AppendLine($"builder.AddProject<Projects.{name}_Mvc>(\"{name}.mvc\");");
}
else
{
    if (hasAspire)
    {
        process.StartInfo.Arguments = $" remove {aspirePath1} reference {mvcPath}";
        process.Start();
    }
}
if (uiselect.Contains("blazorserver"))
{
    sb.Append($" {blazorServerPath} ");
    aspireProjectBuilder.AppendLine($"builder.AddProject<Projects.{name}_BlazorServer>(\"{name}.blazorserver\");");
}
else
{
    if (hasAspire)
    {
        process.StartInfo.Arguments = $" remove {aspirePath1} reference {blazorServerPath}";
        process.Start();
    }
}
if (hasAspire)
{
    sb.Append($" {aspirePath1} {aspirePath2} ");


    var fileContent = $"""
                       var builder = DistributedApplication.CreateBuilder(args);
                       
                       {aspireProjectBuilder.ToString()}
                       builder.Build().Run();
                       """;

    var aspireProgramFile = Path.Combine(dir, "aspire",$"{name}.AppHost", "Program.cs");

    if (File.Exists(aspireProgramFile))
    {
        await File.WriteAllTextAsync(aspireProgramFile, fileContent,Encoding.Default);
    }
}


var uiargs = sb.ToString();

// sln project reference
process.StartInfo.Arguments = $"sln {slnPath} add {uiargs}";
process.Start();
output = process.StandardOutput.ReadToEnd();


if (!uiselect.Contains("api"))
{
    if (Directory.Exists(Path.Combine(dir, "src",$"{name}.Api")))
    {
        Directory.Delete(Path.Combine(dir, "src",$"{name}.Api"), true);
    }
}

if (!uiselect.Contains("mvc"))
{
    if (Directory.Exists(Path.Combine(dir, "src",$"{name}.Mvc")))
    {
        Directory.Delete(Path.Combine(dir, "src",$"{name}.Mvc"), true);
    }
}

if (!uiselect.Contains("blazorserver"))
{
    if (Directory.Exists(Path.Combine(dir, "src",$"{name}.BlazorServer")))
    {
        Directory.Delete(Path.Combine(dir, "src",$"{name}.BlazorServer"), true);
    }
}

process.WaitForExit();



static void ShowBot()
{
    var bot = """
              ,--.  ,--.       ,--.,--.           ,-----.         ,--.  ,--.    ,---.                     ,-----.,--.,--. 
              |  '--'  | ,---. |  ||  | ,---.     |  |) /_  ,---. |  |,-'  '-. /  O  \  ,---.  ,---.     '  .--./|  |`--' 
              |  .--.  || .-. :|  ||  || .-. |    |  .-.  \| .-. ||  |'-.  .-'|  .-.  || .-. || .-. |    |  |    |  |,--. 
              |  |  |  |\   --.|  ||  |' '-' '    |  '--' /' '-' '|  |  |  |  |  | |  || '-' '| '-' '    '  '--'\|  ||  | 
              `--'  `--' `----'`--'`--' `---'     `------'  `---' `--'  `--'  `--' `--'|  |-' |  |-'      `-----'`--'`--' 
              """;
    Console.WriteLine(bot);
}