namespace Meetup.ApplicationCore.Validation
{
    public sealed class EventValidation : AbstractValidator<EventDto>
    {
        public EventValidation()
        {
            RuleFor(_ => _.Name).NotNull()
                                .NotEmpty()
                                .WithMessage("Name must be set")
                                .Length(1, 200);

            RuleFor(_ => _.Description).NotNull()
                                       .NotEmpty()
                                       .WithMessage("Description must be set");

            RuleFor(_ => _.Plan).NotNull()
                                .NotEmpty()
                                .WithMessage("Plan must be set");

            RuleFor(_ => _.Date).GreaterThanOrEqualTo(DateTime.Today)
                                .WithMessage("Date must be in the future");

            RuleFor(_ => _.Location).NotNull()
                                    .NotEmpty()
                                    .WithMessage("Location must be set");
        }
    }
}