// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.async-request').on("click", function (e) {

    //e.preventDefault();

    let url = this.attributes['href'].value;

    $('#container').load(url);
});