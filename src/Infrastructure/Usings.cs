global using Application.Common.Context;
global using Bogus;
global using Dapper;
global using Delta;
global using Domain.Entities;
global using Domain.Models;
global using Domain.Repositories;
global using Domain.UoW;
global using Infrastructure.Common.Context;
global using Infrastructure.Common.Helpers;
global using Infrastructure.Common.Mappings;
global using Infrastructure.Repositories;
global using Infrastructure.UoW;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Npgsql;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Reflection;
global using System.Text;