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
    }
}
