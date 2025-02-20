global using Application.Common.Context;
global using Application.Common.Models;
global using Application.UseCases.Appointment.CreateAppointment;
global using Application.UseCases.Appointment.GetAppointmentById;
global using Application.UseCases.Appointment.ListAppointment;
global using Application.UseCases.Appointment.RemoveAppointment;
global using Application.UseCases.Appointment.UpdateAppointment;
global using Application.UseCases.Assignment.CreateAssignment;
global using Application.UseCases.Assignment.GetAssignmentById;
global using Application.UseCases.Assignment.ListAssignment;
global using Application.UseCases.Assignment.RemoveAssignment;
global using Application.UseCases.Assignment.UpdateAssignment;
global using Application.UseCases.AssignmentImpediment.CreateAssignmentImpediment;
global using Application.UseCases.AssignmentImpediment.GetAssignmentImpedimentById;
global using Application.UseCases.AssignmentImpediment.ListAssignmentImpediment;
global using Application.UseCases.AssignmentImpediment.RemoveAssignmentImpediment;
global using Application.UseCases.AssignmentImpediment.UpdateAssignmentImpediment;
global using Application.UseCases.AssignmentType.CreateAssignmentType;
global using Application.UseCases.AssignmentType.GetAssignmentTypeById;
global using Application.UseCases.AssignmentType.ListAssignmentType;
global using Application.UseCases.AssignmentType.RemoveAssignmentType;
global using Application.UseCases.AssignmentType.UpdateAssignmentType;
global using Application.UseCases.Impediment.CreateImpediment;
global using Application.UseCases.Impediment.GetImpedimentById;
global using Application.UseCases.Impediment.ListImpediment;
global using Application.UseCases.Impediment.RemoveImpediment;
global using Application.UseCases.Impediment.UpdateImpediment;
global using Application.UseCases.Project.CreateProject;
global using Application.UseCases.Project.GetProjectById;
global using Application.UseCases.Project.ListProject;
global using Application.UseCases.Project.RemoveProject;
global using Application.UseCases.Project.UpdateProject;
global using Application.UseCases.Organization.CreateOrganization;
global using Application.UseCases.Organization.GetOrganizationById;
global using Application.UseCases.Organization.ListOrganization;
global using Application.UseCases.Organization.RemoveOrganization;
global using Application.UseCases.Organization.UpdateOrganization;
global using Application.UseCases.User.CreateUser;
global using Application.UseCases.User.GetUserById;
global using Application.UseCases.User.ListUser;
global using Application.UseCases.User.RemoveUser;
global using Application.UseCases.User.UpdateUser;
global using Application.UseCases.UserAssignment.CreateUserAssignment;
global using Application.UseCases.UserAssignment.GetUserAssignmentById;
global using Application.UseCases.UserAssignment.ListUserAssignments;
global using Application.UseCases.UserAssignment.RemoveUserAssignment;
global using Application.UseCases.UserAssignment.UpdateUserAssignment;
global using Application.UseCases.UserProject.CreateUserProject;
global using Application.UseCases.UserProject.GetUserProjectById;
global using Application.UseCases.UserProject.ListUserProject;
global using Application.UseCases.UserProject.RemoveUserProject;
global using Application.UseCases.UserProject.UpdateUserProject;
global using Application.UseCases.Workflow.CreateWorkflow;
global using Application.UseCases.Workflow.GetWorkflowById;
global using Application.UseCases.Workflow.ListWorkflow;
global using Application.UseCases.Workflow.RemoveWorkflow;
global using Application.UseCases.Workflow.UpdateWorkflow;
global using Domain.Entities;
global using Domain.Repositories;
global using Microsoft.EntityFrameworkCore;
global using Moq;
global using Moq.EntityFrameworkCore;
global using System.Threading;
global using Xunit;
