/*
* Manage/Index Form Control JavaScript File
*/

// Overwrite submit event for ajax requests
$("#walletForm").on("submit", function (e) {
    e.preventDefault();
    manageFormSubmit('EditWallet', '#walletForm', '#walletManageSection');
});
$('#changePassForm').on("submit", function (e) {
    e.preventDefault();
    manageFormSubmit('ChangePassword', '#changePassForm', '#passwordSection');
});

// Submit function with ajax post request
// String action : Controller action handling the post request
// String formId : Id of the form
// String resultTargetId : Where to load the result
var manageFormSubmit = function (action, formId, resultTargetId) {
    if (confirm("Are you sure?")) {
        $.ajax({
            url: "Manage/" + action,
            type: "post",
            data: $(formId).serialize(), // So model data will work
            success: function (result) {
                $(resultTargetId).html(result);
                // Register the event again.
                $(formId).on("submit", function (e) {
                    e.preventDefault();
                    manageFormSubmit(action, formId, resultTargetId);
                });
            },
            error: function (error) {
                $(resultTargetId).html(error);
                $(formId).on("submit", function (e) {
                    e.preventDefault();
                    manageFormSubmit(action, formId, resultTargetId);
                });
            }
        });
    }
}