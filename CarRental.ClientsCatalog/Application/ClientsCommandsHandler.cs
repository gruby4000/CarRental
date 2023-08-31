using System.Text.Json;
using CarRental.BuildingBlocks.DDD;
using CarRental.ClientsCatalog.Application.Queries;
using CarRental.ClientsCatalog.Infrastructure;
using CarRental.ClientsCatalog.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.ClientsCatalog.Application;

public sealed class ClientsCommandsHandler
{
    private readonly ClientsContext _ctx;

    public ClientsCommandsHandler(ClientsContext ctx)
    {
        _ctx = ctx;
    }
}