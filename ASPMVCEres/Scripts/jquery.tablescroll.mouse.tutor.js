
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
					grade_values_filter();
					
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
					student_filter();
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

function studentFilterSuccess(r) {
	/*
	
	*/

	if (r.length == 0) {
		$("#student_table_rows").text("Student nie jest zapisany na rzaden przedmiot.");
		return;
	}


	for (i = 0; i < r.length; i++) {
		data1 = '<tr class="student" id="student_row_' + r[i].StudentID + '" onclick="row_mouse_click(this);" onmouseover="row_mouse_over(this);" onmouseout="row_mouse_out(this);">';
		data2 = '<td>' + r[i].FirstName + '</td>' + '<td>' + r[i].LastName + '</td>' + '<td>' + r[i].IndexNo + '</td></tr>';

		$("#student_table_rows").append(data1 + data2);

	}
}

function subjectFilterSuccess(r) {
	
	/*
	$("#semester_table_rows").text("");
	$("#subject_table_rows").text("");
	$("#grade_values_table_rows").text("");
	semesterRowSelection = false;
	subjectRowSelection = false;
	lastSelectedSemesterId = "";
	lastSelectedSubjectId = "";
	*/

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

var h = -1;

function grateValuesFilterSuccess(r) {
	//$("#grade_values_table_rows").text("");

	if (r.length == 0) {
		$("#grade_values_table_rows").text("Brak ocen z przedmiotu.");
		return;
	}

	for (i = 0; i < r.length; i++)
	{
	    /*
		data1 = '<tr id="grade_value_' + r[i].GradeValueID + '">';
		if (r[i].GradeValues != null) {
		    data2 = '<td>' + r[i].Name + '</td>' + '<td><input type="text" value="' + r[i].GradeValues[0].Value + '" /></td></tr>';
		}
		else
		    data2 = '<td>' + r[i].Name + '</td>' + '<td>' + '</td></tr>';

            $("#grade_values_table_rows").append(data1 + data2);
            */

	    
	    if (r[i].GradeValues != null) {
	        data1 = '<tr id="grade_value_' + r[i].GradeID + '">'
	        data2 = '<td style="width:30%" id="name_' + r[i].GradeID + '">' + r[i].Name + '</td><td style="width:30%">' + '<input style="width:20px" id="value_' + r[i].GradeID + '" type="text" value="' + r[i].GradeValues[0].Value.trim() + '" />' + '</td><td style="width:40%">';
	        stamp = stampToString(r[i].GradeValues[0].TimeStamp);
	        data3 = '<input type="hidden" id="stamp_' + r[i].GradeID + '" value="' + stamp + '" />';
	        data4 = '<input type="hidden" id="registration_' + r[i].GradeID + '" value="' + r[i].GradeValues[0].RegistrationID + '" />';
	        h = r[i].GradeValues[0].RegistrationID;
	        data5 = '<input type="button" id="add_button' + r[i].GradeID + '" name="' + r[i].GradeID + '" value="Add" onclick="create(this);"/>';
	        data6 = '<input type="button" id="save_button' + r[i].GradeID + '" name="' + r[i].GradeID + '" value="Zapisz" onclick="save(this);"/>';
	        data7 = '<input type="button" id="remove_button' + r[i].GradeID + '" name="' + r[i].GradeID + '" value="Usuń" onclick="remove(this);"/></td></tr>';

	        $("#grade_values_table_rows").append(data1 + data2 + data3 + data4 + data5 + data6 + data7);
	        $("#add_button" + r[i].GradeID).hide();
                
	    }
	    else 
	    {
	        data1 = '<tr id="grade_value_' + r[i].GradeID + '">'
	        data2 = '<td style="width:30%" id="name_' + r[i].GradeID + '">' + r[i].Name + '</td><td style="width:30%">' + '<input style="width:20px" id="value_' + r[i].GradeID + '" type="text" value="' + '" />' + '</td><td style="width:40%">';
	        data3 = '<input type="hidden" id="stamp_' + r[i].GradeID + '" value="' + '" />';

	        data4 = '<input type="hidden" id="registration_' + r[i].GradeID + '" value="' + h + '" />';
	        data5 = '<input type="button" id="add_button' + r[i].GradeID + '" name="' + r[i].GradeID + '" value="Add" onclick="create(this);"/>';
	        data6 = '<input type="button" id="save_button' + r[i].GradeID + '" name="' + r[i].GradeID + '" value="Zapisz" onclick="save(this);"/>';
	        data7 = '<input type="button" id="remove_button' + r[i].GradeID + '" name="' + r[i].GradeID + '" value="Usuń" onclick="remove(this);"/></td></tr>';
	        $("#grade_values_table_rows").append(data1 + data2 + data3 + data4 + data5 + data6 + data7);
	        $("#remove_button" + r[i].GradeID).hide();
	        $("#save_button" + r[i].GradeID).hide();
	        //data5 = '<input type="button" name="' + r[i].GradeValueID + '" value="Usuń" onclick="remove(this);"/></td></tr>';
	    }

	    
        

	}



}

function student_filter() {
	semester_id = lastSelectedSemesterId.substr(13, lastSelectedSemesterId.length - 1);
	subject_id = lastSelectedSubjectId.substr(12, lastSelectedSubjectId.length - 1);

	$.ajax({
		url: "/GradeValuesTutor/StudentFilter",
		type: "POST",
		data: { semesterId: semester_id, subjectId: subject_id }
	}).success(studentFilterSuccess);

}

function semester_filter() {
	semester_id = lastSelectedSemesterId.substr(13, lastSelectedSemesterId.length - 1);
	subject_id = lastSelectedSubjectId.substr(12, lastSelectedSubjectId.length - 1);

	$.ajax({
		url: "/GradeValuesTutor/SemesterFilter",
		type: "POST",
		data: { semesterId: semester_id, subjectId: subject_id }
	}).success(semesterFilterSuccess);

}

function sumject_filter() {
	student_id = lastSelectedStudentId.substr(12, lastSelectedStudentId.length - 1);
	semester_id = lastSelectedSemesterId.substr(13, lastSelectedSemesterId.length - 1);

	$.ajax({
		url: "/GradeValuesTutor/SubjectFilter",
		type: "POST",
		data: { semesterId: semester_id }
	}).success(subjectFilterSuccess);
}

function grade_values_filter() {
	student_id = lastSelectedStudentId.substr(12, lastSelectedStudentId.length - 1);
	semester_id = lastSelectedSemesterId.substr(13, lastSelectedSemesterId.length - 1);
	subject_id = lastSelectedSubjectId.substr(12, lastSelectedSubjectId.length - 1);

	$.ajax({
		url: "/GradeValuesTutor/GradeValuesFilter",
		type: "POST",
		data: { semesterId: semester_id, subjectId: subject_id, studentId: student_id }
	}).success(grateValuesFilterSuccess);
}

function remove(button) {
    id = button.name;
    stamp = $("#stamp_" + id).val();
    registration = $("#registration_" + id).val();

    alert(id + " " + stamp + " " + registration);

    
    $.ajax({
        url: "/GradeValuesTutor/Delete",
        type: "POST",
        data: { gradeId: id, registrationId: registration, stamp: stamp }
    }).success(deleteSuccess);
    
}

function create(button) {

    id = button.name;
    //stamp = $("#stamp_" + id).val();
    value = $("#value_" + id).val();
    registration = $("#registration_" + id).val();

    alert(id + " " + value + " " + registration);

    $.ajax({
        url: "/GradeValuesTutor/Add",
        type: "POST",
        data: { gradeId: id, registrationId: registration, value: value }
    }).success(createSuccess);

}

function save(button) {

    id = button.name;
    stamp = $("#stamp_" + id).val();
    value = $("#value_" + id).val();
    registration = $("#registration_" + id).val();


    $.ajax({
        url: "/GradeValuesTutor/Update",
        type: "POST",
        data: { gradeId: id, registrationId: registration, value: value, stamp: stamp }
    }).success(saveSuccess);

}

function stampToString(array) {
    data = "";
    for (ii = 0; ii < array.length; ii++) {
        data += array[ii] + "?";
    }

    return data.substr(0, data.length - 1);
}

function deleteSuccess(r) {


    //if (result.data != "") {
    //alert("**************");

    
    if (r.msg != null) {
        $("#error").text(r.msg);
        //alert(r.msg);
        if (r.data != null) {
            $("#name_" + r.data.GradeID).text(r.data.Name);
            $("#value_" + r.data.GradeID).val(r.data.Value);

            stamp = stampToString(r.data.TimeStamp);
            $("#stamp_" + r.data.GradeID).val(stamp);
            $("#registration_" + r.data.GradeID).val(r.data.RegistrationID);
        }
        return;
    }
    if (r.data != null) {
        //$("#grade_value_" + r.data.GradeID).hide();
        $("#value_" + r.data.GradeID).val("");
        $("#remove_button" + r.data.GradeID).hide();
        $("#save_button" + r.data.GradeID).hide();
        $("#add_button" + r.data.GradeID).show();
        //alert(r.data.GradeID + " ");
        return;
    }

    //alert("**************");
}

function saveSuccess(r) {


    //if (result.data != "") {
    //alert("**************");

    
    if (r.msg != null) {
        $("#error").text(r.msg);
        //alert(r.msg);
        if (r.data != null) {
            //$("#name_" + r.data.GradeID).text(r.data.Name);
            $("#value_" + r.data.GradeID).val(r.data.Value.trim());

            stamp = stampToString(r.data.TimeStamp);
            $("#stamp_" + r.data.GradeID).val(stamp);
            //$("#registration_" + r.data.GradeID).val(r.data.RegistrationID);
        }
        return;
    }
    if (r.data != null) {
        //$("#grade_value_" + r.data.GradeID).hide();
        $("#value_" + r.data.GradeID).val(r.data.Value.trim());

        stamp = stampToString(r.data.TimeStamp);
        $("#stamp_" + r.data.GradeID).val(stamp);
        //$("#remove_button" + r.data.GradeID).hide();
        //$("#save_button" + r.data.GradeID).hide();
        //$("#add_button" + r.data.GradeID).show();
        //alert(r.data.GradeID + " ");
        return;
    }
    

    //alert("**************");
}


function createSuccess(r) {


    //if (result.data != "") {
    //alert("**************");


        if (r.data != null) {
            $("#name_" + r.data.GradeID).text(r.data.Name);
            $("#value_" + r.data.GradeID).val(r.data.Value);

            stamp = stampToString(r.data.TimeStamp);
            $("#stamp_" + r.data.GradeID).val(stamp);
            $("#registration_" + r.data.GradeID).val(r.data.RegistrationID);
            $("#add_button" + r.data.GradeID).hide();
            $("#remove_button" + r.data.GradeID).show();
            $("#save_button" + r.data.GradeID).show();
        }


    //alert("**************");
}

