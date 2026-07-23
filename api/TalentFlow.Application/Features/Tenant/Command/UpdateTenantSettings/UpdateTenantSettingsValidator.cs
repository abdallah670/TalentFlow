using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Tenant.Command.UpdateTenantSettings
{
    public class UpdateTenantSettingsValidator :AbstractValidator<UpdateTenantSettings.UpdateTenantSettingsCommand>
    {
        public UpdateTenantSettingsValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Slug)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[a-z0-9-]+$")
                .WithMessage("Slug can contain only lowercase letters, numbers and hyphens.");

            RuleFor(x => x.SubscriptionPlan)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.SubscriptionPlan));

            RuleFor(x => x.CompanyLogoUrl)
                .Must(url => string.IsNullOrWhiteSpace(url) ||
                             Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Invalid logo URL.");

            RuleFor(x => x.PrimaryColor)
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .When(x => !string.IsNullOrWhiteSpace(x.PrimaryColor))
                .WithMessage("Invalid color format.");

            RuleFor(x => x.TimeZone)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.TimeZone));

            RuleFor(x => x.DateFormat)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.DateFormat));
        }

    }
}
