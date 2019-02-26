using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using VideoOnDemand.Models;
using VideoOnDemand.Repositories;
using VideoOnDemand.Modules.DTOModels;
using VideoOnDemand.Modules.MembershipViewModels;

namespace VideoOnDemand.Controllers
{
    public class MembershipController : Controller
    {
        private string _userId;
        private readonly IMapper _mapper;
        private IReadRepository _db;

        public MembershipController(IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager, IMapper mapper, IReadRepository db)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _db = db;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var courseDtoObjects = _mapper.Map<List<CourseDTO>>(_db.GetCourses(_userId));

            var dashboardModel = new DashboardViewModel();
            dashboardModel.Courses = new List<List<CourseDTO>>();

            var numOfRows = courseDtoObjects.Count <= 3 ? 1 : courseDtoObjects.Count / 3;
            for(var i =0; i < numOfRows; i++)
            {
                dashboardModel.Courses.Add(courseDtoObjects.Take(3).ToList());
            }
            return View(dashboardModel);
        }

        [HttpGet]
        public IActionResult Course(int id)
        {
            var course = _db.GetCourse(_userId, id);
            var mappedCourseDTOs = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);
            var mappedModuleDTOs = _mapper.Map<List<ModuleDTO>>(course.Modules);

            for(var i=0; i< mappedModuleDTOs.Count; i++)
            {
                mappedModuleDTOs[i].Downloads = mappedModuleDTOs[i].Downloads.Count.Equals(0) ? null :
                    _mapper.Map<List<DownloadDTO>>(course.Modules[i].Downloads);

                mappedModuleDTOs[i].Videos = course.Modules[i].Videos.Count.Equals(0) ? null :
                    _mapper.Map<List<VideoDTO>>(course.Modules[i].Videos);
            }

            var courseModel = new CourseViewModel {
                Course = mappedCourseDTOs,
                Instructor = mappedInstructorDTO,
                Modules = mappedModuleDTOs
            };

            return View(courseModel);
        }

        [HttpGet]
        public IActionResult Video(int id)
        {
            var video = _db.GetVideo(_userId,id);
            var course = _db.GetCourse(_userId, video.CourseId);
            var mappedVideoDTO = _mapper.Map<VideoDTO>(video);
            var mappedCourseDTO = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);

            var videos = _db.GetVideos(_userId, video.ModuleId).ToList();
            var count = videos.Count();
            var index = videos.IndexOf(video);
            var previous = videos.ElementAtOrDefault(index - 1);
            var previousId = previous == null ? 0 : previous.Id;
            var next = videos.ElementAtOrDefault(index + 1);
            var nextId = next == null ? 0 : next.Id;
            var nextTitle = next == null ? string.Empty : next.Title;
            var nextThumb = next == null ? string.Empty : next.Thumbnail;

            var viewModel = new VideoViewModel
            {
                Video = mappedVideoDTO,
                Instructor = mappedInstructorDTO,
                Course = mappedCourseDTO,
                LessonInfo = new LessonInfoDTO
                {
                    LessonNumber = index + 1,
                    NumberOfLessons = count,
                    NextVideoId = nextId,
                    PreviousVideoId = previousId,
                    NextVideoTitle = nextThumb,
                    NextVideoThumbnail = nextThumb,
                    CurrentVideoTitle = video.Title,
                    CurrentVideoThumbnail = video.Thumbnail
                }

            };

            return View(viewModel);
        }
    }
}