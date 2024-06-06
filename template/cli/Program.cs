using DotnetyddTemplateCli;
using Sharprompt;

System.Console.InputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine($"dotnetydd tool v1.0");
Console.WriteLine("================================");
ShowBot();
Console.WriteLine("================================");
if (args.Length <= 0)
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
Console.WriteLine($"名称 {name}");
Console.WriteLine($"租户： {hasTenant}");
Console.WriteLine($"用户界面： {string.Join(",", ui)}");
Console.WriteLine($"目录： {dir}");
Console.WriteLine($"Aspire： {hasAspire}");
#endif

#if DEBUG
dir = Path.Combine(dir, "TempDemo");
#endif
if (string.IsNullOrEmpty(name))
{
    Console.WriteLine("项目名称不能为空");
    return;
}
string[] unsupportChars= new string[] { " ", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "{", "}", "[", "]", "|", "\\", ":", ";", "\"", "'", "<", ">", ",", ".", "?", "/", " " };
if (unsupportChars.Any(x=>name.Contains(x)))
{
    Console.WriteLine("项目名称含有特殊字符,将被替换为‘_’");
    name = name.Replace(" ", "_");
    foreach (var item in unsupportChars)
    {
        name = name.Replace(item, "_");
    }
}

var options = new CliOption
{
    ProjectName = name,
    OutputDir = dir,
    SelectUi = ui.ToArray(),
    EnableAspire = hasAspire,
    EnableTenant = hasTenant
};
var cliservice = new CliService(options);

try
{
    cliservice.Check()
        .CreateProject()
        .AddToSln()
        .SetAspire()
        .ClearDir()
        .Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
finally
{
    cliservice?.Dispose();
}




return;

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




