function initEditor(inputNode) {
    if (!$.fn.clawEditor) setTimeout(initEditor, 500);
    else {
        var arr = (inputNode ? inputNode : $('.ckeditor_standard, .ckeditor_simple, .ckeditor_basic'));
        arr.each(function () {
            if ($(this).hasClass('ckeditor_simple')) {
                $(this).clawEditor('simple', { height: '50px' });
            }
            else if ($(this).hasClass('ckeditor_basic')) {
                $(this).clawEditor('basic', { height: '200px' });
            }
            else $(this).clawEditor('standard', { height: '380px' });
        });
    }
}
initEditor();