﻿global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net.Mime;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using System.ComponentModel.DataAnnotations;
global using DncyTemplate.Api.Constants;
global using DncyTemplate.Api.Infra;
global using DncyTemplate.Domain.Collections;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.HttpOverrides;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Options;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;

global using Serilog;