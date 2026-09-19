document.addEventListener('DOMContentLoaded', function () {
    var skillsSelect = document.getElementById('skills-select');
    if (skillsSelect && window.Choices) {
        new Choices(skillsSelect, {
            removeItemButton: true,
            placeholderValue: 'Search skills...',
            shouldSort: false
        });
    }

    function initDatePickers(scope) {
        if (!window.flatpickr) return;
        scope.querySelectorAll('.availability-date').forEach(function (input) {
            if (!input._flatpickr) flatpickr(input, { dateFormat: 'Y-m-d', allowInput: true });
        });
    }
    initDatePickers(document);

    var rowsContainer = document.getElementById('availability-rows');
    document.getElementById('add-availability-row').addEventListener('click', function () {
        var firstRow = rowsContainer.querySelector('.availability-row');
        var newRow = firstRow.cloneNode(true);
        newRow.querySelectorAll('input').forEach(function (input) {
            input.value = '';
            if (input._flatpickr) { input._flatpickr.destroy(); delete input._flatpickr; }
        });
        rowsContainer.appendChild(newRow);
        initDatePickers(newRow);
    });

    rowsContainer.addEventListener('click', function (e) {
        if (e.target.classList.contains('remove-row')) {
            if (rowsContainer.querySelectorAll('.availability-row').length > 1) {
                e.target.closest('.availability-row').remove();
            }
        }
    });
});