// namespace WebApi.Modules;

// public static class OrganizationModule
// {
//     public static void MapOrganizationEndpoints(this IVersionedEndpointRouteBuilder app)
//     {
//         var group = app.MapGroup("/api/organizations_old")
//             .WithTags("Organizations")
//             // .RequireAuthorization()
//             .HasApiVersion(1.0);
        
//         group.MapGet("/{id:guid}", GetIOrganizationById)
//             .Produces<OrganizationDto>()
//             .Produces(204)
//             .WithName(nameof(GetIOrganizationById))
//             .MapToApiVersion(1.0);
        
//         group.MapGet("/", GetAllOrganizations)
//             .Produces<PaginatedResult<OrganizationDto>>()
//             .Produces(204)
//             .WithName(nameof(GetAllOrganizations))
//             .WithOpenApi(operation => 
//             {
//                 operation.Parameters[0].Description = "Page number (default: 1)";
//                 operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
//                 operation.Parameters[2].Description = "Sort column (default: Id)";
//                 operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
//                 return operation;
//             })
//             .MapToApiVersion(1.0);
        
//         group.MapPost("/", CreateOrganization)
//             .Accepts<CreateOrganizationCommand>("application/json")
//             //.Produces<OrganizationDto>(201)
//             .Produces(201)
//             .Produces(400)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(CreateOrganization));
        
//         group.MapPatch("/{id:guid}", UpdateOrganization)
//             .Accepts<UpdateOrganizationCommand>("application/json")
//             .Produces(204)
//             .Produces(400)
//             .Produces(404)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(UpdateOrganization));
        
//         group.MapDelete("/{id:guid}", DeleteOrganization)
//             .Produces(204)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(DeleteOrganization));
//     }
    
//     private static async Task<IResult> GetIOrganizationById(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new GetOrganizationByIdQuery(id));

//         return response.OperationResult switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Ok(response.Organization),
//         };
//     }
    
//     private static async Task<IResult> GetAllOrganizations([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
//     {
//         var response = await sender.Send(new ListOrganizationQuery(paginationParams));
        
//         httpContext.Response.Headers.Append("X-Pagination-TotalCount", response.Result.TotalCount.ToString());
//         httpContext.Response.Headers.Append("X-Pagination-PageSize", response.Result.PageSize.ToString());
//         httpContext.Response.Headers.Append("X-Pagination-CurrentPage", response.Result.PageNumber.ToString());
//         httpContext.Response.Headers.Append("X-Pagination-TotalPages", response.Result.TotalPages.ToString());
        
//         return response.OperationResult switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Ok(response.Result),
//         };
//     }
    
//     private static async Task<IResult> CreateOrganization(CreateOrganizationCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Created(),
//         };
//     }
    
//     private static async Task<IResult> UpdateOrganization(Guid id, UpdateOrganizationCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
    
//     private static async Task<IResult> DeleteOrganization(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new RemoveOrganizationCommand(id));

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
// }
