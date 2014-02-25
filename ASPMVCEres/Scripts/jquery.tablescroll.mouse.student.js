
var studentRowSelection = false;
var semesterRowSelection = false;
var subjectRowSelection = false;

var lastSelectedStudentId = "";
var lastSelectedSemesterId = "";
var lastSelectedSubjectId = "";

var linkColor = "#ffffff";
var howverColor = "#e22b2b";
var activeColor = "#FFCC00";

function row_mouse_click(row) {

    switch (row.className) {
        case "student":
            {
                if (row.id != lastSelectedStudentId) {
                    studentRowSelection = true;
                    if (lastSelectedStudentId != "")
                        $("#" + lastSelectedStudentId).css('background-color', linkColor);

                    row.style.backgroundColor = activeColor;
                    lastSelectedStudentId = row.id;
                    semester_filter();
                }
                else {
                    studentRowSelection = false;
                    lastSelectedStudentId = "";
                    row.style.backgroundColor = linkColor;
                }
                break;
            }
        case "semester":
            {
                if (row.id != lastSelectedSemesterId) {
                    semesterRowSelection = true;
                    if (lastSelectedSemesterId != "")
                        $("#" + lastSelectedSemesterId).css('background-color', linkColor);

                    row.style.backgroundColor = activeColor;
                    lastSelectedSemesterId = row.id;
                    sumject_filter();
                }
                else {
                    semesterRowSelection = false;
                    lastSelectedSemesterId = "";
                    row.style.backgroundColor = linkColor;
                }
                break;
            }
        case "subject":
            {
                if (row.id != lastSelectedSubjectId) {
                    subjectRowSelection = true;
                    if (lastSelectedSubjectId != "")
                        $("#" + lastSelectedSubjectId).css('background-color', linkColor);

                    row.style.backgroundColor = activeColor;
                    lastSelectedSubjectId = row.id;
                    grade_values_filter();
                }
                else {
                    subjectRowSelection = false;
                    lastSelectedSubjectId = "";
                    row.style.backgroundColor = linkColor;
                }
                break;
            }
    }


}

function row_mouse_over(row) {
    switch (row.className) {
        case "student":
            {
                if (lastSelectedStudentId != row.id)
                    row.style.backgroundColor = howverColor;

                if (studentRowSelection)
                    studentRowSelection = false;
                break;
            }
        case "semester":
            {
                if (lastSelectedSemesterId != row.id)
                    row.style.backgroundColor = howverColor;

                if (semesterRowSelection)
                    semesterRowSelection = false;
                break;
            }
        case "subject":
            {
                if (lastSelectedSubjectId != row.id)
                    row.style.backgroundColor = howverColor;

                if (subjectRowSelection)
                    subjectRowSelection = false;
                break;
            }

    }
}

function row_mouse_out(row) {
    switch (row.className) {
        case "student":
            {
                if (!studentRowSelection && lastSelectedStudentId != row.id) {
                    row.style.backgroundColor = linkColor;
                }

                if (studentRowSelection)
                    studentRowSelection = false;
                break;
            }
        case "semester":
            {
                if (!semesterRowSelection && lastSelectedSemesterId != row.id) {
                    row.style.backgroundColor = linkColor;
                }

                if (semesterRowSelection)
                    semesterRowSelection = false;
                break;
            }
        case "subject":
            {
                if (!subjectRowSelection && lastSelectedSubjectId != row.id) {
                    row.style.backgroundColor = linkColor;
                }

                if (subjectRowSelection)
                    subjectRowSelection = false;
                break;
            }

    }



}

function semesterFilterSuccess(r)
{
    $("#semester_table_rows").text("");
    $("#subject_table_rows").text("");
    $("#grade_values_table_rows").text("");
    semesterRowSelection = false;
    subjectRowSelection = false;
    lastSelectedSemesterId = "";
    lastSelectedSubjectId = "";

    if (r.length == 0) {
        $("#semester_table_rows").text("Student nie jest zapisany na rzaden przedmiot.");
        return;
    }


	for (i = 0; i < r.length; i++)
	{
		data1 = '<tr class="semester" id="semester_row_' + r[i].SemesterID + '" onclick="row_mouse_click(this);" onmouseover="row_mouse_over(this);" onmouseout="row_mouse_out(this);">';
		data2 = '<td>' + r[i].Name + '</td></tr>';

		$("#semester_table_rows").append(data1 + data2);
            
	}
}

function subjectFilterSuccess(r)
{
    $("#subject_table_rows").text("");
    $("#grade_values_table_rows").text("");
    subjectRowSelection = false;
    lastSelectedSubjectId = "";

    if (r.length == 0) {
        $("#subject_table_rows").text("Brak zapisu na przedmioty.");
        return;
    }

	for (i = 0; i < r.length; i++) {
		data1 = '<tr class="subject" id="subject_row_' + r[i].SubjectID + '" onclick="row_mouse_click(this);" onmouseover="row_mouse_over(this);" onmouseout="row_mouse_out(this);">';
		data2 = '<td>' + r[i].Name + '</td>' + '<td>' + r[i].Conspect + '</td>' + '<td>' + r[i].url + '</td></tr>';

		$("#subject_table_rows").append(data1 + data2);

	}
}

function grateValuesFilterSuccess(r)
{
    $("#grade_values_table_rows").text("");

    if (r.length == 0) {
        $("#grade_values_table_rows").text("Brak ocen z przedmiotu.");
        return;
    }

    for (i = 0; i < r.length; i++) {
        data1 = '<tr class="grade_value">';
        if (r[i].GradeValues != null)
            data2 = '<td>' + r[i].Name + '</td>' + '<td>' + r[i].GradeValues[0].Value + '</td></tr>';
        else
            data2 = '<td>' + r[i].Name + '</td>' + '<td>' + '</td></tr>';

        $("#grade_values_table_rows").append(data1 + data2);

    }
}

function semester_filter()
{
    id = lastSelectedStudentId.substr(12, lastSelectedStudentId.length - 1);

	$.ajax({
		url: "/GradeValues/SemesterFilter",
		type: "POST",
		data: { studentId: id }
	}).success(semesterFilterSuccess);

}

function sumject_filter()
{
    student_id = lastSelectedStudentId.substr(12, lastSelectedStudentId.length - 1);
    semester_id = lastSelectedSemesterId.substr(13, lastSelectedSemesterId.length - 1);

	$.ajax({
		url: "/GradeValues/SubjectFilter",
		type: "POST",
		data: { semesterId: semester_id, studentId: student_id }
	}).success(subjectFilterSuccess);
}

function grade_values_filter()
{
    student_id = lastSelectedStudentId.substr(12, lastSelectedStudentId.length - 1);
    semester_id = lastSelectedSemesterId.substr(13, lastSelectedSemesterId.length - 1);
    subject_id = lastSelectedSubjectId.substr(12, lastSelectedSubjectId.length - 1);

	$.ajax({
		url: "/GradeValues/GradeValuesFilter",
		type: "POST",
		data: { semesterId: semester_id, subjectId: subject_id, studentId: student_id }
	}).success(grateValuesFilterSuccess);
}
