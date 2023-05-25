namespace Meetup.ApplicationCore.Validation
{
    public sealed class SponsorValidation : AbstractValidator<SponsorDto>
    {
        public SponsorValidation()
        {
            RuleFor(_ => _.Name).NotNull()
                                .NotEmpty()
                                .WithMessage("Name must be set")
                                .Length(1,50);
        }
    }
}
