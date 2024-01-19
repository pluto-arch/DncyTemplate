using System.Diagnostics;
using System.Text;

namespace DotnetyddTemplateCli
{
    public class CliService : IDisposable
    {
        private readonly CliOption _options;
        private readonly Process _process ;
        const string searchString = "boltapp";
        public CliService(CliOption options)
        {
            _options = options;
            if (!Directory.Exists(options.OutputDir))
            {
                Directory.CreateDirectory(options.OutputDir);
            }
            _process=new Process();
            _process.StartInfo.FileName = "dotnet";
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            _process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
        }

        private string slnPath = "";
        private string apiPath = "";
        private string mvcPath = "";
        private string blazorServerPath = "";
        private string aspireHostPath = "";
        private string aspireServerDefaultPath = "";


        public CliService CreateProject()
        {
            _process.StartInfo.Arguments =$"new boltapp -n {_options.ProjectName} -T {_options.EnableTenant}  -A {_options.EnableAspire} -o {_options.OutputDir}";
            _process.Start();
            _ = _process.StandardOutput.ReadToEnd();

            var dir = _options.OutputDir;
            var name = _options.ProjectName;
            slnPath = Path.Combine(dir, $"{name}.sln");
            apiPath = Path.Combine(dir, "src",$"{name}.Api",$"{name}.Api.csproj");
            mvcPath = Path.Combine(dir, "src",$"{name}.Mvc",$"{name}.Mvc.csproj");
            blazorServerPath = Path.Combine(dir, "src",$"{name}.BlazorServer",$"{name}.BlazorServer.csproj");
            aspireHostPath=Path.Combine(dir, "aspire",$"{name}.AppHost",$"{name}.AppHost.csproj");
            aspireServerDefaultPath=Path.Combine(dir, "aspire",$"{name}.ServiceDefaults",$"{name}.ServiceDefaults.csproj");

            Console.WriteLine($"generate project : {_options.ProjectName}");
            return this;
        }

       
        public CliService AddToSln()
        {
            _process.StartInfo.Arguments = "";
            StringBuilder commandBuilder = new StringBuilder();
            var uiselect = _options.SelectUi.Select(x => x.ToLower());
            if (uiselect.Contains("api"))
            {
                commandBuilder.Append($" {apiPath} ");
            }

            if (uiselect.Contains("mvc"))
            {
                commandBuilder.Append($" {mvcPath} ");
            }

            if (uiselect.Contains("blazorserver"))
            {
                commandBuilder.Append($" {blazorServerPath} ");
            }

            if (_options.EnableAspire)
            {
                commandBuilder.Append($" {aspireHostPath} {aspireServerDefaultPath} ");
            }

            _process.StartInfo.Arguments =$"sln {slnPath} add {commandBuilder.ToString()}";
            _process.Start();
            _ = _process.StandardOutput.ReadToEnd();
            Console.WriteLine($"success add csproj to sln : {slnPath}");
            return this;
        }



        public CliService SetAspire()
        {
            if (!_options.EnableAspire)
            {
                return this;
            }
            _process.StartInfo.Arguments = "";
            StringBuilder commandBuilder = new StringBuilder();
            StringBuilder aspireProjectBuilder = new StringBuilder();
            var name = _options.ProjectName;
            var uiselect = _options.SelectUi.Select(x => x.ToLower());
            if (!uiselect.Contains("api"))
            {
                commandBuilder.Append($" {apiPath} ");
            }
            else
            {
                aspireProjectBuilder.AppendLine($"builder.AddProject<Projects.{name}_Api>(\"{name}.api\");");
            }

            if (!uiselect.Contains("mvc"))
            {
                commandBuilder.Append($" {mvcPath} ");
            }
            else
            {
                aspireProjectBuilder.AppendLine($"builder.AddProject<Projects.{name}_Mvc>(\"{name}.mvc\");");
            }

            if (!uiselect.Contains("blazorserver"))
            {
                commandBuilder.Append($" {blazorServerPath} ");
            }
            else
            {
                aspireProjectBuilder.AppendLine($"builder.AddProject<Projects.{name}_BlazorServer>(\"{name}.blazorserver\");");
            }

            _process.StartInfo.Arguments =$" remove {aspireHostPath} reference {commandBuilder.ToString()}";
            _process.Start();
            _ = _process.StandardOutput.ReadToEnd();

            _process.StartInfo.Arguments = "";

            var fileContent = $"""
                               var builder = DistributedApplication.CreateBuilder(args);

                               {aspireProjectBuilder.ToString()}
                               builder.Build().Run();
                               """;

            var aspireProgramFile = Path.Combine(_options.OutputDir, "aspire",$"{name}.AppHost", "Program.cs");

            if (File.Exists(aspireProgramFile))
            {
                File.WriteAllText(aspireProgramFile, fileContent,Encoding.Default);
            }

            Console.WriteLine($"success set aspire : {aspireHostPath}");
            return this;
        }



        public CliService ClearDir()
        {
            var dir = _options.OutputDir;
            var name = _options.ProjectName;
            var uiselect = _options.SelectUi.Select(x => x.ToLower());
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

            return this;
        }


        public CliService Check()
        {
            _process.StartInfo.Arguments = "new list --columns author";
            _process.Start();
            string output = _process.StandardOutput.ReadToEnd();
            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.All(x=>!x.Contains(searchString)))
            {
                Console.WriteLine("no template install, do `dotnet new install DotNetBoltTemplate.{version}.nupkg ` first!");
            }
            return this;
        }

        public void Run()
        {
            _process.WaitForExit();
            Console.WriteLine($"Successfully generated project on {_options.OutputDir} ");
        }



        public void Dispose()
        {
            _process?.Dispose();
        }
    }




    public record CliOption
    {
        public string ProjectName { get; set; }

        public string OutputDir { get; set; }

        public string[] SelectUi { get; set; }

        public bool EnableAspire { get; set; }

        public bool EnableTenant { get; set; }
    }
}