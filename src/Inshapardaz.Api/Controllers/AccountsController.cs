﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inshapardaz.Api.Entities;
using Inshapardaz.Api.Helpers;
using Inshapardaz.Api.Models.Accounts;
using Inshapardaz.Api.Services;
using Inshapardaz.Domain.Models;
using System.Threading;
using Inshapardaz.Domain.Ports.Handlers.Account;
using System.Threading.Tasks;
using Paramore.Darker;
using Inshapardaz.Api.Converters;
using Inshapardaz.Api.Views.Accounts;

namespace Inshapardaz.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IRenderAccount _accountRenderer;

        public AccountsController(
            IAccountService accountService,
            IQueryProcessor queryProcessor,
            IMapper mapper,
            IRenderAccount accountRenderer)
        {
            _accountService = accountService;
            _queryProcessor = queryProcessor;
            _accountRenderer = accountRenderer;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public ActionResult<AuthenticateResponse> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _accountService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            _accountService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _accountService.Register(model, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }

        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(VerifyEmailRequest model)
        {
            _accountService.VerifyEmail(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("validate-reset-token")]
        public IActionResult ValidateResetToken(ValidateResetTokenRequest model)
        {
            _accountService.ValidateResetToken(model);
            return Ok(new { message = "Token is valid" });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _accountService.ResetPassword(model);
            return Ok(new { message = "Password reset successful, you can now login" });
        }

        [Authorize(Role.Admin)]
        [HttpGet(Name = nameof(AccountsController.GetAll))]
        public async Task<IActionResult> GetAll(string query, int pageNumber = 1, int pageSize = 10, CancellationToken token = default(CancellationToken))
        {
            var seriesQuery = new GetAccountsQuery(pageNumber, pageSize) { Query = query };
            var series = await _queryProcessor.ExecuteAsync(seriesQuery, cancellationToken: token);

            var args = new PageRendererArgs<AccountModel>
            {
                Page = series,
                RouteArguments = new PagedRouteArgs { PageNumber = pageNumber, PageSize = pageSize, Query = query },
            };

            return new OkObjectResult(_accountRenderer.Render(args));
        }

        [Authorize(Role.Admin, Role.LibraryAdmin)]
        [HttpGet("/libraries/{libraryId}/writers", Name = nameof(AccountsController.GetWriters))]
        public async Task<IActionResult> GetWriters(int libraryId, CancellationToken token = default(CancellationToken))
        {
            var writersQuery = new GetWritersQuery(libraryId);
            var writers = await _queryProcessor.ExecuteAsync(writersQuery, cancellationToken: token);

            return new OkObjectResult(_accountRenderer.RenderLookup(writers));
        }

        [Authorize]
        [HttpGet("{id:int}", Name = nameof(AccountsController.GetById))]
        public ActionResult<AccountView> GetById(int id)
        {
            // users can get their own account and admins can get any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            var account = _accountService.GetById(id);
            return Ok(account);
        }

        [Authorize(Role.Admin)]
        [HttpPost(Name = nameof(AccountsController.Create))]
        public ActionResult<AccountView> Create(CreateRequest model)
        {
            var account = _accountService.Create(model);
            return Ok(account);
        }

        [Authorize]
        [HttpPut("{id:int}", Name = nameof(AccountsController.Update))]
        public ActionResult<AccountView> Update(int id, UpdateRequest model)
        {
            // users can update their own account and admins can update any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            // only admins can update role
            if (Account.Role != Role.Admin)
                model.Role = null;

            var account = _accountService.Update(id, model);
            return Ok(account);
        }

        [Authorize]
        [HttpDelete("{id:int}", Name = nameof(AccountsController.Delete))]
        public IActionResult Delete(int id)
        {
            // users can delete their own account and admins can delete any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            _accountService.Delete(id);
            return Ok(new { message = "Account deleted successfully" });
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
#if DEBUG
                SameSite = SameSiteMode.Lax
#else
                SameSite = SameSiteMode.None,
                Secure = true
#endif
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
