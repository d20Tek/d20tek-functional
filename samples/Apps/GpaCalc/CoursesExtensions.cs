﻿using D20Tek.Functional;

namespace GpaCalc;

internal static class CoursesExtensions
{
    public static IEnumerable<Course> InitializeCourses() => [];

    public static Identity<double> CalculateGpa(this Course[] courses, Func<string, double> gradeToPoint) =>
        courses.Fork(
            x => courses.Sum(x => x.Credits * gradeToPoint(x.Grade)),
            x => courses.Sum(x => x.Credits),
            (totalPoints, totalCredits) => totalPoints / totalCredits);
}