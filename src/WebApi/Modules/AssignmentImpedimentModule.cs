// namespace WebApi.Modules;

// public static class AssignmentImpedimentModule
// {
//     public static void MapAssignmentImpedimentEndpoints(this IVersionedEndpointRouteBuilder app)
//     {
//         var group = app.MapGroup("/api/assignmentImpediments")
//             .WithTags("AssignmentImpediments")
//             // .RequireAuthorization()
//             .HasApiVersion(1.0);
        
//         group.MapGet("/{id:guid}", GetIAssignmentImpedimentById)
//             .Produces<AssignmentImpedimentDto>()
//             .Produces(404)
//             .WithName(nameof(GetIAssignmentImpedimentById))
//             .MapToApiVersion(1.0);
        
//         group.MapGet("/", GetAllAssignmentImpediments)
//             .Produces<PaginatedResult<AssignmentImpedimentDto>>()
//             .Produces(404)
//             .WithName(nameof(GetAllAssignmentImpediments))
//             .WithOpenApi(operation => 
//             {
//                 operation.Parameters[0].Description = "Page number (default: 1)";
//                 operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
//                 operation.Parameters[2].Description = "Sort column (default: Id)";
//                 operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
//                 return operation;
//             })
//             .MapToApiVersion(1.0);
        
//         group.MapPost("/", CreateAssignmentImpediment)
//             .Accepts<CreateAssignmentImpedimentCommand>("application/json")
//             //.Produces<AssignmentImpedimentDto>(201)
//             .Produces(201)
//             .Produces(400)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(CreateAssignmentImpediment));
        
//         group.MapPatch("/{id:guid}", UpdateAssignmentImpediment)
//             .Accepts<UpdateAssignmentImpedimentCommand>("application/json")
//             .Produces(204)
//             .Produces(400)
//             .Produces(404)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(UpdateAssignmentImpediment));
        
//         group.MapDelete("/{id:guid}", DeleteAssignmentImpediment)
//             .Produces(204)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(DeleteAssignmentImpediment));
//     }
    
//     private static async Task<IResult> GetIAssignmentImpedimentById(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new GetAssignmentImpedimentByIdQuery(id));

//         return response.OperationResult switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Ok(response.AssignmentImpediment),
//         };
//     }
    
//     private static async Task<IResult> GetAllAssignmentImpediments([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
//     {
//         var response = await sender.Send(new ListAssignmentImpedimentQuery(paginationParams));
        
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
    
//     private static async Task<IResult> CreateAssignmentImpediment(CreateAssignmentImpedimentCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Created(),
//         };
//     }
    
//     private static async Task<IResult> UpdateAssignmentImpediment(Guid id, UpdateAssignmentImpedimentCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
    
//     private static async Task<IResult> DeleteAssignmentImpediment(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new RemoveAssignmentImpedimentCommand(id));

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
// }
