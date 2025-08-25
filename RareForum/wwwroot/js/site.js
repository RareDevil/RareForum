// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

// document ready function - Jquery
// $(function() {
//     $("input").each(function(){
//         if ($(this).hasClass("input-validation-error")) {
//             $(this).addClass("is-invalid");
//
//             const errorMessageElement = $(".field-validation-error[data-valmsg-for="+$(this).attr("name")+"]");
//             if (errorMessageElement !== null) {
//                 const popover = new bootstrap.Popover(this, {
//                     placement: 'right',
//                     content: errorMessageElement.text(),
//                     trigger: 'focus',
//                     customClass: 'custom-error-popover'
//                 });
//                 popover.show();
//             }
//         }
//     });
// });

// Native javascript.
document.querySelectorAll("input,textarea").forEach((input) => {
    if (input.classList.contains("input-validation-error")) {
        input.classList.add("is-invalid");
        const inputName = input.attributes.getNamedItem("name").value;
        const errorMessageElement = document.querySelector(".field-validation-error[data-valmsg-for="+inputName+"]");
        
        let popoverElement = input;
        if (input.type === "textarea" && input.id === "easyMDE") {
            let easyMDE = input.parentElement.querySelector(".EasyMDEContainer");
            easyMDE.classList.add("is-invalid");
            popoverElement = easyMDE;
        }
        
        if (errorMessageElement !== null) {
            const popover = new bootstrap.Popover(popoverElement, {
                placement: 'right',
                content: errorMessageElement.innerText,
                trigger: 'manual',
                customClass: 'custom-error-popover'
            });
            popover.show();
        }
    }
});