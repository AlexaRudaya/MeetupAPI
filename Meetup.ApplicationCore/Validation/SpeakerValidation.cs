namespace Meetup.ApplicationCore.Validation
{
    public sealed class SpeakerValidation : AbstractValidator<SpeakerDto>
    {
        public SpeakerValidation()
        {
            RuleFor(_ => _.Name).NotNull()
                                .NotEmpty()
                                .WithMessage("Name must be set")
                                .Length(1, 50);
        }

        public async static Task ValidateSpeaker(SpeakerDto speakerDto)
        {
            var validator = new SpeakerValidation();

            var validationResult = await validator.ValidateAsync(speakerDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }
        }
    }
}