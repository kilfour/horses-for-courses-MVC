using HorsesForCourses.Api.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HorsesForCourses.Core.Domain.Courses;
using Microsoft.EntityFrameworkCore.Metadata;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Core.Abstractions;

namespace HorsesForCourses.Api.Courses;

public class CourseDataConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> course)
    {
        course.HasKey(c => c.Id);

        var id = course.Property(c => c.Id)
            .HasConversion(new IdValueConverter<Course>())
            .Metadata;
        id.SetValueComparer(new IdValueComparer<Course>());
        id.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        id.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
        course.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasColumnType("INTEGER")
            .HasAnnotation("Sqlite:Autoincrement", true);

        course.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(DefaultString.MaxLength);

        course.Property(c => c.StartDate)
            .IsRequired();

        course.Property(c => c.EndDate)
            .IsRequired();

        course.Property(c => c.IsConfirmed)
            .IsRequired();

        course.OwnsMany(c => c.RequiredSkills, sb =>
        {
            sb.WithOwner().HasForeignKey("CourseId");

            sb.Property<string>("value")
              .HasColumnName("Skill")
              .IsRequired()
              .HasMaxLength(100);

            sb.HasKey("CourseId", "value");

            sb.ToTable("CourseRequiredSkills");
        });

        course.OwnsMany(c => c.TimeSlots, timeSlot =>
        {
            timeSlot.WithOwner().HasForeignKey("CourseId");
            timeSlot.ToTable("CourseTimeSlots");

            timeSlot.HasKey("CourseId", nameof(TimeSlot.Day), nameof(TimeSlot.Start));

            timeSlot.Property(t => t.Day)
                .HasColumnName("Day")
                .IsRequired();

            timeSlot.Property(t => t.Start)
                .HasColumnName("StartHour")
                .HasConversion(v => v.Value, v => OfficeHour.From(v))
                .IsRequired();

            timeSlot.Property(t => t.End)
                .HasColumnName("EndHour")
                .HasConversion(v => v.Value, v => OfficeHour.From(v))
                .IsRequired();
        });

        course.HasOne(c => c.AssignedCoach)
               .WithMany()
               .HasForeignKey("AssignedCoachId")
               .OnDelete(DeleteBehavior.Restrict);

        course.ToTable("Courses");
    }
}
