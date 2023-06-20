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

        public async static Task ValidateSponsor(SponsorDto sponsorDto)
        {
            var validator = new SponsorValidation();

            var validationResult = await validator.ValidateAsync(sponsorDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }
        }
    }
}