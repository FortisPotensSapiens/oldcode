$(document).ready(() => {
    const checkAll = $("#accept-all");

    if (checkAll.length) {
        $(checkAll).on('change', function () {
            const checked = $(this).is(':checked');

            $('.agreement').map(function () {
                $(this).find('input').prop('checked', checked);
            })
        })
    }
})