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
            $("#Todo_IsComplete").prop("checked", response.isComplete)
            $("#Todo_IsComplete").val(response.isComplete);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action", "/Home/Update");
        }
    });
}




