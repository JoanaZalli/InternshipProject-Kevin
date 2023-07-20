﻿using Application.UseCases.ActivateAccount.Commands;
using Application.UseCases.Authentication.Commands;
using Application.UseCases.Authentication.Queries;
using Application.UseCases.ForgotPassword.Commands;
using Application.UseCases.ForgotPassword.Queries;
using Application.UseCases.ForgotUsername.Queries;
using Application.UseCases.ResendEmailVerification.Commands;
using AutoMapper;
using Infrastructure.Persistence.Seeds;
using InternshipProject.Objects.Requests.AuthenticationRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers {
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper) {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest) {

            var registerCommand = _mapper.Map<RegisterCommand>(registerRequest);

            var result = await _mediator.Send(registerCommand);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogInRequest logInRequest) {

            var loginQuery = _mapper.Map<LoginQuery>(logInRequest);

            var result = await _mediator.Send(loginQuery);

            return Ok(result);
        }

        // verify email used for registration
        [AllowAnonymous]
        [HttpGet("verify-email")] // [FromQuery] is always a GET request. Also this call makes changes to the database,
                                  // declaring it as POST would give a 405 error. 
        public async Task<IActionResult> VerifyEmail([FromQuery] string token, [FromQuery] string email) {

            var activationCommand = new ActivateAccountCommand {
                Email = email,
                Token = token
            };

            await _mediator.Send(activationCommand);
            return Ok();
        }

        // send new email with new token if old token expired
        [AllowAnonymous]
        [HttpPatch("request-new-email")]
        public async Task<IActionResult> RequestNewEmail([FromBody] ResendVerificationEmailRequest resendVerificationEmailRequest) {
            var request = _mapper.Map<ResendEmailVerificationCommand>(resendVerificationEmailRequest);

            await _mediator.Send(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("forgot-username")]
        public async Task<IActionResult> ForgotUsername([FromQuery] ForgotUsernameRequest forgotUsernameRequest) {
            var request = _mapper.Map<ForgotUsernameQuery>(forgotUsernameRequest);

            await _mediator.Send(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] ForgotPasswordRequest forgotPasswordRequest) {
            var request = _mapper.Map<ForgotPasswordQuery>(forgotPasswordRequest);

            await _mediator.Send(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromHeader] string token, [FromBody] ResetPasswordRequest resetPasswordRequest) {

            var request = new ResetPasswordCommand {
                Email = resetPasswordRequest.Email,
                Token = token,
                Password = resetPasswordRequest.Password,
                ConfirmPassword = resetPasswordRequest.ConfirmPassword
            };

            await _mediator.Send(request);

            return Ok();
        }


        // TBD
        [Authorize(Policy = Permissions.IsSuperAdmin)]
        [HttpPost("test-policy")]
        public IActionResult TestPolicy() {
            return Ok();
        }

        // TBD
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("test-role")]
        public IActionResult TestRoles([FromQuery] string str) {
            return Ok(str);
        }
    }
}
