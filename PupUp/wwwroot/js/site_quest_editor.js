$("#quest-selector").select2({
    closeOnSelect: false,
    placeholder: "Quest Condition",
    allowHtml: true,
    allowClear: true,
    tags: true
});
$('#quest-selector').change(function () {
    $("#RequiredQuests").val($(this).val().join(':'));
});
$("#training-selector").select2({
    closeOnSelect: false,
    placeholder: "Trainings Condition",
    allowHtml: true,
    allowClear: true,
    tags: true
});
$('#training-selector').change(function () {
    $("#TrainingIds").val($(this).val().join(':'));
});
$("#badge-selector").select2({
    closeOnSelect: false,
    placeholder: "Badges Rewards",
    allowHtml: true,
    allowClear: true,
    tags: true
});
$('#badge-selector').change(function () {
    $("#RewardValue").val($(this).val().join(':'));
});
$("#reward-type").on('change', function () {
    if ($(this).val() == 0) {
        $("#badge-selector").removeClass("non-active");
        $("#reward-num").addClass("non-active");
    }
    else {
        $("#badge-selector").addClass("non-active");
        $("#reward-num").removeClass("non-active");
    }
});