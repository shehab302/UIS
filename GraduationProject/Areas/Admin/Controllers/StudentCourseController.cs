using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Models;
using Models.ViewModels;
using System.Linq.Expressions;
using Utility;
using System.Reflection.Emit;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.Professor},{SD.Student}")]

    public class StudentCourseController : Controller
    {
        IStudentCourseRepository StudentCourseRepository;

        ICourseRepository CourseRepository;

        IStudentRepository StudentRepository;
        public StudentCourseController(IStudentCourseRepository StudentCourseRepository, ICourseRepository courseRepository , IStudentRepository StudentRepository)
        {
            this.StudentCourseRepository = StudentCourseRepository;

            this.CourseRepository = courseRepository;

            this.StudentRepository = StudentRepository;
        }
        public IActionResult Index(string studentId)
        {
            var student = StudentRepository.GetOne(expression: e => e.StudentId == studentId).FirstOrDefault();

            ViewBag.studentId = studentId;

            var studentcourses = StudentCourseRepository.GetAll(includeProp: [e => e.Course],expression: e => e.StudentId == studentId).ToList();

            EnumLevel[] levels = { EnumLevel.First_Level, EnumLevel.Secound_Level, EnumLevel.Third_Level, EnumLevel.Fourth_Level };

            levels = levels.Where(level => level <= student.Level).ToArray();

            ViewBag.Levels = levels;

            List <char> grade = new List<char>();

            int sumOftotalStudentDegree = 0;
            int sumOftotalCourseDegree = 0;
            char totalgrade;

            foreach (EnumLevel level in levels)
            {
                var studentCoursesForLevel = studentcourses.Where(e => e.Course.CourseLevel == level).ToList();

                int sumOfStudentDegree = 0;
                int sumOfCourseDegree = 0;
                bool failedFlag = false;

                foreach (var item in studentCoursesForLevel)
                {
                    if (item.Grade == 'F')
                    {
                        failedFlag = true;
                        break;
                    }

                    sumOfStudentDegree += item.Degree;
                    sumOfCourseDegree += item.Course.Degree;
                }

                sumOftotalStudentDegree = sumOftotalStudentDegree + sumOfStudentDegree;
                sumOftotalCourseDegree = sumOftotalCourseDegree + sumOfCourseDegree;

                char levelGrade;

                if (failedFlag)
                {
                    levelGrade = 'F';
                }
                else
                {
                    double levelPercent = 0;
                    if (sumOfCourseDegree > 0)
                    {
                        levelPercent = (double)sumOfStudentDegree * 100 / sumOfCourseDegree;
                    }

                    if (levelPercent >= 90)
                        levelGrade = 'A';
                    else if (levelPercent >= 75)
                        levelGrade = 'B';
                    else if (levelPercent >= 60)
                        levelGrade = 'C';
                    else if (levelPercent >= 50)
                        levelGrade = 'D';
                    else
                        levelGrade = 'F';
                }

                grade.Add(levelGrade);

            }

            if ( grade.Contains('F'))
            {
                totalgrade = 'F';
            }
            else
            {
                double TotalPercent = 0;

                if (sumOftotalCourseDegree > 0)
                {
                    TotalPercent = (double)sumOftotalStudentDegree * 100 / sumOftotalCourseDegree;
                }

                if (TotalPercent >= 90)
                    totalgrade = 'A';
                else if (TotalPercent >= 75)
                    totalgrade = 'B';
                else if (TotalPercent >= 60)
                    totalgrade = 'C';
                else if (TotalPercent >= 50)
                    totalgrade = 'D';
                else
                    totalgrade = 'F';

            }

            ViewBag.totalgrade = totalgrade;

            return View(model : grade);
        }

        public IActionResult Index2(string studentId , EnumLevel level)
        {
            var studentcourses = StudentCourseRepository.GetAll(includeProp: [e => e.Course, e => e.Course.Member], expression: e => e.StudentId == studentId);

            var studentcourses2 = studentcourses.Where(e => e.Course.CourseLevel == level).ToList();

            ViewBag.studentId = studentId;

            return View(model: studentcourses2);
        }

        public IActionResult Create( int courseId)
        {
            var course = CourseRepository.GetOne( expression: e => e.CourseId == courseId).FirstOrDefault();

            return View(model : course);
        }

        [HttpPost]
        public IActionResult Create(string StudentSSN , int courseId)
        {
            var student = StudentRepository.GetOne(expression: e => e.SSN == StudentSSN).FirstOrDefault();

            var course = CourseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();

            var studentcourse = StudentCourseRepository.GetAll(expression: e => e.StudentId == student.StudentId);

            var studentcourse2 = studentcourse.Where(e => e.CourseId == courseId).FirstOrDefault();

            if (studentcourse2 == null && (student.Level == course.CourseLevel) )
            {
                var studentCourse = new StudentCourse
                {
                    StudentId = student.StudentId, 
                    CourseId = courseId
                };

                StudentCourseRepository.Add(studentCourse);
                StudentCourseRepository.Commit();
                TempData["Message"] = " The student is Added Successfully";
                return RedirectToAction(nameof(Resultpage));
            }

            else if (studentcourse2 != null)
            {
                TempData["Message"] = " The student is already assigned to this Course";
            }

            else if (studentcourse2 == null && (student.Level != course.CourseLevel))
            {
                TempData["Message"] = " The student level is not same as the Course level";
            }

            return RedirectToAction(nameof(Resultpage));
        }

        public IActionResult Resultpage()
        {
            string message = TempData["Message"] as string;
            ViewBag.Message = message;
            return View();
        }

        public IActionResult Resultpage2(int courseId)
        {
            string message = TempData["Message"] as string;

            ViewBag.Message = message;

            ViewBag.CourseId = courseId;

            return View();
        }

        public IActionResult Delete(int courseId)
        {
            var course = CourseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();

            return View(model: course);
        }

        [HttpPost]
        public IActionResult Delete(string StudentSSN, int courseId)
        {
            var student = StudentRepository.GetOne(expression: e => e.SSN == StudentSSN).FirstOrDefault();

            var course = CourseRepository.GetOne(expression: e => e.CourseId == courseId).FirstOrDefault();

            var studentcourse = StudentCourseRepository.GetAll(expression: e => e.StudentId == student.StudentId);

            var studentcourse2 = studentcourse.Where(e => e.CourseId == courseId).FirstOrDefault();

            if (studentcourse2 != null && (studentcourse2.StudentFinalDegree == 0) )
            {
                var studentCourse = new StudentCourse
                {
                    StudentId = student.StudentId,
                    CourseId = courseId
                };

                StudentCourseRepository.Delete(studentcourse2);
                StudentCourseRepository.Commit();
                TempData["Message"] = " The student is Deleted Successfully";
                return RedirectToAction(nameof(Resultpage));
            }

            else if (studentcourse2 == null)
            {
                TempData["Message"] = " The student is Already not added to this course";
            }

            else if (studentcourse2 != null && (studentcourse2.StudentFinalDegree != 0))
            {
                TempData["Message"] = " The student has finished this course and get final degrees, cannot be deleted";
            }

            return RedirectToAction(nameof(Resultpage));
        }

        public IActionResult Degrees(string studentId , int courseId)
        {
            var studentcourse = StudentCourseRepository.GetAll(includeProp: [e => e.Course] , expression: e => e.StudentId == studentId);

            var studentcourse2 = studentcourse.Where(e => e.CourseId == courseId).FirstOrDefault();

            return View(model: studentcourse2);
        }

        public IActionResult AddDegrees(string studentId, int courseId)
        {
            var studentcourse = StudentCourseRepository.GetAll( includeProp: [e => e.Course , e => e.Student] ,expression: e => e.StudentId == studentId);

            var studentcourse2 = studentcourse.Where(e => e.CourseId == courseId).FirstOrDefault();

            return View(model: studentcourse2);
        }

        [HttpPost]
        public IActionResult AddDegrees(StudentCourse studentcourse)
        {
            var course = CourseRepository.GetOne(expression: e => e.CourseId == studentcourse.CourseId).FirstOrDefault();
            if (
                   studentcourse.StudentAttendancedegree > course.AttendanceDegree
                || studentcourse.StudentMidTermDegree > course.MidTermDegree
                || studentcourse.StudentFinalDegree > course.FinalDegree
                || studentcourse.StudentOralDegree > course.OralDegree
                || studentcourse.StudentPracticalDegree > course.PracticalDegree
                )
            {
                TempData["Message"] = " Student degree must be not more than Exam degree";
                return RedirectToAction(nameof(Resultpage2), new { studentcourse.CourseId });

            }
            else
            {
                studentcourse.Degree = studentcourse.StudentFinalDegree + studentcourse.StudentMidTermDegree + studentcourse.StudentOralDegree + studentcourse.StudentPracticalDegree + studentcourse.StudentAttendancedegree;

                if (studentcourse.Degree >= (0.9*course.Degree))
                    studentcourse.Grade = 'A';
                else if (studentcourse.Degree >= (0.75*course.Degree))
                    studentcourse.Grade = 'B';
                else if (studentcourse.Degree >= (0.6*course.Degree))
                    studentcourse.Grade = 'C';
                else if (studentcourse.Degree >= (0.5*course.Degree))
                    studentcourse.Grade = 'D';

                else 
                    studentcourse.Grade = 'F';

                StudentCourseRepository.Edit(studentcourse);
                CourseRepository.Commit();
                TempData["Message"] = " Degrees added successfully";


                var studentcourses = StudentCourseRepository.GetAll(includeProp: [e => e.Course], expression: e => e.StudentId == studentcourse.StudentId);

                var level = studentcourse.Course.CourseLevel;

                var studentcourses2 = studentcourses.Where(e => e.Course.CourseLevel == level).ToList();

                var courses = CourseRepository.GetAll(expression: e => e.CourseLevel == level).ToList();

                var allCoursesEnrolled = courses.All(course =>studentcourses2.Any(sc => sc.CourseId == course.CourseId));

                if (allCoursesEnrolled == true)
                {
                    var passedAllCourses = studentcourses2.All(sc => sc.Grade != 'F' && (sc.Grade == 'A' || sc.Grade == 'B' || sc.Grade == 'C' || sc.Grade == 'D'));

                    if (passedAllCourses == true)
                    {
                       var student = StudentRepository.GetOne(expression : e => e.StudentId == studentcourse.StudentId).FirstOrDefault();

                        student.Level++;

                        StudentRepository.Edit(student);
                        StudentRepository.Commit();
                    }

                }


                return RedirectToAction(nameof(Resultpage2), new { studentcourse.CourseId });

            }

        }
    }
}
