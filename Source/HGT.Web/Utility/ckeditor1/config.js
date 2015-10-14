/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here.
    // For complete reference see:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

    //// The toolbar groups arrangement, optimized for two toolbar rows.
    //config.toolbarGroups = [
	//	{ name: 'clipboard', groups: ['clipboard', 'undo'] },
	//	{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
	//	{ name: 'links' },
	//	{ name: 'insert' },
	//	{ name: 'forms' },
	//	{ name: 'tools' },
	//	{ name: 'document', groups: ['mode', 'document', 'doctools'] },
	//	{ name: 'others' },
	//	'/',
	//	{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
	//	{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
	//	{ name: 'styles' },
	//	{ name: 'colors' },
	//	{ name: 'about' }
    //];

    // define simple, basic, standard and full toolbar. ckeditor is belong to 4 toobars
    config.toolbar_simple =
    [
             ['Source'],
             ['Bold', 'Italic', 'RemoveFormat'],
             ['SimpleLink', 'Unlink'],
             ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord']
    ];

    config.toolbar_basic =
    [
             ['Source'],
             ['Bold', 'Italic', 'RemoveFormat'],
             ['Font', 'FontSize'],
             ['TextColor', 'BGColor'],
             ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
             ['SimpleLink', 'Unlink'],
             ['Image', 'Table'],
             ['Cut', 'Copy', 'Paste', 'PasteFromWord']
    ];

    config.toolbar_standard =
    [
            ['Source'],
            ['Bold', 'Italic', 'RemoveFormat'],
            ['Font', 'FontSize'],
            ['TextColor', 'BGColor'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
            ['SimpleLink', 'Unlink'],
            ['Image', 'Table'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
            ['Undo', 'Redo'],
            ['HorizontalRule', 'PageBreak'],
            ['basicstyles', 'cleanup'],           
            ['list', 'indent', 'blocks', 'align', 'bidi'],
            ['Cut', 'Copy', 'Paste', 'PasteFromWord']
            
    ];

    config.toolbar_full =
    [
            ['Source', '-', 'Preview', 'Print', '-', 'Templates'],
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
            ['find', 'selection', 'spellchecker'],
            ['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'],
            ['Maximize', 'ShowBlocks'],
            ['basicstyles', 'cleanup'],
            ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'],
            ['list', 'indent', 'blocks', 'align', 'bidi'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
            ['SimpleLink', 'Unlink', 'Anchor'],
            ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'],
            ['Styles', 'Format', 'Font', 'FontSize'],
            ['TextColor', 'BGColor']
    ];
    // Remove some buttons provided by the standard plugins, which are
    // not needed in the Standard(s) toolbar.
    config.removeButtons = 'Underline,Subscript,Superscript';

    // Set the most common block elements.
    config.format_tags = 'p;h1;h2;h3;pre';

    // Simplify the dialog windows.
    config.removeDialogTabs = 'image:advanced;link:advanced';
};
