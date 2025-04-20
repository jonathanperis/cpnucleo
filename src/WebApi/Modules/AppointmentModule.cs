// namespace WebApi.Modules;

// public static class AppointmentModule
// {
//     public static void MapAppointmentEndpoints(this IVersionedEndpointRouteBuilder app)
//     {
//         var group = app.MapGroup("/api/appointments")
//             .WithTags("Appointments")
//             // .RequireAuthorization()
//             .HasApiVersion(1.0);
        
//         group.MapGet("/{id:guid}", GetIAppointmentById)
//             .Produces<AppointmentDto>()
//             .Produces(404)
//             .WithName(nameof(GetIAppointmentById))
//             .MapToApiVersion(1.0);
        
//         group.MapGet("/", GetAllAppointments)
//             .Produces<PaginatedResult<AppointmentDto>>()
//             .Produces(404)
//             .WithName(nameof(GetAllAppointments))
//             .WithOpenApi(operation => 
//             {
//                 operation.Parameters[0].Description = "Page number (default: 1)";
//                 operation.Parameters[1].Description = "Items per page (default: 10, max: 100)";
//                 operation.Parameters[2].Description = "Sort column (default: Id)";
//                 operation.Parameters[3].Description = "Sort order (ASC/DESC, default: ASC)";
//                 return operation;
//             })
//             .MapToApiVersion(1.0);
        
//         group.MapPost("/", CreateAppointment)
//             .Accepts<CreateAppointmentCommand>("application/json")
//             //.Produces<AppointmentDto>(201)
//             .Produces(201)
//             .Produces(400)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(CreateAppointment));
        
//         group.MapPatch("/{id:guid}", UpdateAppointment)
//             .Accepts<UpdateAppointmentCommand>("application/json")
//             .Produces(204)
//             .Produces(400)
//             .Produces(404)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(UpdateAppointment));
        
//         group.MapDelete("/{id:guid}", DeleteAppointment)
//             .Produces(204)
//             .MapToApiVersion(1.0)
//             .WithName(nameof(DeleteAppointment));
//     }
    
//     private static async Task<IResult> GetIAppointmentById(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new GetAppointmentByIdQuery(id));

//         return response.OperationResult switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Ok(response.Appointment),
//         };
//     }
    
//     private static async Task<IResult> GetAllAppointments([AsParameters] PaginationParams paginationParams, ISender sender, HttpContext httpContext)
//     {
//         var response = await sender.Send(new ListAppointmentQuery(paginationParams));
        
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
    
//     private static async Task<IResult> CreateAppointment(CreateAppointmentCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.Created(),
//         };
//     }
    
//     private static async Task<IResult> UpdateAppointment(Guid id, UpdateAppointmentCommand command, ISender sender)
//     {
//         var response = await sender.Send(command);

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
    
//     private static async Task<IResult> DeleteAppointment(Guid id, ISender sender)
//     {
//         var response = await sender.Send(new RemoveAppointmentCommand(id));

//         return response switch
//         {
//             OperationResult.Failed => Results.Problem(),
//             OperationResult.NotFound => Results.NotFound(),
//             _ => Results.NoContent(),
//         };
//     }
// }
