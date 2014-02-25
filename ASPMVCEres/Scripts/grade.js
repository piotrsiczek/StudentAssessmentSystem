function errorBox(xhr, status, error)
{
    alert("dupa"); 
    //var err = eval("(" + xhr.responseText + ")");
    alert(xhr.statusText + " " + status + " " + error);
}

function deleteSuccess(r) {


    //if (result.data != "") {
    //alert("**************");

    if (r.msg != null)
    {
        $("#error").text(r.msg);
        //alert(r.msg);
        if (r.data != null)
        {
            $("#name_" + r.data.GradeID).text(r.data.Name);
            $("#max_" + r.data.GradeID).text(r.data.MaxValue);
            stamp = stampToString(r.data.TimeStamp);
            $("#stamp_" + r.data.GradeID).val(stamp);
        }
        return;
    }
    if (r.data != null)
    {
        $("#row_" + r.data.GradeID).hide();
        //alert(r.data.GradeID + " ");
        return;
    }
    
    //alert("**************");
}

function createSuccess(r) {

    //alert("asdfsdfsdf" + r.data.Name);

    if (r.msg != null) {
        $("#error").text(r.msg);
    }
    if (r.data != null)
    {

        /*
        <tr id="row_42100">

        <td>
            42100
        </td>
        <td>
            2100
        </td>
        <td class="field" id="name_42100">K1</td>
        <td class="field" id="max_42100">20</td>
        
        <td>

            <input type="hidden" class="hidden_field" id="stamp_42100" value="0?0?0?0?0?0?7?246" />
            <input type="button" name="42100" value="Edytuj" onclick='edit(this);'/>
            <input type="button" name="42100" value="Usuń" onclick='remove(this);'/>

   
            
        </td>
    </tr>
        */

        //alert(r.data.GradeID + " ");
        data1 = '<tr id="row_' + r.data.GradeID + '"><td style="width:30%" id="name_' + r.data.GradeID + '">' + r.data.Name + '</td><td style="width:30%" id="max_' + r.data.GradeID + '">' + r.data.MaxValue + '</td><td style="width:40%">';
        data2 = '<input type="hidden" id="stamp_' + r.data.GradeID + '" value="' + stampToString(r.data.TimeStamp) + '" />';
        data3 = '<input type="button" name="' + r.data.GradeID + '" value="Edytuj" onclick="edit(this);"/>';
        data4 = '<input type="button" name="' + r.data.GradeID + '" value="Usuń" onclick="remove(this);"/></td></tr>';

        //data2 = '<input type="button" value="Edit" onclick="edit(' + r.data.GradeID + ', &#39;' + r.data.Name + '&#39;, &#39; ' + r.data.MaxValue + '&#39;)"/>';

        //data3 = '<a data-ajax="true" data-ajax-method="POST" data-ajax-success="deleteSuccess" href="/PartialGrade/Delete?gradeId=' + r.data.GradeID;

        //stamp=0%3F0%3F0%3F0%3F0%3F0%3F7%3F245"
        //data4 = '&amp;stamp=' + r.data.TimeStamp[0] + '?' + r.data.TimeStamp[1] + '?' + r.data.TimeStamp[2] + '?' + r.data.TimeStamp[3] + '?' + r.data.TimeStamp[4] + '?' + r.data.TimeStamp[5] + '?' + r.data.TimeStamp[6] + '?' + r.data.TimeStamp[7]+'">Delete</a></td></tr>';


        $("#gradesTable").append(data1 + data2 + data3 + data4);
        clearFields();
    }
}

function saveSuccess(r)
{
    if (r.msg != null) {
        $("#error").text(r.msg);
        //alert(r.msg);
        //return;
    }
    if (r.data != null) {

        $("#name_" + r.data.GradeID).text(r.data.Name);
        $("#max_" + r.data.GradeID).text(r.data.MaxValue);
        stamp = stampToString(r.data.TimeStamp);
        $("#stamp_" + r.data.GradeID).val(stamp);
        //alert(r.data.GradeID + " ");
    }

    cancel();
}

function edit(button)
{
    //id, name, max, stamp
    id = button.name;

    //name = $("#row_" + id).find(".field")[0].innerText
    //max = $("#row_" + id).find(".field")[1].innerText;
    //stamp = $("#row_" + id).find(".hidden_field")[0].value;
    name = $("#name_" + id).text();
    max = $("#max_" + id).text();
    stamp = $("#stamp_" + id).val();

    //var id = $(this).data('assigned-id');
    //name = $(this).data('assigned-name').val();
    //max = $(this).data('assigned-max').val();

    $("#partialGradeName").val(name);
    $("#partialGradeValue").val(max);

    $("#addEditButton").val("Zapisz");
    $("#addEditButton").attr("onClick", "save(" + id + ", '" + stamp + "')");
    //$("#addEditButton").click = save(id);
    $("#cancelButton").show();
    
    //alert(id + name + max);
}

function remove(button)
{
    id = button.name;
    stamp = $("#stamp_" +  id).val();

    $.ajax({
    url: "/PartialGrade/Delete",
    type: "POST",
    data: { gradeId: id, stamp: stamp }
    }).success(deleteSuccess).error(errorBox);
}



function create()
{
    //alert("You are running jQuery version: " + $.fn.jquery);
    
    name = $("#partialGradeName").val();
    max = $("#partialGradeValue").val();

    $("#error").text("");

    if (name == "" || max == "")
    {
        $("#error").text("Prosze wpisać dane.");
        return;
    }

     
    $.ajax({
        url: "/PartialGrade/Create",
        type: "POST",
        data: { realizationId: $("#realizationIdHidden").val(), name: name, maxValue: max }
    }).success(createSuccess).error(errorBox);
}

function save(id, stamp)
{

    name = $("#partialGradeName").val();
    max = $("#partialGradeValue").val();

    $("#error").text("");

    if (name == "" || max == "") {
        $("#error").text("Prosze wpisać dane.");
        return;
    }

    $.ajax({
        url: "/PartialGrade/Save",
        type: "POST",
        data: { gradesId: id, name: name, maxValue: max, stamp: stamp }
    }).success(saveSuccess);

    

}

function cancel()
{
    $("#addEditButton").val("Dodaj");
    $("#addEditButton").attr("onClick", "create()");
    $("#cancelButton").hide();
    clearFields();
}

function clearFields()
{
    $("#partialGradeName").val("");
    $("#partialGradeValue").val("");
}

function test(button)
{
    alert($("#row_2100").find(".stamp").text());
    alert(button.id);
    alert(button.name);
}

function stampToString(array)
{
    data = "";
    for (i=0; i < array.length; i++)
    {
        data += array[i] + "?";
    }

    return data.substr(0, data.length - 1);
}