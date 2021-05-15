// Write your JavaScript code.

﻿$(function () {
    $("time").each(function (i, e) {
        const dateTimeValue = $(e).attr("datetime");
        if (!dateTimeValue) {
            return;
        }

        const time = moment.utc(dateTimeValue).local();
        $(e).html(time.format("DD/MM/YYYY"));
        $(e).attr("title", $(e).attr("datetime"));
    });
});