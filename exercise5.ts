import { Temporal } from "@js-temporal/polyfill";

import {
  EnrollmentStatus,
  describeEnrollment,
} from "./models/enrollment.model";

import { CourseStatus, describeCourse } from "./models/course.model";

const pending: EnrollmentStatus = {
  status: "PENDING",
  requestedAt: Temporal.Now.instant(),
  studentId: "STU-001",
  courseId: "CRS-101",
};

console.log(describeEnrollment(pending));

const webDev: CourseStatus = {
  status: "ACTIVE",
  enrolledCount: 28,
  startDate: Temporal.PlainDate.from("2026-09-01"),
};

console.log(describeCourse(webDev));
