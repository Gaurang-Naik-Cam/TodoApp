function deleteTodo(i) 
{
    $.ajax({
        url: 'Home/Delete',
        type: 'POST',
        data: {
            id: i
        },
        success: function() {
            window.location.reload();
        }
    });
}

function populateForm(i) {

    $.ajax({
        url: 'Home/PopulateForm',
        type: 'GET',
        data: {
            id: i
        },
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $("#Todo_Name").val(response.name);
            $("#Todo_Id").val(response.id);
            if (response.isComplete) {
                console.log("true condition")
                $("#Todo_IsComplete").attr("checked", "checked");
            }
            else
                $("#Todo_IsComplete").removeAttr("checked");
            $("#Todo_IsComplete").val(response.isComplete);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action", "/Home/Update");
        }
    });
}
