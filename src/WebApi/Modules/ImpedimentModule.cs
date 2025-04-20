// namespace WebApi.Modules;

// public static class ImpedimentModule
// {
//     public static void MapImpedimentEndpoints(this IVersionedEndpointRouteBuilder app)
//     {
//         var group = app.MapGroup("/api/impediments")
//             .WithTags("Impediments")
//             // .RequireAuthorization()
//             .HasApiVersion(1.0);
        
//         group.MapGet("/{id:guid}", GetIImpedimentById)
//             .Produces<ImpedimentDto>()
//             .Produces(404)
//             .WithName(nameof(GetIImpedimentById))
//             .MapToApiVersion(1.0);
        
//         group.MapGet("/", GetAllImpediments)
//             .Produces<PaginatedResult<ImpedimentDto>>()
//             .Produces(404)
//             .WithName(nameof(GetAllImpediments))
//             .WithOpenApi(operation => 
//             {
//                 operation.Parameters[0].Description = "Page number (default: 1)";
//                 operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
//                 operation.Parameters[2].Description = "Sort column (default: Id)";
//                 operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
//                 return operation;
//             })
//             .MapToApiVersion(1.0);
        
//         group.MapPost("/", CreateImpediment)
//             .Accepts<CreateImpedimentCommand>("application/json")
//             //.Produces<ImpedimentDto>(201)
//             .Produces(201)
//             .Produces(400)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(CreateImpediment));
        
//         group.MapPatch("/{id:guid}", UpdateImpediment)
//             .Accepts<UpdateImpedimentCommand>("application/json")
//             .Produces(204)
//             .Produces(400)
//             .Produces(404)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(UpdateImpediment));
        
//         group.MapDelete("/{id:guid}", DeleteImpediment)
//             .Produces(204)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(DeleteImpediment));
//     }
    
//     private static async Task<IResult> GetIImpedimentById(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new GetImpedimentByIdQuery(id));

//         return response.OperationResult switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Ok(response.Impediment),
//         };
//     }
    
//     private static async Task<IResult> GetAllImpediments([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
//     {
//         var response = await sender.Send(new ListImpedimentQuery(paginationParams));
        
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
    
//     private static async Task<IResult> CreateImpediment(CreateImpedimentCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Created(),
//         };
//     }
    
//     private static async Task<IResult> UpdateImpediment(Guid id, UpdateImpedimentCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
    
//     private static async Task<IResult> DeleteImpediment(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new RemoveImpedimentCommand(id));

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
// }
