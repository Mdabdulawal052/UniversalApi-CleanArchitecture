using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Commands.UserCommand;
using MediatR;

using Application.Queries.UserQueries;
using Application.Queries.MenuQueries;

namespace Web.Api.Handlers
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly IMediator _mediator;
        public string ClaimType { get; }

        public RolesAuthorizationHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                await Task.CompletedTask;
            }

            var validRole = false;
            if (requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                //if (context.Resource is RouteEndpoint endpoint)
                //{
                //    endpoint.RoutePattern.RequiredValues.TryGetValue("controller", out var _controller);
                //    endpoint.RoutePattern.RequiredValues.TryGetValue("action", out var _action);
                //    var claims = context.User.Claims;
                //    var userName = claims.FirstOrDefault(c => c.Type == "name").Value;
                //    var roles = requirement.AllowedRoles;

                //    validRole = await _dataAccessService.GetMenuItemsAsync(context.User, _controller.ToString(), _action.ToString());
                //}/*new Users().GetUsers().Where(p => roles.Contains(p.Role) && p.UserName == userName).Any();*/


                //var claims = context.User.Claims;
                //var userName = claims.FirstOrDefault(c => c.Type == "role").Value;
                //var roles = requirement.AllowedRoles;
                var userClaims = context.User.Claims;
                string userName = userClaims.First(c => c.Type == ClaimTypes.Name).Value;
                var role = userClaims.First(c => c.Type == ClaimTypes.Role).Value;//requirement.AllowedRoles.ToList();

                var path = ((DefaultHttpContext)context.Resource).Request.Path.Value;

                validRole =await _mediator.Send(new GetMenuItemQuery() { ctx = context.User, userName = userName, Roles = role, path = path });
                //await _dataAccessService.GetMenuItemsAsync(context.User, userName, role, path);
            }

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            await Task.CompletedTask;
        }
    }
}
